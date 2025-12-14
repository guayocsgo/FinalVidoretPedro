using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour // funcion: Define la clase PlayerController que controla el movimiento y la cámara del jugador.
{
    public Camera playerCamera; // funcion: Referencia a la cámara del jugador.

    public float WalkSpeed = 5f; // funcion: Velocidad de caminar.
    public float runSpeed = 10f; // funcion: Velocidad de correr.
    public float jumpHeight = 2f; // funcion: Altura máxima del salto.
    public float gravityScale = -20f; // funcion: Escala de la gravedad aplicada al jugador.
    public float rotationSensitivity = 10f; // funcion: Sensibilidad de la rotación de la cámara.

    private float cameraVerticalAngle; // funcion: Ángulo vertical acumulado de la cámara.
    Vector3 moveInput = Vector3.zero; // funcion: Vector de entrada de movimiento.
    Vector3 rotationinput = Vector3.zero; // funcion: Vector de entrada de rotación.
    CharacterController characterController; // funcion: Referencia al componente CharacterController.

    private void Awake() // funcion: Se ejecuta al instanciar el objeto.
    {
        characterController = GetComponent<CharacterController>(); // funcion: Obtiene el componente CharacterController.
    }

    private void Update() // funcion: Se ejecuta cada frame.
    {
        Look(); // funcion: Llama al método para controlar la rotación de la cámara.
        Move(); // funcion: Llama al método para controlar el movimiento del jugador.
    }

    private void Move() // funcion: Controla el movimiento y salto del jugador.
    {
        if (characterController.isGrounded) // funcion: Verifica si el jugador está en el suelo.
        {
            moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); // funcion: Obtiene la entrada de movimiento en X y Z.
            moveInput = Vector3.ClampMagnitude(moveInput, 1f); // funcion: Limita la magnitud del vector a 1.

            if (Input.GetButton("Sprint")) // funcion: Si se mantiene presionado el botón de correr.
            {
                moveInput = transform.TransformDirection(moveInput) * runSpeed; // funcion: Aplica la velocidad de correr y transforma a espacio global.
            }
            else
            {
                moveInput = transform.TransformDirection(moveInput) * WalkSpeed; // funcion: Aplica la velocidad de caminar y transforma a espacio global.
            }

            if (Input.GetButtonDown("Jump")) // funcion: Si se presiona el botón de salto.
            {
                moveInput.y = Mathf.Sqrt(jumpHeight * -2f * gravityScale); // funcion: Calcula la velocidad vertical necesaria para alcanzar la altura de salto.
            }
        }
        moveInput.y += gravityScale * Time.deltaTime; // funcion: Aplica la gravedad al movimiento vertical.
        characterController.Move(moveInput * Time.deltaTime); // funcion: Mueve al jugador según el vector de movimiento.
    }

    private void Look() // funcion: Controla la rotación de la cámara y del jugador.
    {
        rotationinput.x = Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime; // funcion: Obtiene la entrada horizontal del ratón.
        rotationinput.y = Input.GetAxis("Mouse Y") * rotationSensitivity * Time.deltaTime; // funcion: Obtiene la entrada vertical del ratón.

        cameraVerticalAngle += rotationinput.y; // funcion: Acumula el ángulo vertical de la cámara.
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -70f, 70f); // funcion: Limita el ángulo vertical de la cámara.

        transform.Rotate(Vector3.up * rotationinput.x); // funcion: Rota el jugador horizontalmente.
        playerCamera.transform.localRotation = Quaternion.Euler(-cameraVerticalAngle, 0f, 0f); // funcion: Rota la cámara verticalmente.
    }
}
