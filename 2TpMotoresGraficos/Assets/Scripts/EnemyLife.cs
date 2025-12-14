
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyLife : MonoBehaviour // funcion: Define la clase EnemyLife que gestiona la vida del enemigo.
{
    public int vidaEnemigo = 100; // funcion: Vida actual del enemigo.
    public Slider BarraVidaEnemigo; // funcion: Referencia al slider de la barra de vida del enemigo.
    public GameObject bloodEffectPrefab; // funcion: Prefab del efecto de sangre al recibir daño o morir.

    public EnemySpawnManager spawnManager; // funcion: Referencia al gestor de spawn de enemigos.
    public bool canSpawnEnemies = true; // funcion: Indica si este enemigo puede generar nuevos enemigos al morir.

    private bool isDead = false; // funcion: Indica si el enemigo está muerto.
    private Animator animator; // funcion: Referencia al componente Animator.
    private int vidaMaxima; // funcion: Vida máxima del enemigo.
    private EnemyAi enemyAi; // funcion: Referencia al script de IA del enemigo.

    private void Awake() // funcion: Inicializa referencias y valores al instanciar el objeto.
    {
        animator = GetComponent<Animator>(); // funcion: Obtiene el componente Animator del enemigo.
        enemyAi = GetComponent<EnemyAi>(); // funcion: Obtiene el componente EnemyAi del enemigo.
        vidaMaxima = vidaEnemigo; // funcion: Asigna la vida máxima igual a la vida inicial.

        ConfigurarSlider(); // funcion: Configura el slider de la barra de vida.
    }

    void ConfigurarSlider() // funcion: Configura los valores del slider de vida.
    {
        if (BarraVidaEnemigo != null) // funcion: Verifica si el slider está asignado.
        {
            BarraVidaEnemigo.maxValue = vidaMaxima; // funcion: Asigna el valor máximo del slider.
            BarraVidaEnemigo.value = vidaEnemigo; // funcion: Asigna el valor actual del slider.
            
        }
        else
        {
            
        }
    }

    private void Update() // funcion: Se ejecuta cada frame.
    {
        if (BarraVidaEnemigo != null && !isDead) // funcion: Si el slider existe y el enemigo no está muerto.
        {
            BarraVidaEnemigo.value = vidaEnemigo; // funcion: Actualiza el valor del slider con la vida actual.
        }

        if (vidaEnemigo <= 0 && !isDead) // funcion: Si la vida es 0 o menos y no está muerto.
        {
            Die(); // funcion: Ejecuta la lógica de muerte.
        }
    }

    public void TakeDamage(int damage) // funcion: Aplica daño al enemigo.
    {
        if (isDead) return; // funcion: Si ya está muerto, no hace nada.

        vidaEnemigo -= damage; // funcion: Resta el daño recibido a la vida.
        vidaEnemigo = Mathf.Max(0, vidaEnemigo); // funcion: Asegura que la vida no sea menor que 0.

        

        if (enemyAi != null && vidaEnemigo > 0) // funcion: Si tiene IA y sigue vivo.
        {
            enemyAi.AlRecibirDanio(); // funcion: Llama a la función de reacción al daño en la IA.
        }

        if (BarraVidaEnemigo != null) // funcion: Si el slider existe.
        {
            BarraVidaEnemigo.value = vidaEnemigo; // funcion: Actualiza el valor del slider.
        }
        else
        {
            
            ConfigurarSlider(); // funcion: Intenta configurar el slider de nuevo.
        }

        if (bloodEffectPrefab != null) // funcion: Si hay prefab de sangre asignado.
        {
            Vector3 bloodPosition = transform.position + Vector3.up * 1f; // funcion: Calcula la posición del efecto de sangre.
            GameObject blood = Instantiate(bloodEffectPrefab, bloodPosition, Quaternion.identity); // funcion: Instancia el efecto de sangre.
            ParticleSystem ps = blood.GetComponent<ParticleSystem>(); // funcion: Obtiene el sistema de partículas del efecto.
            if (ps != null) // funcion: Si existe el sistema de partículas.
            {
                ps.Play(); // funcion: Reproduce el efecto de partículas.
            }
            Destroy(blood, 2f); // funcion: Destruye el efecto de sangre tras 2 segundos.
        }
    }

    void Die() // funcion: Lógica de muerte del enemigo.
    {
        isDead = true; // funcion: Marca al enemigo como muerto.
        

        if (spawnManager != null) // funcion: Si hay un gestor de spawn asignado.
        {
            spawnManager.OnEnemyDeath(transform.position, canSpawnEnemies); // funcion: Notifica al gestor de spawn la muerte del enemigo.
        }

        if (bloodEffectPrefab != null) // funcion: Si hay prefab de sangre asignado.
        {
            GameObject blood = Instantiate(bloodEffectPrefab, transform.position + Vector3.up * 1f, Quaternion.identity); // funcion: Instancia el efecto de sangre.
            ParticleSystem ps = blood.GetComponent<ParticleSystem>(); // funcion: Obtiene el sistema de partículas.
            if (ps != null) // funcion: Si existe el sistema de partículas.
            {
                var emission = ps.emission; // funcion: Accede a la emisión de partículas.
                emission.SetBurst(0, new ParticleSystem.Burst(0, 30)); // funcion: Configura un burst de partículas.
            }
            Destroy(blood, 2f); // funcion: Destruye el efecto de sangre tras 2 segundos.
        }

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); // funcion: Obtiene el componente NavMeshAgent.
        if (agent != null) // funcion: Si existe el agente de navegación.
        {
            agent.isStopped = true; // funcion: Detiene el agente.
            agent.enabled = false; // funcion: Desactiva el agente.
        }

        Collider col = GetComponent<Collider>(); // funcion: Obtiene el collider del enemigo.
        if (col != null) // funcion: Si existe el collider.
        {
            col.enabled = false; // funcion: Desactiva el collider.
        }

        if (BarraVidaEnemigo != null) // funcion: Si el slider existe.
        {
            BarraVidaEnemigo.gameObject.SetActive(false); // funcion: Oculta la barra de vida.
        }

        if (animator != null) // funcion: Si existe el animator.
        {
            animator.SetBool("isWalking", false); // funcion: Detiene la animación de caminar.
        }

        StartCoroutine(DeathAnimation()); // funcion: Inicia la corrutina de animación de muerte.
    }

    IEnumerator DeathAnimation() // funcion: Corrutina para animar la muerte del enemigo.
    {
        float duration = 1.5f; // funcion: Duración de la animación.
        float elapsed = 0f; // funcion: Tiempo transcurrido.
        Vector3 originalScale = transform.localScale; // funcion: Escala original del enemigo.
        Vector3 originalPosition = transform.position; // funcion: Posición original del enemigo.
        Quaternion originalRotation = transform.rotation; // funcion: Rotación original del enemigo.

        while (elapsed < duration) // funcion: Mientras no termine la animación.
        {
            elapsed += Time.deltaTime; // funcion: Incrementa el tiempo transcurrido.
            float t = elapsed / duration; // funcion: Calcula el porcentaje de la animación.

            transform.position = Vector3.Lerp(originalPosition, originalPosition + Vector3.down * 0.5f, t); // funcion: Interpola la posición hacia abajo.
            transform.rotation = Quaternion.Lerp(originalRotation, originalRotation * Quaternion.Euler(90, 0, 0), t); // funcion: Interpola la rotación para simular caída.
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 0.5f, t); // funcion: Reduce la escala del enemigo.

            yield return null; // funcion: Espera al siguiente frame.
        }

        Destroy(gameObject, 1f); // funcion: Destruye el objeto enemigo tras 1 segundo.
    }
}
