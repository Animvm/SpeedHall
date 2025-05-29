using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultadoNivel : MonoBehaviour
{
    [Header("Elementos del Pop-up")]
    public GameObject panelResultado; // El panel completo del pop-up
    public Text textoTitulo; // "¡NIVEL COMPLETADO!" o "GAME OVER"
    public Text textoTiempo; // Tiempo usado o restante
    public Text textoRecord; // Mensaje de nuevo récord
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
    
    // Variables para récords
    private string nombreEscena;
    private float mejorTiempo = 0f;
    private bool esNuevoRecord = false;

    void Start()
    {
        // Oculta el panel al inicio 
        if (panelResultado != null)
        {
            panelResultado.SetActive(false);
        }

        // Obtiene el nombre de la escena para guardar récords
        nombreEscena = SceneManager.GetActiveScene().name;
        
        // Carga el mejor tiempo guardado
        mejorTiempo = PlayerPrefs.GetFloat("MejorTiempo_" + nombreEscena, 0f);

        ConfigurarBotones();
    }

    void ConfigurarBotones()
    {
        if (botonSiguienteNivel != null)
            botonSiguienteNivel.onClick.AddListener(IrSiguienteNivel);

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

        // Verifica si es un nuevo récord
        VerificarRecord();

        MostrarPanel();
        ConfigurarTextos();
        ConfigurarBotonesVictoria();

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
        esNuevoRecord = false;

        MostrarPanel();
        ConfigurarTextos();
        ConfigurarBotonesDerrota();

        if (efectoDerrota != null)
            efectoDerrota.SetActive(true);

        if (efectoVictoria != null)
            efectoVictoria.SetActive(false);
    }

    void VerificarRecord()
    {
        // Si no hay récord previo o el tiempo actual es mejor
        if (mejorTiempo == 0f || tiempoUsado < mejorTiempo)
        {
            esNuevoRecord = true;
            mejorTiempo = tiempoUsado;
            
            // Guarda el nuevo récord
            PlayerPrefs.SetFloat("MejorTiempo_" + nombreEscena, mejorTiempo);
            PlayerPrefs.Save();
            
        }
        else
        {
            esNuevoRecord = false;
        }
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

    void ConfigurarTextos()
    {
        // Configura el título
        if (textoTitulo != null)
        {
            if (nivelCompletado)
            {
                textoTitulo.text = "¡NIVEL COMPLETADO!";
            }
            else
            {
                textoTitulo.text = "GAME OVER";
            }
        }

        // Configura el texto del tiempo 
        if (textoTiempo != null)
        {
            if (nivelCompletado)
            {
                int minutos = Mathf.FloorToInt(tiempoUsado / 60);
                int segundos = Mathf.FloorToInt(tiempoUsado % 60);
                
                if (minutos > 0)
                {
                    textoTiempo.text = string.Format("Tiempo: {0}:{1:00}", minutos, segundos);
                }
                else
                {
                    textoTiempo.text = string.Format("Tiempo: {0} segundos", segundos);
                }
            }
            else
            {
                textoTiempo.text = "¡Se acabó el tiempo!";
            }
        }

        // Configura mensaje de récord 
        if (textoRecord != null)
        {
            if (nivelCompletado && esNuevoRecord)
            {
                textoRecord.text = "¡NUEVO RÉCORD!";
                textoRecord.gameObject.SetActive(true);
            }
            else if (nivelCompletado && mejorTiempo > 0)
            {
                int mejorMinutos = Mathf.FloorToInt(mejorTiempo / 60);
                int mejorSegundos = Mathf.FloorToInt(mejorTiempo % 60);
                
                if (mejorMinutos > 0)
                {
                    textoRecord.text = string.Format("Récord: {0}:{1:00}", mejorMinutos, mejorSegundos);
                }
                else
                {
                    textoRecord.text = string.Format("Récord: {0} segundos", mejorSegundos);
                }
                
                textoRecord.gameObject.SetActive(true);
            }
            else
            {
                textoRecord.gameObject.SetActive(false);
            }
        }
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

    void IrSiguienteNivel()
    {
        Time.timeScale = 1f; // Reanuda el tiempo
        
        // Por ahora reinicia el mismo nivel 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    void ReiniciarNivel()
    {
        Time.timeScale = 1f; // Reanuda el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void IrMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}