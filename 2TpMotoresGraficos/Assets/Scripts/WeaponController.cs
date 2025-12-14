
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour 
{
    public float FireRange = 200f; // funcion: Distancia máxima del disparo.
    public LayerMask hittableLayers; // funcion: Capas que pueden ser impactadas por el disparo.
    public Transform cameraPlayerTransform; // funcion: Referencia al transform de la cámara del jugador.
    public GameObject bulletHolePrefab; // funcion: Prefab para el agujero de bala.
    public float recoilForce = 5f; // funcion: Fuerza de retroceso al disparar.
    public Transform weaponMuzzle; // funcion: Punto de salida del disparo.
    public GameObject flashEffect; // funcion: Prefab del efecto de fogonazo al disparar.
    public float fireRate = 0.6f; // funcion: Tiempo entre disparos.
    public int maxAmmo = 8; // funcion: Máxima cantidad de munición.
    private float lastTimeShoot = Mathf.NegativeInfinity; // funcion: Momento del último disparo.
    public float reloadTime = 1.5f; // funcion: Tiempo de recarga.
    public int currentAmmo { get; private set; } // funcion: Munición actual (solo lectura pública).

    [Header("Audio")]
    public AudioClip sonidoDisparo; // funcion: Sonido al disparar.
    public AudioClip sonidoRecarga; // funcion: Sonido al recargar.
    private AudioSource audioSource; // funcion: Referencia al componente AudioSource.

    private void Awake() // funcion: Inicializa variables y eventos al instanciar el objeto.
    {
        currentAmmo = maxAmmo; // funcion: Asigna la munición máxima al iniciar.
        EventManager.current.updateBulletsEvent.Invoke(currentAmmo, maxAmmo); // funcion: Actualiza la UI de balas.

        audioSource = gameObject.AddComponent<AudioSource>(); // funcion: Añade un componente AudioSource.
        audioSource.playOnAwake = false; // funcion: Desactiva la reproducción automática del audio.
        audioSource.volume = 0.5f; // funcion: Ajusta el volumen del audio.
    }

    private void Start() // funcion: Inicializa referencias al iniciar la escena.
    {
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform; // funcion: Obtiene la referencia a la cámara principal.
    }

    private void Update() // funcion: Se ejecuta cada frame.
    {
        if (Input.GetMouseButtonDown(0)) // funcion: Si se presiona el botón izquierdo del ratón.
        {
            TryShoot(); // funcion: Intenta disparar.
        }
        if (Input.GetKeyDown(KeyCode.R)) // funcion: Si se presiona la tecla R.
        {
            StartCoroutine(Reload()); // funcion: Inicia la recarga.
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f); // funcion: Suaviza la posición del arma para el retroceso.
    }

    private bool TryShoot() // funcion: Intenta realizar un disparo.
    {
        if (lastTimeShoot + fireRate < Time.time) // funcion: Verifica si ha pasado suficiente tiempo desde el último disparo.
        {
            if (currentAmmo >= 1) // funcion: Verifica si hay munición disponible.
            {
                HandleShoot(); // funcion: Ejecuta la lógica de disparo.
                currentAmmo -= 1; // funcion: Resta una bala.
                EventManager.current.updateBulletsEvent.Invoke(currentAmmo, maxAmmo); // funcion: Actualiza la UI de balas.
                return true; // funcion: Indica que se disparó.
            }
        }
        return false; // funcion: No se pudo disparar.
    }

    private void HandleShoot() // funcion: Lógica completa de un disparo.
    {
        if (sonidoDisparo != null && audioSource != null) // funcion: Si hay sonido de disparo.
        {
            audioSource.PlayOneShot(sonidoDisparo); // funcion: Reproduce el sonido de disparo.
        }

        GameObject flashClone = Instantiate(flashEffect, weaponMuzzle.position, Quaternion.Euler(weaponMuzzle.forward), transform); // funcion: Instancia el efecto de fogonazo.
        Destroy(flashClone, 0.5f); // funcion: Destruye el efecto de fogonazo tras 0.5 segundos.
        AddRecoil(); // funcion: Aplica el retroceso al arma.

        RaycastHit[] hits = Physics.RaycastAll(cameraPlayerTransform.position, cameraPlayerTransform.forward, FireRange); // funcion: Realiza un raycast múltiple en la dirección de la cámara.

        

        EnemyLife closestEnemy = null; // funcion: Referencia al enemigo más cercano impactado.
        RaycastHit closestEnemyHit = new RaycastHit(); // funcion: Información del impacto más cercano.
        float closestDistance = Mathf.Infinity; // funcion: Distancia más cercana inicializada en infinito.

        foreach (RaycastHit hit in hits) // funcion: Itera sobre todos los impactos detectados.
        {
            

            EnemyLife enemy = hit.collider.GetComponent<EnemyLife>(); // funcion: Intenta obtener el componente EnemyLife del objeto impactado.

            if (enemy != null && hit.distance < closestDistance) // funcion: Si es un enemigo y está más cerca que el anterior.
            {
                closestEnemy = enemy; // funcion: Actualiza el enemigo más cercano.
                closestEnemyHit = hit; // funcion: Actualiza la información del impacto.
                closestDistance = hit.distance; // funcion: Actualiza la distancia más cercana.
                
            }
        }

        if (closestEnemy != null) // funcion: Si se impactó un enemigo.
        {
            

            GameObject bulletHoleClone = Instantiate(bulletHolePrefab, closestEnemyHit.point + closestEnemyHit.normal * 0.001f, Quaternion.LookRotation(closestEnemyHit.normal)); // funcion: Instancia el agujero de bala en el enemigo.
            Destroy(bulletHoleClone, 5f); // funcion: Destruye el agujero de bala tras 5 segundos.

            closestEnemy.TakeDamage(10); // funcion: Aplica daño al enemigo.

            
        }
        else // funcion: Si no se impactó ningún enemigo.
        {
            

            RaycastHit surfaceHit; // funcion: Información del impacto en superficie.
            if (Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out surfaceHit, FireRange, hittableLayers)) // funcion: Realiza un raycast para superficies.
            {
                GameObject bulletHoleClone = Instantiate(bulletHolePrefab, surfaceHit.point + surfaceHit.normal * 0.001f, Quaternion.LookRotation(surfaceHit.normal)); // funcion: Instancia el agujero de bala en la superficie.
                Destroy(bulletHoleClone, 5f); // funcion: Destruye el agujero de bala tras 5 segundos.
            }
        }

        lastTimeShoot = Time.time; // funcion: Actualiza el tiempo del último disparo.
    }

    private void AddRecoil() // funcion: Aplica retroceso visual al arma.
    {
        transform.Rotate(-recoilForce, 0f, 0f); // funcion: Rota el arma hacia atrás.
        transform.position = transform.position - transform.forward * (recoilForce / 50f); // funcion: Mueve el arma hacia atrás.
    }

    IEnumerator Reload() // funcion: Corrutina para recargar el arma.
    {
        

        if (sonidoRecarga != null && audioSource != null) // funcion: Si hay sonido de recarga.
        {
            audioSource.PlayOneShot(sonidoRecarga); // funcion: Reproduce el sonido de recarga.
        }

        yield return new WaitForSeconds(reloadTime); // funcion: Espera el tiempo de recarga.
        currentAmmo = maxAmmo; // funcion: Rellena la munición.
        EventManager.current.updateBulletsEvent.Invoke(currentAmmo, maxAmmo); // funcion: Actualiza la UI de balas.
        
    }
}
