
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour // funcion: Define la clase EnemyAi que controla la IA del enemigo.
{
    public Transform player; // funcion: Referencia al transform del jugador.
    public float detectionRange = 20f; // funcion: Distancia a la que el enemigo detecta al jugador.
    public float stopDistance = 2f; // funcion: Distancia mínima para detenerse antes de atacar.

    [Header("Ataque")]
    public int danioAtaque = 10; // funcion: Daño que inflige el enemigo al atacar.
    public float tiempoEntreAtaques = 1f; // funcion: Tiempo entre ataques consecutivos.
    private float ultimoAtaque = 0f; // funcion: Marca el último momento en que atacó.

    private NavMeshAgent agent; // funcion: Componente de navegación.
    private EnemyLife enemyLife; // funcion: Referencia al script de vida del enemigo.
    private Animator animator; // funcion: Controlador de animaciones.
    private Rigidbody rb; // funcion: Referencia al Rigidbody del enemigo.
    private PlayerHealth playerHealth; // funcion: Referencia a la vida del jugador.

    void Start() // funcion: Inicializa componentes y parámetros al iniciar el script.
    {
        agent = GetComponent<NavMeshAgent>(); // funcion: Obtiene el componente NavMeshAgent.
        enemyLife = GetComponent<EnemyLife>(); // funcion: Obtiene el componente EnemyLife.
        animator = GetComponent<Animator>(); // funcion: Obtiene el componente Animator.
        rb = GetComponent<Rigidbody>(); // funcion: Obtiene el componente Rigidbody.

        if (player != null) // funcion: Si el jugador está asignado.
        {
            playerHealth = player.GetComponent<PlayerHealth>(); // funcion: Obtiene el componente PlayerHealth del jugador.
            
        }

        if (animator == null) // funcion: Si no se encuentra el Animator.
        {
            
        }

        if (rb != null) // funcion: Si el Rigidbody existe.
        {
            rb.isKinematic = true; // funcion: Desactiva la física del Rigidbody.
            rb.useGravity = false; // funcion: Desactiva la gravedad del Rigidbody.
        }

        NavMeshHit hit; // funcion: Variable para almacenar información de NavMesh.
        if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas)) // funcion: Busca la posición más cercana en el NavMesh.
        {
            transform.position = hit.position; // funcion: Ajusta la posición al NavMesh.
            agent.Warp(hit.position); // funcion: Mueve el agente a la posición ajustada.
        }
        else
        {
            
        }

        agent.acceleration = 12f; // funcion: Configura la aceleración del agente.
        agent.angularSpeed = 200f; // funcion: Configura la velocidad angular del agente.
        agent.stoppingDistance = stopDistance; // funcion: Configura la distancia de parada.
        agent.autoBraking = true; // funcion: Activa el frenado automático.
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance; // funcion: Configura la evasión de obstáculos.
        agent.avoidancePriority = 50; // funcion: Prioridad de evasión.
        agent.radius = 0.3f; // funcion: Radio del agente para navegación.
    }

    void Update() // funcion: Se ejecuta cada frame para controlar el comportamiento del enemigo.
    {
        if (agent == null || !agent.enabled || !agent.isOnNavMesh) // funcion: Si el agente no está listo, sale.
        {
            return;
        }

        if (enemyLife.vidaEnemigo <= 0) // funcion: Si el enemigo está muerto.
        {
            if (animator != null)
            {
                animator.SetBool("isWalking", false); // funcion: Detiene la animación de caminar.
            }
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position); // funcion: Calcula la distancia al jugador.

        if (distance < detectionRange) // funcion: Si el jugador está dentro del rango de detección.
        {
            if (distance > stopDistance) // funcion: Si el jugador está fuera del rango de ataque.
            {
                if (agent.isStopped)
                {
                    agent.isStopped = false; // funcion: Activa el movimiento del agente.
                }

                agent.SetDestination(player.position); // funcion: Establece el destino del agente al jugador.

                if (animator != null)
                {
                    animator.SetBool("isWalking", true); // funcion: Activa la animación de caminar.
                }

                RotateTowardsPlayer(); // funcion: Gira el enemigo hacia el jugador.

                if (agent.velocity.magnitude < 0.5f && agent.remainingDistance > stopDistance + 1f) // funcion: Si el agente está atascado.
                {
                    agent.ResetPath(); // funcion: Reinicia el camino del agente.

                    Vector3 direccionJugador = (player.position - transform.position).normalized; // funcion: Calcula la dirección hacia el jugador.
                    Vector3 puntoIntermedio = transform.position + direccionJugador * 2f; // funcion: Calcula un punto intermedio.

                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(puntoIntermedio, out hit, 3f, NavMesh.AllAreas)) // funcion: Busca un punto válido en el NavMesh.
                    {
                        agent.SetDestination(hit.position); // funcion: Establece el nuevo destino.
                    }
                }
            }
            else // funcion: Si el jugador está dentro del rango de ataque.
            {
                agent.isStopped = true; // funcion: Detiene el movimiento del agente.

                if (animator != null)
                {
                    animator.SetBool("isWalking", false); // funcion: Detiene la animación de caminar.
                }

                RotateTowardsPlayer(); // funcion: Gira el enemigo hacia el jugador.

                if (Time.time >= ultimoAtaque + tiempoEntreAtaques) // funcion: Si ha pasado suficiente tiempo desde el último ataque.
                {
                    AtacarJugador(); // funcion: Ataca al jugador.
                    ultimoAtaque = Time.time; // funcion: Actualiza el tiempo del último ataque.
                }
            }
        }
        else // funcion: Si el jugador está fuera del rango de detección.
        {
            agent.isStopped = true; // funcion: Detiene el movimiento del agente.

            if (animator != null)
            {
                animator.SetBool("isWalking", false); // funcion: Detiene la animación de caminar.
            }
        }
    }

    void AtacarJugador() // funcion: Lógica para atacar al jugador.
    {
        if (playerHealth != null) // funcion: Si la referencia a la vida del jugador existe.
        {
            playerHealth.RecibirDanio(danioAtaque); // funcion: Aplica daño al jugador.
            
        }
        else
        {
            
        }
    }

    void RotateTowardsPlayer() // funcion: Gira el enemigo para mirar al jugador.
    {
        if (player == null) return; // funcion: Si no hay jugador, sale.

        Vector3 direction = (player.position - transform.position).normalized; // funcion: Calcula la dirección hacia el jugador.

        if (direction.x > 0) // funcion: Si el jugador está a la derecha.
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // funcion: Ajusta la escala para mirar a la derecha.
        }
        else if (direction.x < 0) // funcion: Si el jugador está a la izquierda.
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // funcion: Ajusta la escala para mirar a la izquierda.
        }
    }

    void OnCollisionEnter(Collision collision) // funcion: Se ejecuta al colisionar con otro objeto.
    {
        if (agent != null && agent.isOnNavMesh && !agent.isStopped) // funcion: Si el agente está activo y en el NavMesh.
        {
            float distance = Vector3.Distance(transform.position, player.position); // funcion: Calcula la distancia al jugador.
            if (distance < detectionRange && distance > stopDistance) // funcion: Si el jugador está en rango de persecución.
            {
                agent.isStopped = false; // funcion: Activa el movimiento del agente.
            }
        }
    }

    public void AlRecibirDanio() // funcion: Lógica al recibir daño.
    {
        if (agent != null && agent.isOnNavMesh) // funcion: Si el agente está activo y en el NavMesh.
        {
            float distance = Vector3.Distance(transform.position, player.position); // funcion: Calcula la distancia al jugador.
            if (distance < detectionRange && distance > stopDistance) // funcion: Si el jugador está en rango de persecución.
            {
                agent.isStopped = false; // funcion: Activa el movimiento del agente.
                agent.SetDestination(player.position); // funcion: Establece el destino al jugador.
            }
        }
    }

    void OnDrawGizmosSelected() // funcion: Dibuja gizmos en el editor para visualizar los rangos.
    {
        Gizmos.color = Color.yellow; // funcion: Color para el rango de detección.
        Gizmos.DrawWireSphere(transform.position, detectionRange); // funcion: Dibuja el rango de detección.

        Gizmos.color = Color.red; // funcion: Color para el rango de parada.
        Gizmos.DrawWireSphere(transform.position, stopDistance); // funcion: Dibuja el rango de parada.
    }
}
