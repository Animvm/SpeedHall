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
    public Button botonNiveles;
    public Button botonPersonajes;
    public Button botonOpciones;
    public Button botonSalir;
    
    [Header("Botones de Navegación")]
    public Button botonVolverPersonajes;
    public Button botonVolverOpciones;
    
    [Header("Botones de Debug (Testing)")]
    public Button botonResetearProgreso;
    public Button botonDesbloquearNivel2;
    
    void Start()
    {
        // Muestra solo el panel principal al inicio
        MostrarPanelPrincipal();
        
        // Configura botones del menú principal
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
        
        // Configura botones de navegación
        if (botonVolverPersonajes != null)
            botonVolverPersonajes.onClick.AddListener(MostrarPanelPrincipal);
        
        if (botonVolverOpciones != null)
            botonVolverOpciones.onClick.AddListener(MostrarPanelPrincipal);
        
        // Configura botones de debug (solo en editor)
        #if UNITY_EDITOR
        if (botonResetearProgreso != null)
            botonResetearProgreso.onClick.AddListener(ResetearProgreso);
        
        if (botonDesbloquearNivel2 != null)
            botonDesbloquearNivel2.onClick.AddListener(DesbloquearNivel2);
        #endif
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
        // Obtiene el nivel máximo desbloqueado
        int nivelMaximo = PlayerPrefs.GetInt("NivelMaximo", 1);
        
        // Limita al número máximo de niveles que realmente existen
        int totalNivelesExistentes = 2;
        if (nivelMaximo > totalNivelesExistentes)
        {
            nivelMaximo = totalNivelesExistentes;
        }
        
        // Va al nivel más alto disponible
        string nombreEscena = "Level" + nivelMaximo;
        
        // Guarda el nivel actual
        PlayerPrefs.SetInt("NivelActual", nivelMaximo);
        PlayerPrefs.Save();
        
        // Carga la escena del nivel
        SceneManager.LoadScene(nombreEscena);
    }
    
    void IrSeleccionNiveles()
    {
        SceneManager.LoadScene("SeleccionNiveles");
    }
    
    void SalirJuego()
    {        
        // En el editor de Unity
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // En la aplicación compilada
            Application.Quit();
        #endif
    }
    
    // Funciones de debug para testing
    void ResetearProgreso()
    {
        // Limpia todos los PlayerPrefs del juego
        PlayerPrefs.DeleteKey("NivelMaximo");
        PlayerPrefs.DeleteKey("NivelActual");
        PlayerPrefs.DeleteKey("MejorTiempo_Level1");
        PlayerPrefs.DeleteKey("MejorTiempo_Level2");
        PlayerPrefs.Save();
        
        // Recarga la escena para actualizar la interfaz
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void DesbloquearNivel2()
    {
        PlayerPrefs.SetInt("NivelMaximo", 2);
        PlayerPrefs.Save();
    }
}