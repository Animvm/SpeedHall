using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    public float tiempoLimite = 30f; // Tiempo límite 
    public Transform puntoInicio; // Donde empieza el jugador
    public Transform puntoMeta; // Donde debe llegar 
    
    [Header("UI del Juego")]
    public Text textoTiempo; // Muestra el tiempo restante
    public ResultadoNivel resultadoNivel; // Referencia al script del pop-up
    
    private float tiempoRestante;
    private bool juegoTerminado = false;
    private PlayerController jugador;
    
    void Start()
    {
        // Inicializa el tiempo
        tiempoRestante = tiempoLimite;
        
        // Encuentra al jugador
        jugador = FindObjectOfType<PlayerController>();
        
        // Posiciona al jugador en el inicio
        if (puntoInicio != null && jugador != null)
        {
            jugador.transform.position = puntoInicio.position;
        }
    }
    
    void Update()
    {
        if (!juegoTerminado)
        {
            // Va restando tiempo
            tiempoRestante -= Time.deltaTime;
            
            // Actualiza texto del tiempo
            ActualizarTextoTiempo();
            
            // Verifica si se acabó el tiempo
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
        //Debug.Log("Ganaste!!");
        
        // Calcula estrellas y mostra resultado (estrellas dependerán del tiempo restante)
        int estrellas = CalcularEstrellas();
        
        if (resultadoNivel != null)
        {
            resultadoNivel.MostrarVictoria(estrellas, tiempoRestante, tiempoLimite);
        }
    }
    
    void GameOver()
    {
        juegoTerminado = true;
        tiempoRestante = 0;
        //Debug.Log("Se acabó el tiempo");
        
        if (resultadoNivel != null)
        {
            resultadoNivel.MostrarDerrota(tiempoLimite, tiempoLimite);
        }
    }
    
    int CalcularEstrellas()
    {
        int estrellas = 1; // Al menos 1 estrella por completar nivel
        
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
        
        //Debug.Log("Estrellas obtenidas: " + estrellas);
        return estrellas;
    }
    
    // Función para reiniciar el nivel
    public void ReiniciarNivel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}