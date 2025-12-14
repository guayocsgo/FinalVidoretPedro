using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDamagePlayer : MonoBehaviour // funcion: Define la clase EnemyDamagePlayer que detecta colisiones con el jugador.
{
    private void OnTriggerEnter(Collider other) // funcion: Se ejecuta al entrar en contacto con otro collider.
    {
        if (other.CompareTag("Player")) // funcion: Verifica si el objeto que colisiona tiene el tag "Player".
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // funcion: Reinicia la escena actual al detectar colisión con el jugador.
        }
    }
}
