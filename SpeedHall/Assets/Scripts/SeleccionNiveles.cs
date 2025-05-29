using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SeleccionNiveles : MonoBehaviour
{
    [Header("UI de Selección de Niveles")]
    public Button botonVolver; // Botón para volver al menú principal
    public Button[] botonesNiveles; // Array de botones de niveles
    public Text[] textosEstrellas; // Array que muestran las estrellas obtenidas
    public Text[] textosTiempos; // Textos que muestran los mejores tiempos
    
    [Header("Configuración de Niveles")]
    public string[] nombresEscenas; // Nombres de las escenas de cada nivel
    public int[] tiemposLimite; // Tiempo límite de cada nivel
    
    private int nivelMaximoDesbloqueado = 1; // Cuántos niveles están desbloqueados
    
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
        
        Debug.Log("Nivel máximo desbloqueado: " + nivelMaximoDesbloqueado);
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
            
            // Actualiza información de estrellas y tiempos
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
        int estrellasObtenidas = PlayerPrefs.GetInt("Estrellas_" + nombreEscena, 0);
        float mejorTiempo = PlayerPrefs.GetFloat("MejorTiempo_" + nombreEscena, 0f);
        
        // Actualiza texto de estrellas
        if (indice < textosEstrellas.Length && textosEstrellas[indice] != null)
        {
            if (estrellasObtenidas > 0)
            {
                string textoEstrellas = "";
                for (int j = 0; j < 3; j++)
                {
                    textoEstrellas += (j < estrellasObtenidas) ? "⭐" : "☆";
                }
                textosEstrellas[indice].text = textoEstrellas;
            }
            else
            {
                textosEstrellas[indice].text = "☆☆☆";
            }
        }
        
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
        Debug.Log("Cargando nivel: " + numeroNivel);
        
        // Determina el nombre de la escena
        string nombreEscena = "";
        if (numeroNivel - 1 < nombresEscenas.Length)
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
        
        if (nivelActual >= nivelMaximo)
        {
            PlayerPrefs.SetInt("NivelMaximo", nivelActual + 1);
            PlayerPrefs.Save();
            Debug.Log("Nivel " + (nivelActual + 1) + " desbloqueado!");
        }
    }
    
    // Función para guardar estadísticas del nivel
    public static void GuardarEstadisticasNivel(string nombreEscena, int estrellas)
    {
        // Guarda las mejores estrellas obtenidas
        int mejoresEstrellas = PlayerPrefs.GetInt("Estrellas_" + nombreEscena, 0);
        if (estrellas > mejoresEstrellas)
        {
            PlayerPrefs.SetInt("Estrellas_" + nombreEscena, estrellas);
            PlayerPrefs.Save();
        }
    }
}