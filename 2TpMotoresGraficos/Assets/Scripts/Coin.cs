using UnityEngine;

public class Coin : MonoBehaviour 
{
    [Header("Configuración de Moneda")]
    [SerializeField] private int coinValue = 1; // funcion: Valor de la moneda al ser recolectada.
    [SerializeField] private float rotationSpeed = 100f; // funcion: Velocidad de rotación de la moneda.
    [SerializeField] private AudioClip collectSound; // funcion: Sonido que se reproduce al recolectar la moneda.

    [Header("Efectos Opcionales")]
    [SerializeField] private GameObject collectEffect; // funcion: Prefab del efecto visual al recolectar la moneda.

    private void Update() // funcion: Se ejecuta cada frame.
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); // funcion: Rota la moneda constantemente sobre el eje Y.
    }

    private void OnTriggerEnter(Collider other) // funcion: Se ejecuta al entrar en contacto con otro collider.
    {
        

        if (other.CompareTag("Player")) // funcion: Verifica si el objeto que colisiona tiene el tag "Player".
        {
            

            CoinManager coinManager = FindObjectOfType<CoinManager>(); // funcion: Busca el CoinManager en la escena.
            if (coinManager != null) // funcion: Si se encuentra el CoinManager.
            {
                coinManager.AddCoins(coinValue); // funcion: Añade el valor de la moneda al CoinManager.
                
            }
            else
            {
                
            }

            if (collectSound != null) // funcion: Si hay un sonido asignado.
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position); // funcion: Reproduce el sonido en la posición de la moneda.
            }

            if (collectEffect != null) // funcion: Si hay un efecto visual asignado.
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity); // funcion: Instancia el efecto visual en la posición de la moneda.
            }

            Destroy(gameObject); // funcion: Destruye la moneda tras ser recolectada.
        }
        else
        {
            
        }
    }
}
