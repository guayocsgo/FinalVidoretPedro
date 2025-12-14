
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour 
{
    [Header("Vida")]
    public int vidaMaxima = 100; // funcion: Vida máxima del jugador.
    public int vidaActual; // funcion: Vida actual del jugador.

    [Header("UI")]
    public Slider barraVida; // funcion: Referencia al slider de la barra de vida.
    public GameObject gameOverPanel; // funcion: Referencia al panel de Game Over.

    [Header("Audio")]
    public AudioClip sonidoMuerte; // funcion: Sonido al morir.
    public AudioClip sonidoDanio; // funcion: Sonido al recibir daño.
    private AudioSource audioSource; // funcion: Referencia al componente AudioSource.

    private bool isDead = false; // funcion: Indica si el jugador está muerto.

    void Start() // funcion: Inicializa valores y referencias al iniciar el script.
    {
        vidaActual = vidaMaxima; // funcion: Asigna la vida actual igual a la máxima.

        audioSource = gameObject.AddComponent<AudioSource>(); // funcion: Añade un componente AudioSource al objeto.
        audioSource.playOnAwake = false; // funcion: Desactiva la reproducción automática del audio.
        audioSource.volume = 0.7f; // funcion: Ajusta el volumen del audio.

        if (barraVida != null) // funcion: Si la barra de vida está asignada.
        {
            barraVida.maxValue = vidaMaxima; // funcion: Asigna el valor máximo del slider.
            barraVida.value = vidaActual; // funcion: Asigna el valor actual del slider.
        }

        if (gameOverPanel != null) // funcion: Si el panel de Game Over está asignado.
        {
            gameOverPanel.SetActive(false); // funcion: Oculta el panel de Game Over al inicio.
        }
    }

    void Update() // funcion: Se ejecuta cada frame.
    {
        if (barraVida != null) // funcion: Si la barra de vida está asignada.
        {
            barraVida.value = vidaActual; // funcion: Actualiza el valor del slider con la vida actual.
        }
    }

    public void RecibirDanio(int cantidad) // funcion: Aplica daño al jugador.
    {
        if (isDead) return; // funcion: Si el jugador ya está muerto, no hace nada.

        vidaActual -= cantidad; // funcion: Resta la cantidad de daño a la vida actual.
        vidaActual = Mathf.Max(0, vidaActual); // funcion: Asegura que la vida no sea menor que 0.

        

        if (sonidoDanio != null && audioSource != null && vidaActual > 0) // funcion: Si hay sonido de daño y el jugador sigue vivo.
        {
            audioSource.PlayOneShot(sonidoDanio); // funcion: Reproduce el sonido de daño.
        }

        if (vidaActual <= 0) // funcion: Si la vida llega a 0 o menos.
        {
            Die(); // funcion: Llama al método de muerte.
        }
    }

    public void Die() // funcion: Lógica de muerte del jugador.
    {
        if (isDead) return; // funcion: Si ya está muerto, no hace nada.
        isDead = true; // funcion: Marca al jugador como muerto.

        

        if (sonidoMuerte != null && audioSource != null) // funcion: Si hay sonido de muerte.
        {
            audioSource.PlayOneShot(sonidoMuerte); // funcion: Reproduce el sonido de muerte.
        }

        WeaponController weapon = GetComponentInChildren<WeaponController>(); // funcion: Obtiene el controlador de armas del jugador.
        if (weapon != null) // funcion: Si existe el controlador de armas.
        {
            weapon.enabled = false; // funcion: Desactiva el controlador de armas.
        }

        PlayerController fpsController = GetComponent<PlayerController>(); // funcion: Obtiene el controlador de movimiento del jugador.
        if (fpsController != null) // funcion: Si existe el controlador de movimiento.
        {
            fpsController.enabled = false; // funcion: Desactiva el controlador de movimiento.
        }

        if (gameOverPanel != null) // funcion: Si el panel de Game Over está asignado.
        {
            gameOverPanel.SetActive(true); // funcion: Muestra el panel de Game Over.
            Time.timeScale = 0f; // funcion: Pausa el juego.
            Cursor.lockState = CursorLockMode.None; // funcion: Libera el cursor.
            Cursor.visible = true; // funcion: Hace visible el cursor.
        }
        else
        {
            Invoke("RestartLevel", 2f); // funcion: Reinicia el nivel después de 2 segundos si no hay panel de Game Over.
        }
    }

    public void RestartLevel() // funcion: Reinicia el nivel actual.
    {
        Time.timeScale = 1f; // funcion: Restaura la escala de tiempo a la normalidad.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // funcion: Recarga la escena actual.
    }

    void OnCollisionEnter(Collision collision) // funcion: Se ejecuta al colisionar con otro objeto.
    {
        if (collision.gameObject.CompareTag("Enemy")) // funcion: Si colisiona con un objeto con tag "Enemy".
        {
            

            vidaActual = 0; // funcion: Establece la vida a 0.
            Die(); // funcion: Llama al método de muerte.
        }
    }
}
