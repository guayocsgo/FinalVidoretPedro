using UnityEngine;
using TMPro;

public class WeaponInfo : MonoBehaviour 
{
    public TMP_Text currentBullets; // función: Referencia pública al texto que muestra las balas actuales.
    public TMP_Text totalBullets; // función: Referencia pública al texto que muestra el total de balas.

    private void OnEnable() // función: Método llamado automáticamente cuando el objeto se activa.
    {
        EventManager.current.updateBulletsEvent.AddListener(UpdateBullets); // función: Suscribe el método UpdateBullets al evento updateBulletsEvent.
    }

    private void OnDisable() // función: Método llamado automáticamente cuando el objeto se desactiva.
    {
        
    }

    public void UpdateBullets(int newCurrentBullets, int newTotalBullets) // función: Actualiza la UI con la cantidad de balas actuales y totales.
    {
        if (newCurrentBullets <= 0) // función: Verifica si las balas actuales son menores o iguales a cero.
        {
            currentBullets.color = new Color(1, 0, 0); // función: Cambia el color del texto de balas actuales a rojo.
        }
        else // función: Si hay balas disponibles.
        {
            currentBullets.color = Color.white; // función: Cambia el color del texto de balas actuales a blanco.
        }
        currentBullets.text = newCurrentBullets.ToString(); // función: Actualiza el texto de balas actuales con el nuevo valor.
        totalBullets.text = newTotalBullets.ToString(); // función: Actualiza el texto de balas totales con el nuevo valor.
    }
}
