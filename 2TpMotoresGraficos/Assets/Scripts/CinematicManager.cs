using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CinematicManager : MonoBehaviour 
{
    [Header("Imágenes de la Cinemática")]
    public Sprite[] imagenesHistoria; // funcion: Array de sprites que representan las imágenes de la cinemática.

    [Header("UI Referencias")]
    public Image imagenPantalla; // funcion: Referencia al componente Image donde se muestran las imágenes.

    [Header("Configuración")]
    public string escenaSiguiente = "GameScene"; // funcion: Nombre de la escena a cargar al finalizar la cinemática.
    public float tiempoFadeIn = 0.5f; // funcion: Duración del efecto de aparición de imagen.
    public float tiempoFadeOut = 0.5f; // funcion: Duración del efecto de desaparición de imagen.
    public KeyCode teclaContinuar = KeyCode.E; // funcion: Tecla para avanzar la cinemática.

    [Header("Audio")]
    public AudioClip[] sonidosPorImagen; // funcion: Array de sonidos para cada imagen.
    public AudioClip musicaFondo; // funcion: Música de fondo de la cinemática.
    private AudioSource audioSource; // funcion: Referencia al componente AudioSource.

    private int indiceActual = 0; // funcion: Índice de la imagen actual.
    private bool puedeAvanzar = true; // funcion: Indica si se puede avanzar a la siguiente imagen.
    private bool cinematicaTerminada = false; // funcion: Indica si la cinemática ha terminado.

    void Start() 
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // funcion: Añade un componente AudioSource al objeto.

        if (musicaFondo != null) 
        {
            audioSource.clip = musicaFondo; // funcion: Asigna la música de fondo al AudioSource.
            audioSource.loop = true; // funcion: Activa el loop de la música.
            audioSource.volume = 0.3f; // funcion: Ajusta el volumen de la música.
            audioSource.Play(); // funcion: Reproduce la música de fondo.
        }

        if (imagenesHistoria == null || imagenesHistoria.Length == 0) // funcion: Verifica que haya imágenes configuradas.
        {
            
            return;
        }

        if (imagenPantalla == null) // funcion: Verifica que la imagen de pantalla esté asignada.
        {
            
            return;
        }

        Cursor.lockState = CursorLockMode.None; // funcion: Libera el cursor.
        Cursor.visible = true; // funcion: Hace visible el cursor.

        MostrarImagen(0); // funcion: Muestra la primera imagen de la cinemática.

        
    }

    void Update() 
    {
        if (cinematicaTerminada) return; // funcion: Si la cinemática terminó, no hace nada.

        if (Input.GetKeyDown(teclaContinuar) && puedeAvanzar) // funcion: Si se presiona la tecla para avanzar y se puede avanzar.
        {
            AvanzarCinematica(); // funcion: Avanza a la siguiente imagen.
        }

        if (Input.GetMouseButtonDown(0) && puedeAvanzar) // funcion: Si se hace clic izquierdo y se puede avanzar.
        {
            AvanzarCinematica(); // funcion: Avanza a la siguiente imagen.
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // funcion: Si se presiona ESC.
        {
            SaltarCinematica(); // funcion: Salta la cinemática.
        }
    }

    void AvanzarCinematica() // funcion: Avanza a la siguiente imagen de la cinemática.
    {
        indiceActual++; // funcion: Incrementa el índice de la imagen.

        if (indiceActual < imagenesHistoria.Length) // funcion: Si hay más imágenes por mostrar.
        {
            StartCoroutine(TransicionImagen(indiceActual)); // funcion: Inicia la transición a la siguiente imagen.
        }
        else
        {
            TerminarCinematica(); // funcion: Termina la cinemática si no hay más imágenes.
        }
    }

    void MostrarImagen(int indice) // funcion: Muestra la imagen correspondiente al índice.
    {
        if (indice >= imagenesHistoria.Length) return; // funcion: Si el índice es inválido, no hace nada.

        if (audioSource != null) // funcion: Si existe el AudioSource.
        {
            audioSource.Stop(); // funcion: Detiene cualquier sonido que esté reproduciéndose.
        }

        if (imagenPantalla != null && imagenesHistoria[indice] != null) // funcion: Si la imagen de pantalla y la imagen a mostrar existen.
        {
            imagenPantalla.sprite = imagenesHistoria[indice]; // funcion: Asigna el sprite a la imagen de pantalla.
            imagenPantalla.color = Color.white; // funcion: Asegura que la imagen sea visible.
           
        }

        if (sonidosPorImagen != null && sonidosPorImagen.Length > indice && sonidosPorImagen[indice] != null) // funcion: Si hay sonido para la imagen.
        {
            audioSource.PlayOneShot(sonidosPorImagen[indice]); // funcion: Reproduce el sonido correspondiente.
        }
    }

    IEnumerator TransicionImagen(int nuevoIndice) // funcion: Corrutina para la transición entre imágenes.
    {
        puedeAvanzar = false; // funcion: Bloquea el avance durante la transición.

        float tiempo = 0; // funcion: Inicializa el tiempo de transición.

        while (tiempo < tiempoFadeOut) // funcion: Realiza el fade out.
        {
            tiempo += Time.deltaTime; // funcion: Incrementa el tiempo.
            float alpha = Mathf.Lerp(1f, 0f, tiempo / tiempoFadeOut); // funcion: Calcula el alpha para el fade out.

            if (imagenPantalla != null) // funcion: Si la imagen de pantalla existe.
            {
                Color color = imagenPantalla.color; // funcion: Obtiene el color actual.
                imagenPantalla.color = new Color(color.r, color.g, color.b, alpha); // funcion: Aplica el alpha calculado.
            }

            yield return null; // funcion: Espera al siguiente frame.
        }

        MostrarImagen(nuevoIndice); // funcion: Muestra la nueva imagen.

        tiempo = 0; // funcion: Reinicia el tiempo para el fade in.
        while (tiempo < tiempoFadeIn) // funcion: Realiza el fade in.
        {
            tiempo += Time.deltaTime; // funcion: Incrementa el tiempo.
            float alpha = Mathf.Lerp(0f, 1f, tiempo / tiempoFadeIn); // funcion: Calcula el alpha para el fade in.

            if (imagenPantalla != null) // funcion: Si la imagen de pantalla existe.
            {
                imagenPantalla.color = new Color(1f, 1f, 1f, alpha); // funcion: Aplica el alpha calculado.
            }

            yield return null; // funcion: Espera al siguiente frame.
        }

        puedeAvanzar = true; // funcion: Permite avanzar nuevamente.
    }

    void TerminarCinematica() // funcion: Termina la cinemática y carga la siguiente escena.
    {
        cinematicaTerminada = true; // funcion: Marca la cinemática como terminada.
        // funcion: (Eliminado Debug.Log de cinemática terminada)
        StartCoroutine(IrASiguienteEscena()); // funcion: Inicia la corrutina para cambiar de escena.
    }

    IEnumerator IrASiguienteEscena() // funcion: Corrutina para el fade out final y cambio de escena.
    {
        if (imagenPantalla != null) // funcion: Si la imagen de pantalla existe.
        {
            float tiempo = 0; // funcion: Inicializa el tiempo.
            while (tiempo < 1f) // funcion: Realiza el fade out.
            {
                tiempo += Time.deltaTime; // funcion: Incrementa el tiempo.
                Color color = imagenPantalla.color; // funcion: Obtiene el color actual.
                imagenPantalla.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1f, 0f, tiempo)); // funcion: Aplica el alpha para el fade out.
                yield return null; // funcion: Espera al siguiente frame.
            }
        }

        yield return new WaitForSeconds(0.5f); // funcion: Espera medio segundo antes de cambiar de escena.

        SceneManager.LoadScene(escenaSiguiente); // funcion: Carga la siguiente escena.
    }

    void SaltarCinematica() // funcion: Salta la cinemática y termina inmediatamente.
    {
        
        TerminarCinematica(); // funcion: Llama a la función para terminar la cinemática.
    }
}
