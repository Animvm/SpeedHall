using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultadoNivel : MonoBehaviour
{
    [Header("Elementos del Pop-up")]
    public GameObject panelResultado; // El panel completo del pop-up
    public Text textoTitulo; // "Ganaste!" o "Game Over"
    public Text textoTiempo; // Tiempo usado o restante
    public Image[] imagenesEstrellas; // Array de 3 imágenes de estrellas
    public Sprite estrellaLlena; // Sprite de estrella dorada
    public Sprite estrellaVacia; // Sprite de estrella gris

    [Header("Botones")]
    public Button botonSiguienteNivel;
    public Button botonReiniciarNivel;
    public Button botonMenuPrincipal;

    [Header("Efectos Visuales")]
    public GameObject efectoVictoria; // Efecto de victoria
    public GameObject efectoDerrota; // Efectos para game over

    private bool nivelCompletado = false;
    private int estrellasObtenidas = 0;
    private float tiempoUsado = 0f;
    private float tiempoLimite = 0f;

    void Start()
    {
        // Oculta el panel al inicio 
        if (panelResultado != null)
        {
            panelResultado.SetActive(false);
            Debug.Log("Pop-up ocultado al inicio");
        }

        ConfigurarBotones();
    }

    void ConfigurarBotones()
    {

        if (botonReiniciarNivel != null)
            botonReiniciarNivel.onClick.AddListener(ReiniciarNivel);

        if (botonMenuPrincipal != null)
            botonMenuPrincipal.onClick.AddListener(IrMenuPrincipal);
    }

    // Función para mostrar resultado de victoria
    public void MostrarVictoria(int estrellas, float tiempoRestante, float tiempoTotal)
    {
        nivelCompletado = true;
        estrellasObtenidas = estrellas;
        tiempoUsado = tiempoTotal - tiempoRestante;
        tiempoLimite = tiempoTotal;

        MostrarPanel();

        if (efectoVictoria != null)
            efectoVictoria.SetActive(true);

        if (efectoDerrota != null)
            efectoDerrota.SetActive(false);
    }

    // Función para mostrar resultado de derrota
    public void MostrarDerrota(float tiempoUsado, float tiempoTotal)
    {
        nivelCompletado = false;
        estrellasObtenidas = 0;
        this.tiempoUsado = tiempoUsado;
        tiempoLimite = tiempoTotal;

        MostrarPanel();

        if (efectoDerrota != null)
            efectoDerrota.SetActive(true);

        if (efectoVictoria != null)
            efectoVictoria.SetActive(false);
    }

    void MostrarPanel()
    {
        if (panelResultado != null)
        {
            panelResultado.SetActive(true);
            // Pausar el juego
            Time.timeScale = 0f;
        }
    }

    void IrMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
    void ConfigurarBotonesVictoria()
    {
        // En victoria, mostrar botón de siguiente nivel
        if (botonSiguienteNivel != null)
            botonSiguienteNivel.gameObject.SetActive(true);

        if (botonReiniciarNivel != null)
            botonReiniciarNivel.gameObject.SetActive(true);

        if (botonMenuPrincipal != null)
            botonMenuPrincipal.gameObject.SetActive(true);
    }

    void ConfigurarBotonesDerrota()
    {
        // En derrota, ocultar botón de siguiente nivel
        if (botonSiguienteNivel != null)
            botonSiguienteNivel.gameObject.SetActive(false);

        if (botonReiniciarNivel != null)
            botonReiniciarNivel.gameObject.SetActive(true);

        if (botonMenuPrincipal != null)
            botonMenuPrincipal.gameObject.SetActive(true);
    }
    
    void ReiniciarNivel()
    {
        Time.timeScale = 1f; // Reanuda el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}