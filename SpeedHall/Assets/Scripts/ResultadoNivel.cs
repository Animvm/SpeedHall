using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultadoNivel : MonoBehaviour
{
    [Header("Elementos del Pop-up")]
    public GameObject panelResultado;
    public Text textoTitulo;
    public Text textoTiempo;
    public Text textoRecord;

    [Header("Botones")]
    public Button botonSiguienteNivel;
    public Button botonReiniciarNivel;
    public Button botonMenuPrincipal;

    [Header("Configuración de Niveles")]
    public int totalNivelesDisponibles = 2;
    
    // Variables privadas
    private bool nivelCompletado = false;
    private float tiempoUsado = 0f;
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
        tiempoUsado = tiempoTotal - tiempoRestante;

        // Verifica si es un nuevo récord
        VerificarRecord();
        
        // Desbloquea el siguiente nivel
        SeleccionNiveles.DesbloquearSiguienteNivel();

        MostrarPanel();
        ConfigurarTextos();
        ConfigurarBotonesVictoria();
    }

    // Función para mostrar resultado de derrota
    public void MostrarDerrota(float tiempoUsado, float tiempoTotal)
    {
        nivelCompletado = false;
        this.tiempoUsado = tiempoUsado;
        esNuevoRecord = false;

        MostrarPanel();
        ConfigurarTextos();
        ConfigurarBotonesDerrota();
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
            // Pausa el juego
            Time.timeScale = 0f;
        }
    }

    void ConfigurarTextos()
    {
        // Configura título - solo cambia el texto
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

        // Configura texto de tiempo - solo el contenido
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
                // Formatea el tiempo del nuevo récord
                int segundos = Mathf.RoundToInt(tiempoUsado);
                string tiempoTexto = (segundos == 1) ? "segundo" : "segundos";
                
                textoRecord.text = string.Format("¡NUEVO RÉCORD! {0} {1}", segundos, tiempoTexto);
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
        // Obtiene el nivel actual
        int nivelActual = PlayerPrefs.GetInt("NivelActual", 1);
        
        // Verifica si hay un siguiente nivel disponible
        bool hayMasNiveles = nivelActual < totalNivelesDisponibles;
        
        // Configura botón de siguiente nivel
        if (botonSiguienteNivel != null)
        {
            botonSiguienteNivel.gameObject.SetActive(hayMasNiveles);
            
            if (hayMasNiveles)
            {
                botonSiguienteNivel.interactable = true;
            }
        }

        if (botonReiniciarNivel != null)
            botonReiniciarNivel.gameObject.SetActive(true);

        if (botonMenuPrincipal != null)
            botonMenuPrincipal.gameObject.SetActive(true);
    }

    void ConfigurarBotonesDerrota()
    {
        // En derrota, oculta botón de siguiente nivel
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
        
        // Obtiene el nivel actual
        int nivelActual = PlayerPrefs.GetInt("NivelActual", 1);
        int siguienteNivel = nivelActual + 1;
        
        // Verifica si existe el siguiente nivel
        if (siguienteNivel <= totalNivelesDisponibles)
        {
            string siguienteEscena = "Level" + siguienteNivel;
            
            // Guarda el nuevo nivel actual
            PlayerPrefs.SetInt("NivelActual", siguienteNivel);
            PlayerPrefs.Save();
            
            SceneManager.LoadScene(siguienteEscena);
        }
        else
        {
            // No hay más niveles, vuelve al menú de selección
            SceneManager.LoadScene("SeleccionNiveles");
        }
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