using UnityEngine;

public class EnemyController : MonoBehaviour 
{
    public int health = 50; // funcion: Salud inicial del enemigo.

    public void TakeDamage(int amount) // funcion: Método para recibir daño.
    {
        health -= amount; // funcion: Resta la cantidad de daño a la salud del enemigo.
        

        if (health <= 0) // funcion: Si la salud es menor o igual a cero.
        {
            Die(); // funcion: Llama al método para eliminar al enemigo.
        }
    }

    void Die() // funcion: Método para eliminar al enemigo.
    {
        
        Destroy(gameObject); // funcion: Destruye el objeto del enemigo en la escena.
    }
}
