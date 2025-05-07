using UnityEngine;

public class HunterSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnDelay = 15f;
    public float detectionRadius = 5f;
    public LayerMask playerLayer;

    [Header("Vent Animation & Sound")]
    public Animator ventAnimator;
    public string animationTriggerName = "Activate";
    public AudioClip ventSound;

    private AudioSource audioSource;

    private bool playerTriggered = false;
    private bool enemySpawned = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (!playerTriggered)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
            if (hits.Length > 0)
            {
                playerTriggered = true;


                if (ventAnimator != null)
                {
                    ventAnimator.SetTrigger(animationTriggerName);
                }

                if (ventSound != null)
                {
                    audioSource.PlayOneShot(ventSound);
                }


                Invoke(nameof(SpawnEnemy), spawnDelay);
            }
        }
    }

    void SpawnEnemy()
    {
        if (!enemySpawned)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemySpawned = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
