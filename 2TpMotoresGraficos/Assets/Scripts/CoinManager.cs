using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour 
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI coinText; // funcion: Referencia al texto UI donde se muestran las monedas.

    [Header("Configuración")]
    [SerializeField] private string coinPlayerPrefsKey = "TotalCoins"; // funcion: Clave para guardar las monedas totales en PlayerPrefs.

    private int currentCoins = 0; // funcion: Monedas recolectadas en la sesión actual.
    private int totalCoinsCollected = 0; // funcion: Monedas totales acumuladas (persistentes).

    private void Start() // funcion: Se ejecuta al iniciar el script.
    {
        totalCoinsCollected = PlayerPrefs.GetInt(coinPlayerPrefsKey, 0); // funcion: Obtiene las monedas totales guardadas de PlayerPrefs.

        UpdateCoinUI(); // funcion: Actualiza el texto de monedas en la UI.

        
    }

    public void AddCoins(int amount) // funcion: Añade monedas al contador.
    {
        currentCoins += amount; // funcion: Suma la cantidad a las monedas de la sesión.
        totalCoinsCollected += amount; // funcion: Suma la cantidad a las monedas totales.

        PlayerPrefs.SetInt(coinPlayerPrefsKey, totalCoinsCollected); // funcion: Guarda el total de monedas en PlayerPrefs.
        PlayerPrefs.Save(); // funcion: Fuerza el guardado de PlayerPrefs.

        UpdateCoinUI(); // funcion: Actualiza el texto de monedas en la UI.

        
    }

    private void UpdateCoinUI() // funcion: Actualiza el texto de monedas en la UI.
    {
        if (coinText != null) // funcion: Verifica que el texto de monedas esté asignado.
        {
            coinText.text = "Monedas: " + currentCoins.ToString(); // funcion: Muestra la cantidad de monedas actuales en la UI.
        }
    }

    public int GetCurrentCoins() // funcion: Devuelve las monedas de la sesión actual.
    {
        return currentCoins; // funcion: Retorna el valor de monedas actuales.
    }

    public int GetTotalCoins() // funcion: Devuelve el total de monedas acumuladas.
    {
        return totalCoinsCollected; // funcion: Retorna el valor de monedas totales.
    }

    public void ResetTotalCoins() // funcion: Resetea el total de monedas acumuladas.
    {
        totalCoinsCollected = 0; // funcion: Pone el total de monedas a cero.
        PlayerPrefs.SetInt(coinPlayerPrefsKey, 0); // funcion: Guarda el valor cero en PlayerPrefs.
        PlayerPrefs.Save(); // funcion: Fuerza el guardado de PlayerPrefs.
        
    }
}
