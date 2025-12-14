using UnityEngine;

public class BillboardHealthBar : MonoBehaviour 
{
    private Camera mainCamera; 

    void Start() 
    {
        mainCamera = Camera.main; // funcion: Asigna la cámara principal a la variable mainCamera.
    }

    void LateUpdate() 
    {
        if (mainCamera != null) // funcion: Verifica que la cámara principal esté asignada.
        {
            transform.LookAt(transform.position + mainCamera.transform.forward); // funcion: Hace que el objeto mire siempre hacia la cámara, manteniendo la barra de vida orientada al jugador.
        }
    }
}
