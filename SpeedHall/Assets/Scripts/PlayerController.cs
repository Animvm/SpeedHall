using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float tiempoMovimiento = 0.1f; // Tiempo que tarda en moverse (más rápido)
    
    [Header("Configuración del Grid")]
    public float tamanoCelda = 0.5f; // Tamaño de cada celda del grid (más pequeño)
    
    private Vector2 posicionObjetivo; // Hacia dónde se está moviendo el jugador
    private bool estaMoviendose = false; // Para evitar que se mueva mientras ya se está moviendo
    private Vector2 inputInicial; // Para detectar el swipe
    private bool tocandoPantalla = false;
    private Camera camaraJuego;
        private Vector2 direccionMovimiento = Vector2.zero; // Dirección actual de movimiento
    private bool moviendoseContinuamente = false; // Si está en movimiento continuo
    private float tiempoUltimoMovimiento = 0f; // Para controlar el timing
    
    void Start()
    {
        // Inicializa la posición objetivo como la posición actual
        // Pero ajustada al grid más cercano
        Vector2 posicionActual = transform.position;
        posicionObjetivo = new Vector2(
            Mathf.Round(posicionActual.x / tamanoCelda) * tamanoCelda,
            Mathf.Round(posicionActual.y / tamanoCelda) * tamanoCelda
        );
        
        // Posiciona inmediatamente en el grid
        transform.position = posicionObjetivo;
        
        camaraJuego = Camera.main;
    }
    
    // Función para que el LevelManager pueda reinicializar la posición
    public void InicializarPosicion(Vector3 nuevaPosicion)
    {
        posicionObjetivo = nuevaPosicion;
        transform.position = nuevaPosicion;
        estaMoviendose = false;
    }
    
    void Update()
    {
        // Detecta input para cambiar dirección
        DetectarInput();
        
        // Movimiento continuo
        if (moviendoseContinuamente)
        {
            MoverContinuamente();
        }
        
        // Mueve hacia la posición objetivo
        MoverHaciaPosicionObjetivo();
    }
    
    // Función para detectar input
    void DetectarInput()
    {
        // Detecta teclado (solo para testing en pc)
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            CambiarDireccion(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            CambiarDireccion(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CambiarDireccion(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            CambiarDireccion(Vector2.right);
        }
        
        // Detectar swipe 
        DetectarSwipe();
    }
    
    void CambiarDireccion(Vector2 nuevaDireccion)
    {
        // Solo cambia si no es la misma dirección o si no se está moviendo
        if (direccionMovimiento != nuevaDireccion || !moviendoseContinuamente)
        {
            direccionMovimiento = nuevaDireccion;
            moviendoseContinuamente = true;
            
            // Si no se está moviendo actualmente, empezar inmediatamente
            if (!estaMoviendose)
            {
                IntentarSiguienteMovimiento();
            }
        }
    }
    
    void MoverContinuamente()
    {
        // Solo intentar moverse si no estamos en proceso de movimiento
        if (!estaMoviendose)
        {
            IntentarSiguienteMovimiento();
        }
    }
    
    void IntentarSiguienteMovimiento()
    {
        if (!moviendoseContinuamente) return;
        
        Vector2 siguientePosicion = posicionObjetivo + (direccionMovimiento * tamanoCelda);
        
        if (EsPosicionValida(siguientePosicion))
        {
            // Moverse a la siguiente posición
            posicionObjetivo = siguientePosicion;
            estaMoviendose = true;
        }
        else
        {
            // Si choca con algo detiene el movimiento continuo
            moviendoseContinuamente = false;
            Debug.Log("Chocó con obstáculo, deteniendo movimiento");
        }
    }
    
    bool EsPosicionValida(Vector2 posicion)
    {
         // Verifica si hay algún obstáculo en esa posición
        Collider2D obstaculo = Physics2D.OverlapCircle(posicion, 0.1f);
        
        // Si encuentra un collider que no sea el del jugador, la posición no es válida
        if (obstaculo != null && obstaculo.gameObject != gameObject)
        {
            // Verifica si el obstáculo tiene el tag "Pared" o "Obstaculo"
            if (obstaculo.CompareTag("Untagged") || obstaculo.name.Contains("Pared"))
            {
                Debug.Log("Colisión detectada con: " + obstaculo.gameObject.name);
                return false;
            }
        }
        
        return true;
    }
    
    void MoverHaciaPosicionObjetivo()
    {
        if (estaMoviendose)
        {
            // Mover hacia la posición objetivo
            transform.position = Vector2.MoveTowards(transform.position, posicionObjetivo, 
                                                   (tamanoCelda / tiempoMovimiento) * Time.deltaTime);
            
            // Verifica si ya llegamos al objetivo
            if (Vector2.Distance(transform.position, posicionObjetivo) < 0.01f)
            {
                transform.position = posicionObjetivo;
                estaMoviendose = false;
                
                // Si está en movimiento continuo, intenta los siguiente movimiento
                if (moviendoseContinuamente)
                {
                    IntentarSiguienteMovimiento();
                }
            }
        }
    }
    
    void DetectarSwipe()
    {
        // Detecta cuando el jugador toca la pantalla
        if (Input.GetMouseButtonDown(0))
        {
            // Convierte posición del mouse a posición del mundo
            Vector3 posicionMundo = camaraJuego.ScreenToWorldPoint(Input.mousePosition);
            inputInicial = new Vector2(posicionMundo.x, posicionMundo.y);
            tocandoPantalla = true;
        }
        
        // Detecta cuando el jugador levanta el dedo
        if (Input.GetMouseButtonUp(0) && tocandoPantalla)
        {
            Vector3 posicionMundo = camaraJuego.ScreenToWorldPoint(Input.mousePosition);
            Vector2 inputFinal = new Vector2(posicionMundo.x, posicionMundo.y);
            
            // Calcula la dirección del swipe
            Vector2 direccionSwipe = inputFinal - inputInicial;
            
            // Solo procesa si el swipe es lo suficientemente largo
            if (direccionSwipe.magnitude > 0.5f)
            {
                ProcesarSwipe(direccionSwipe);
            }
            
            tocandoPantalla = false;
        }
    }
    
    void ProcesarSwipe(Vector2 direccion)
    {
        Vector2 nuevaDireccion = Vector2.zero;
        
        // Determina la dirección predominante del swipe
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
        {
            // Movimiento horizontal
            if (direccion.x > 0)
                nuevaDireccion = Vector2.right; // Derecha
            else
                nuevaDireccion = Vector2.left; // Izquierda
        }
        else
        {
            // Movimiento vertical
            if (direccion.y > 0)
                nuevaDireccion = Vector2.up; // Arriba
            else
                nuevaDireccion = Vector2.down; // Abajo
        }
        
        CambiarDireccion(nuevaDireccion);
    }
}