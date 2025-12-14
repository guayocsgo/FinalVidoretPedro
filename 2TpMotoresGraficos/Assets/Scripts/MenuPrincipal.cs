using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelMenuPrincipal; // funcion: referencia al panel del menú principal
    public GameObject panelOpciones;      // funcion: referencia al panel de opciones

    private void Start()
    {
        // funcion: muestra el panel del menú principal al iniciar
        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(true);
        }

        // funcion: oculta el panel de opciones al iniciar
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }
    }

    public void Jugar()
    {
        // funcion: carga la escena "Nivel1" para iniciar el juego
        SceneManager.LoadScene("Nivel1");
    }

    public void AbrirOpciones()
    {
        // funcion: oculta el menú principal
        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(false);
        }

        // funcion: muestra el panel de opciones
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(true);
        }
    }

    public void CerrarOpciones()
    {
        // funcion: oculta el panel de opciones
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
        }

        // funcion: muestra el menú principal
        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(true);
        }
    }

    public void Salir()
    {
        // funcion: sale del juego (o detiene la ejecución en el editor)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // funcion: detiene el juego en el editor
#else
        Application.Quit(); // funcion: cierra la aplicación en build
#endif
    }
}
