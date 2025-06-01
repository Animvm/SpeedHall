using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SeleccionNiveles : MonoBehaviour
{
    [Header("UI de Selección de Niveles")]
    public Button botonVolver;
    public Button[] botonesNiveles;
    public Text[] textosTiempos;
    
    [Header("Configuración de Niveles")]
    public string[] nombresEscenas;
    
    // Variables privadas
    private int nivelMaximoDesbloqueado = 1;
    
    void Start()
    {
        // Carga progreso del jugador
        CargarProgreso();
        
        // Configura botones
        ConfigurarBotones();
        
        // Actualiza interfaz
        ActualizarInterfazNiveles();
    }
    
    void CargarProgreso()
    {
        // Carga el nivel máximo desbloqueado
        nivelMaximoDesbloqueado = PlayerPrefs.GetInt("NivelMaximo", 1);
    }
    
    void ConfigurarBotones()
    {
        // Configura botón de volver
        if (botonVolver != null)
            botonVolver.onClick.AddListener(VolverAlMenu);
        
        // Configura botones de niveles
        for (int i = 0; i < botonesNiveles.Length; i++)
        {
            if (botonesNiveles[i] != null)
            {
                int numeroNivel = i + 1; // Los niveles empiezan en 1
                botonesNiveles[i].onClick.AddListener(() => CargarNivel(numeroNivel));
            }
        }
    }
    
    void ActualizarInterfazNiveles()
    {
        for (int i = 0; i < botonesNiveles.Length; i++)
        {
            int numeroNivel = i + 1;
            
            if (botonesNiveles[i] != null)
            {
                // Verifica si el nivel está desbloqueado
                bool nivelDesbloqueado = numeroNivel <= nivelMaximoDesbloqueado;
                
                // Configura el botón
                botonesNiveles[i].interactable = nivelDesbloqueado;
                
                // Cambia apariencia del botón según esté desbloqueado o no
                Image imagenBoton = botonesNiveles[i].GetComponent<Image>();
                if (imagenBoton != null)
                {
                    imagenBoton.color = nivelDesbloqueado ? Color.white : Color.gray;
                }
                
                // Configura texto del botón
                Text textoBoton = botonesNiveles[i].GetComponentInChildren<Text>();
                if (textoBoton != null)
                {
                    textoBoton.text = nivelDesbloqueado ? numeroNivel.ToString() : "🔒";
                }
            }
            
            // Actualiza información de tiempos
            ActualizarEstadisticasNivel(i, numeroNivel);
        }
    }
    
    void ActualizarEstadisticasNivel(int indice, int numeroNivel)
    {
        // Obtiene el nombre de la escena para este nivel
        string nombreEscena = "";
        if (indice < nombresEscenas.Length)
        {
            nombreEscena = nombresEscenas[indice];
        }
        else
        {
            // Nombres por defecto si no están configurados
            nombreEscena = "Level" + numeroNivel;
        }
        
        // Carga estadísticas guardadas
        float mejorTiempo = PlayerPrefs.GetFloat("MejorTiempo_" + nombreEscena, 0f);
        
        // Actualiza texto de tiempo
        if (indice < textosTiempos.Length && textosTiempos[indice] != null)
        {
            if (mejorTiempo > 0)
            {
                int minutos = Mathf.FloorToInt(mejorTiempo / 60);
                int segundos = Mathf.FloorToInt(mejorTiempo % 60);
                
                if (minutos > 0)
                {
                    textosTiempos[indice].text = string.Format("{0}:{1:00}", minutos, segundos);
                }
                else
                {
                    textosTiempos[indice].text = segundos + "s";
                }
            }
            else
            {
                textosTiempos[indice].text = "--";
            }
        }
    }
    
    void CargarNivel(int numeroNivel)
    {
        // Determina el nombre de la escena
        string nombreEscena = "";
        if (numeroNivel - 1 < nombresEscenas.Length && !string.IsNullOrEmpty(nombresEscenas[numeroNivel - 1]))
        {
            nombreEscena = nombresEscenas[numeroNivel - 1];
        }
        else
        {
            // Nombre por defecto
            nombreEscena = "Level" + numeroNivel;
        }
        
        // Guarda el nivel seleccionado para uso en el juego
        PlayerPrefs.SetInt("NivelActual", numeroNivel);
        PlayerPrefs.Save();
        
        // Carga la escena del nivel
        SceneManager.LoadScene(nombreEscena);
    }
    
    void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
    
    // Función pública para desbloquear el siguiente nivel
    public static void DesbloquearSiguienteNivel()
    {
        int nivelActual = PlayerPrefs.GetInt("NivelActual", 1);
        int nivelMaximo = PlayerPrefs.GetInt("NivelMaximo", 1);
        
        // Si el nivel actual completado es mayor o igual al máximo desbloqueado
        if (nivelActual >= nivelMaximo)
        {
            int nuevoMaximo = nivelActual + 1;
            PlayerPrefs.SetInt("NivelMaximo", nuevoMaximo);
            PlayerPrefs.Save();
        }
    }
}