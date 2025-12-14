using UnityEngine;

public class Sway : MonoBehaviour 
{
    private Quaternion originLocalRotation; // funcion: Almacena la rotación local original del objeto.

    private void Start() // funcion: Método llamado automáticamente al iniciar el script.
    {
        originLocalRotation = transform.localRotation; // funcion: Guarda la rotación local inicial del objeto.
    }

    private void Update() // funcion: Método llamado una vez por frame.
    {
        updateSway(); // funcion: Llama al método que actualiza el efecto de sway.
    }

    private void updateSway() // funcion: Aplica el efecto de sway basado en el movimiento del ratón.
    {
        float t_xLookInput = Input.GetAxis("Mouse X"); // funcion: Obtiene el movimiento horizontal del ratón.
        float t_yLookInput = Input.GetAxis("Mouse Y"); // funcion: Obtiene el movimiento vertical del ratón.

        Quaternion t_xAngleAdjustment = Quaternion.AngleAxis(-t_xLookInput * 1.45f, Vector3.up); // funcion: Calcula el ajuste de rotación en el eje Y (horizontal).
        Quaternion t_yAngleAdjustment = Quaternion.AngleAxis(t_yLookInput * 1.45f, Vector3.right); // funcion: Calcula el ajuste de rotación en el eje X (vertical).
        Quaternion t_targetRotation = originLocalRotation * t_xAngleAdjustment * t_yAngleAdjustment; // funcion: Calcula la rotación objetivo combinando la original y los ajustes.

        transform.localRotation = Quaternion.Slerp(transform.localRotation, t_targetRotation, Time.deltaTime * 10f); // funcion: Interpola suavemente la rotación actual hacia la rotación objetivo.
    }
}
