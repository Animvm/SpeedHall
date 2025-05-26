using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class LevelManager : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    public float tiempoLimite = 30f; // Tiempo límite 
    public Transform puntoInicio; // Donde empieza el jugador
    public Transform puntoMeta; // Donde debe llegar 
    
    [Header("UI del Juego")]
    public Text textoTiempo; // Para mostrar el tiempo restante
    public GameObject panelVictoria; // Panel que aparece al ganar
    public GameObject panelDerrota; // Panel que aparece al perder
    
    private float tiempoRestante;
    private bool juegoTerminado = false;
    private PlayerController jugador;
    
    void Start()
    {
        // Inicializa el tiempo
        tiempoRestante = tiempoLimite;
        
        // Encontrar el jugador
        jugador = FindObjectOfType<PlayerController>();
        
        // Posiciona al jugador en el punto de inicio
        if (puntoInicio != null && jugador != null)
        {
            jugador.transform.position = puntoInicio.position;
        }
        
        // Ocultar paneles al inicio
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(false);
    }
    
    void Update()
    {
        if (!juegoTerminado)
        {
            // Restar tiempo
            tiempoRestante -= Time.deltaTime;
            
            // Actualiza UI del tiempo
            ActualizarTextoTiempo();
            
            // Verificar si se acabó el tiempo
            if (tiempoRestante <= 0)
            {
                GameOver();
            }
            
            // Verifica si el jugador llegó a la meta
            VerificarVictoria();
        }
    }
    
    void ActualizarTextoTiempo()
    {
        if (textoTiempo != null)
        {
            int segundos = Mathf.CeilToInt(tiempoRestante);
            textoTiempo.text = "Tiempo: " + segundos;
        }
    }
    
    void VerificarVictoria()
    {
        if (jugador != null && puntoMeta != null)
        {
            // Verifica si el jugador está cerca de la meta
            float distancia = Vector2.Distance(jugador.transform.position, puntoMeta.position);
            if (distancia < 0.5f) // Si está muy cerca de la meta
            {
                Victoria();
            }
        }
    }
    
    void Victoria()
    {
        juegoTerminado = true;
        Debug.Log("Ganaste!!!!");
        
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }
        
        // Calcula las estrellas basadas en tiempo restante
        CalcularEstrellas();
    }
    
    void GameOver()
    {
        juegoTerminado = true;
        tiempoRestante = 0;
        Debug.Log("Se acabó el tiempo :c");
        
        if (panelDerrota != null)
        {
            panelDerrota.SetActive(true);
        }
    }
    
    void CalcularEstrellas()
    {
        int estrellas = 1; // Al menos 1 estrella por completar
        
        // 2 estrellas si queda más del 30% del tiempo
        if (tiempoRestante > tiempoLimite * 0.3f)
        {
            estrellas = 2;
        }
        
        // 3 estrellas si queda más del 60% del tiempo
        if (tiempoRestante > tiempoLimite * 0.6f)
        {
            estrellas = 3;
        }
        
        Debug.Log("Estrellas obtenidas: " + estrellas);
    }
    
    // Función para reiniciar el nivel
    public void ReiniciarNivel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}