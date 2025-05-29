using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Paneles del Menú")]
    public GameObject panelMenuPrincipal;
    public GameObject panelPersonajes;
    public GameObject panelOpciones;
    
    [Header("Botones del Menú Principal")]
    public Button botonJugar;
    public Button botonNiveles; // Nuevo botón
    public Button botonPersonajes;
    public Button botonOpciones;
    public Button botonSalir;
    
    [Header("Botones de Navegación")]
    public Button botonVolverPersonajes;
    public Button botonVolverOpciones;
    
    [Header("Opciones de Audio")]
    public Slider sliderVolumenMusica;
    public Slider sliderVolumenEfectos;
    
    private int personajeSeleccionado = 0; // Índice del personaje seleccionado
    
    void Start()
    {
        // Mostrar solo el panel principal al inicio
        MostrarPanelPrincipal();
        
        // Configurar botones del menú principal
        if (botonJugar != null)
            botonJugar.onClick.AddListener(IniciarJuego);
        
        if (botonNiveles != null)
            botonNiveles.onClick.AddListener(IrSeleccionNiveles);
        
        if (botonPersonajes != null)
            botonPersonajes.onClick.AddListener(MostrarPanelPersonajes);
        
        if (botonOpciones != null)
            botonOpciones.onClick.AddListener(MostrarPanelOpciones);
        
        if (botonSalir != null)
            botonSalir.onClick.AddListener(SalirJuego);
        
        // Configura botones 
        if (botonVolverPersonajes != null)
            botonVolverPersonajes.onClick.AddListener(MostrarPanelPrincipal);
        
        if (botonVolverOpciones != null)
            botonVolverOpciones.onClick.AddListener(MostrarPanelPrincipal);
        
        // Configurar audio
        ConfigurarSlidersAudio();
        
        // Cargar configuraciones guardadas
        CargarConfiguraciones();
    }
    
    void MostrarPanelPrincipal()
    {
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(true);
        if (panelPersonajes != null) panelPersonajes.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(false);
    }
    
    void MostrarPanelPersonajes()
    {
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(false);
        if (panelPersonajes != null) panelPersonajes.SetActive(true);
        if (panelOpciones != null) panelOpciones.SetActive(false);
    }
    
    void MostrarPanelOpciones()
    {
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(false);
        if (panelPersonajes != null) panelPersonajes.SetActive(false);
        if (panelOpciones != null) panelOpciones.SetActive(true);
    }
    
    void IniciarJuego()
    {
        // Guarda el personaje seleccionado para usar en el juego
        PlayerPrefs.SetInt("PersonajeSeleccionado", personajeSeleccionado);
        PlayerPrefs.Save();
        
        // Cargar la escena del juego
        SceneManager.LoadScene("SampleScene");
    }
    
    void IrSeleccionNiveles()
    {
        SceneManager.LoadScene("SeleccionNiveles");
    }
    
    void SalirJuego()
    {
        Debug.Log("Saliendo del juego...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    void ConfigurarSlidersAudio()
    {
        if (sliderVolumenMusica != null)
        {
            sliderVolumenMusica.onValueChanged.AddListener(CambiarVolumenMusica);
        }
        
        if (sliderVolumenEfectos != null)
        {
            sliderVolumenEfectos.onValueChanged.AddListener(CambiarVolumenEfectos);
        }
    }
    
    void CambiarVolumenMusica(float volumen)
    {
        // Guardar el volumen de música
        PlayerPrefs.SetFloat("VolumenMusica", volumen);
        PlayerPrefs.Save();
        
        Debug.Log("Volumen música: " + volumen);
    }
    
    void CambiarVolumenEfectos(float volumen)
    {
        // Guardar el volumen de efectos
        PlayerPrefs.SetFloat("VolumenEfectos", volumen);
        PlayerPrefs.Save();
        
        Debug.Log("Volumen efectos: " + volumen);
    }
    
    void CargarConfiguraciones()
    {
        // Cargar personaje seleccionado
        personajeSeleccionado = PlayerPrefs.GetInt("PersonajeSeleccionado", 0);
        
        // Cargar volúmenes
        if (sliderVolumenMusica != null)
        {
            sliderVolumenMusica.value = PlayerPrefs.GetFloat("VolumenMusica", 0.8f);
        }
        
        if (sliderVolumenEfectos != null)
        {
            sliderVolumenEfectos.value = PlayerPrefs.GetFloat("VolumenEfectos", 0.8f);
        }
    }
}