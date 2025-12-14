
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Wave
{
    public string waveName = "Oleada 1"; // funcion: nombre de la oleada
    public GameObject enemyPrefab;       // funcion: prefab del enemigo a spawnear
    public int enemyCount = 1;           // funcion: cantidad de enemigos en la oleada
    public float delayBeforeWave = 1f;   // funcion: tiempo de espera antes de la oleada
}

public class EnemySpawnManager : MonoBehaviour
{
    [Header("Configuración del Jugador")]
    public Transform player; // funcion: referencia al jugador

    [Header("Configuración de Spawn")]
    public float spawnRadius = 10f;           // funcion: radio máximo de spawn
    public float minDistanceFromPlayer = 5f;  // funcion: distancia mínima al jugador para spawnear
    public float spawnHeight = 0.5f;          // funcion: altura de spawn

    [Header("Oleadas")]
    public Wave[] waves; // funcion: array de oleadas

    [Header("UI")]
    public GameObject victoryPanel; // funcion: panel de victoria
    public Text waveText;           // funcion: texto de la oleada actual

    [Header("Configuración de Monedas")]
    public string coinPlayerPrefsKey = "TotalCoins"; // funcion: clave para PlayerPrefs de monedas

    private Text totalCoinsText; // funcion: texto de monedas totales
    private Text levelCoinsText; // funcion: texto de monedas del nivel

    private int totalEnemiesAlive = 0; // funcion: enemigos vivos actualmente
    private int currentWave = 0;       // funcion: índice de la oleada actual

    private void Start()
    {
        // funcion: oculta el panel de victoria al inicio
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);

            // funcion: busca el texto de monedas totales en el panel de victoria
            Transform totalCoinsTransform = victoryPanel.transform.Find("TotalCoinsText");
            if (totalCoinsTransform != null)
            {
                totalCoinsText = totalCoinsTransform.GetComponent<Text>();
            }

            // funcion: busca el texto de monedas del nivel en el panel de victoria
            Transform levelCoinsTransform = victoryPanel.transform.Find("LevelCoinsText");
            if (levelCoinsTransform != null)
            {
                levelCoinsText = levelCoinsTransform.GetComponent<Text>();
            }
        }

        // funcion: verifica que haya oleadas configuradas
        if (waves == null || waves.Length == 0)
        {
            return;
        }

        // funcion: verifica que cada oleada tenga un prefab asignado
        for (int i = 0; i < waves.Length; i++)
        {
            if (waves[i].enemyPrefab == null)
            {
                // funcion: si falta un prefab, continúa (no detiene el juego)
            }
        }

        // funcion: inicia la primera oleada tras el delay configurado
        Invoke("StartFirstWave", waves[0].delayBeforeWave);
    }

    void StartFirstWave()
    {
        // funcion: spawnea la primera oleada
        SpawnWave(0);
    }

    public void OnEnemyDeath(Vector3 deathPosition, bool canSpawn)
    {
        // funcion: reduce el contador de enemigos vivos al morir uno
        totalEnemiesAlive--;

        // funcion: si no quedan enemigos vivos, revisa si hay más oleadas
        if (totalEnemiesAlive <= 0)
        {
            CheckNextWave();
        }
    }

    void CheckNextWave()
    {
        // funcion: avanza a la siguiente oleada
        currentWave++;

        // funcion: si hay más oleadas, las programa tras el delay
        if (currentWave < waves.Length)
        {
            float delay = waves[currentWave].delayBeforeWave;
            Invoke("SpawnNextWave", delay);
        }
        else
        {
            // funcion: si no hay más oleadas, muestra el panel de victoria tras 1 segundo
            Invoke("ShowVictory", 1f);
        }
    }

    void SpawnNextWave()
    {
        // funcion: spawnea la siguiente oleada
        SpawnWave(currentWave);
    }

    void SpawnWave(int waveIndex)
    {
        // funcion: si el índice de oleada es inválido, sale
        if (waveIndex >= waves.Length)
        {
            return;
        }

        Wave currentWaveData = waves[waveIndex]; // funcion: obtiene los datos de la oleada actual

        // funcion: si no hay prefab, sale
        if (currentWaveData.enemyPrefab == null)
        {
            return;
        }

        // funcion: si no hay jugador asignado, sale
        if (player == null)
        {
            return;
        }

        int enemiesToSpawn = currentWaveData.enemyCount; // funcion: cantidad de enemigos a spawnear

        // funcion: actualiza el texto de la oleada en la UI
        if (waveText != null)
        {
            waveText.text = currentWaveData.waveName + " (" + (waveIndex + 1) + "/" + waves.Length + ")";
        }

        // funcion: instancia los enemigos de la oleada
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy(currentWaveData.enemyPrefab);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        // funcion: obtiene una posición aleatoria válida para el spawn
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // funcion: ajusta la posición al NavMesh si es posible
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(spawnPosition, out hit, 10f, UnityEngine.AI.NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
        }
        else
        {
            return;
        }

        // funcion: instancia el enemigo en la posición calculada
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // funcion: si la instancia falla, sale
        if (newEnemy == null)
        {
            return;
        }

        // funcion: asigna la etiqueta "Enemy" al nuevo enemigo
        newEnemy.tag = "Enemy";

        // funcion: incrementa el contador de enemigos vivos
        totalEnemiesAlive++;

        // funcion: configura el componente EnemyLife si existe
        EnemyLife enemyLife = newEnemy.GetComponent<EnemyLife>();
        if (enemyLife != null)
        {
            enemyLife.spawnManager = this;         // funcion: referencia al spawn manager
            enemyLife.canSpawnEnemies = false;     // funcion: desactiva spawn adicional

            // funcion: verifica si el slider de vida está asignado
            if (enemyLife.BarraVidaEnemigo == null)
            {
                // funcion: si no hay slider, continúa
            }
        }

        // funcion: configura el componente EnemyAi si existe
        EnemyAi enemyAi = newEnemy.GetComponent<EnemyAi>();
        if (enemyAi != null)
        {
            enemyAi.player = player; // funcion: asigna el jugador como objetivo
        }
    }

    void ShowVictory()
    {
        // funcion: muestra el panel de victoria y detiene el tiempo
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            DisplayCoinInfo(); // funcion: muestra la información de monedas
        }

        Time.timeScale = 0f; // funcion: pausa el juego
    }

    void DisplayCoinInfo()
    {
        // funcion: busca el CoinManager en la escena
        CoinManager coinManager = FindObjectOfType<CoinManager>();

        // funcion: muestra las monedas del nivel si el texto y el manager existen
        if (levelCoinsText != null && coinManager != null)
        {
            levelCoinsText.text = "Monedas en este nivel: " + coinManager.GetCurrentCoins();
        }

        // funcion: obtiene el total de monedas guardadas
        int totalCoins = PlayerPrefs.GetInt(coinPlayerPrefsKey, 0);

        // funcion: muestra el total de monedas si el texto existe
        if (totalCoinsText != null)
        {
            totalCoinsText.text = "Total de Monedas: " + totalCoins;
        }
    }

    public void RestartGame()
    {
        // funcion: reinicia el juego y la escena actual
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    Vector3 GetRandomSpawnPosition()
    {
        // funcion: calcula una posición aleatoria válida para el spawn
        Vector3 spawnPos = Vector3.zero;
        int attempts = 0;
        int maxAttempts = 30;

        while (attempts < maxAttempts)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minDistanceFromPlayer, spawnRadius);
            spawnPos = player.position + new Vector3(randomCircle.x, spawnHeight, randomCircle.y);

            float distanceToPlayer = Vector3.Distance(new Vector3(spawnPos.x, player.position.y, spawnPos.z), player.position);

            if (distanceToPlayer >= minDistanceFromPlayer)
            {
                return spawnPos;
            }

            attempts++;
        }

        // funcion: si no encuentra una posición válida, genera una por defecto
        return player.position + player.forward * minDistanceFromPlayer + player.right * Random.Range(-3f, 3f);
    }
}
