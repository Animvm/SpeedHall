using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float tiempoMovimiento = 0.3f; // Tiempo que tarda en moverse de una celda a otra
    
    [Header("Configuración del Grid")]
    public float tamanoCelda = 1f; // Tamaño de cada celda del grid
    
    private Vector2 posicionObjetivo; // Hacia dónde se está moviendo el jugador
    private bool estaMoviendose = false; // Para evitar que se mueva mientras ya se está moviendo
    private Vector2 inputInicial; // Para detectar el swipe
    private bool tocandoPantalla = false;
    private Camera camaraJuego;
    
    void Start()
    {
        // Inicializa la posición objetivo como la posición actual
        posicionObjetivo = transform.position;
        camaraJuego = Camera.main;
    }
    
    void Update()
    {
        // Solo detecta input si no nos estamos moviendo
        if (!estaMoviendose)
        {
            DetectarSwipe();
            DetectarTeclado(); // Para probar el juego con el teclado
        }
        
        // Mueve hacia la posición objetivo
        MoverHaciaPosicionObjetivo();
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
            
            // detecta si el swipe es lo suficientemente largo
            if (direccionSwipe.magnitude > 0.5f)
            {
                ProcesarMovimiento(direccionSwipe);
            }
            
            tocandoPantalla = false;
        }
    }
    
    void ProcesarMovimiento(Vector2 direccion)
    {
        Vector2 nuevaPosicion = posicionObjetivo;
        
        // Determina la dirección predominante del swipe
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
        {
            // Movimiento horizontal
            if (direccion.x > 0)
                nuevaPosicion += Vector2.right * tamanoCelda; // Derecha
            else
                nuevaPosicion += Vector2.left * tamanoCelda; // Izquierda
        }
        else
        {
            // Movimiento vertical
            if (direccion.y > 0)
                nuevaPosicion += Vector2.up * tamanoCelda; // Arriba
            else
                nuevaPosicion += Vector2.down * tamanoCelda; // Abajo
        }
        
        // Verifica si la nueva posición es válida
        if (EsPosicionValida(nuevaPosicion))
        {
            posicionObjetivo = nuevaPosicion;
            estaMoviendose = true;
        }
    }
    
    bool EsPosicionValida(Vector2 posicion)
    {
         // Verifica si hay algún obstáculo en esa posición
        Collider2D obstaculo = Physics2D.OverlapCircle(posicion, 0.1f);
        
        // Si encuentra un collider que no sea el del jugador, la posición no es válida
        if (obstaculo != null && obstaculo.gameObject != gameObject)
        {
            return false;
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
            }
        }
    }
    
    // Función para moverse con teclado y poder probar el juego en el editor
    void DetectarTeclado()
    {
        Vector2 nuevaPosicion = posicionObjetivo;
        bool hayMovimiento = false;
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            nuevaPosicion += Vector2.up * tamanoCelda;
            hayMovimiento = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            nuevaPosicion += Vector2.down * tamanoCelda;
            hayMovimiento = true;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nuevaPosicion += Vector2.left * tamanoCelda;
            hayMovimiento = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            nuevaPosicion += Vector2.right * tamanoCelda;
            hayMovimiento = true;
        }
        
        if (hayMovimiento && EsPosicionValida(nuevaPosicion))
        {
            posicionObjetivo = nuevaPosicion;
            estaMoviendose = true;
        }
    }
}