using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Configuración del Nivel")]
    public float tiempoLimite = 30f;
    public Transform puntoInicio;
    public Transform puntoMeta;
    
    [Header("UI del Juego")]
    public Text textoTiempo;
    public ResultadoNivel resultadoNivel;
    
    // Variables privadas
    private float tiempoRestante;
    private bool juegoTerminado = false;
    private PlayerController jugador;
    
    void Start()
    {
        // Inicializa el tiempo
        tiempoRestante = tiempoLimite;
        
        // Detecta y guarda qué nivel es este basado en el nombre de la escena
        string nombreEscena = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        
        // Extrae el número del nivel del nombre de la escena
        if (nombreEscena.Contains("Level"))
        {
            string numeroStr = nombreEscena.Replace("Level", "");
            if (int.TryParse(numeroStr, out int numeroNivel))
            {
                PlayerPrefs.SetInt("NivelActual", numeroNivel);
                PlayerPrefs.Save();
            }
        }
        
        // Encuentra al jugador
        jugador = FindObjectOfType<PlayerController>();
        
        // Posiciona al jugador en el inicio
        if (puntoInicio != null && jugador != null)
        {
            // Posiciona al jugador en el grid más cercano al punto de inicio
            Vector3 posicionInicio = puntoInicio.position;
            Vector3 posicionGrid = new Vector3(
                Mathf.Round(posicionInicio.x),
                Mathf.Round(posicionInicio.y),
                0
            );
            
            jugador.transform.position = posicionGrid;
            
            // Llama a una función para reinicializar el PlayerController
            jugador.InicializarPosicion(posicionGrid);
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
            if (distancia < 0.5f)
            {
                Victoria();
            }
        }
    }
    
    void Victoria()
    {
        juegoTerminado = true;
        
        // Calcula estrellas y muestra resultado
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
        
        return estrellas;
    }
    
    // Función para reiniciar el nivel
    public void ReiniciarNivel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}