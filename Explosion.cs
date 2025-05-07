using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius = 5f;
    public float force = 700f;
    public int damage = 30;
    public LayerMask affectedLayers;

    public AudioClip explosionSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;
    }


    void Start()
    {
        

        Explode();
    }

    public void Explode()
    {

        if (explosionSound != null)
        {
            GameObject soundObject = new GameObject("ExplosionSound");
            AudioSource newSource = soundObject.AddComponent<AudioSource>();
            newSource.clip = explosionSound;
            newSource.spatialBlend = 1f;
            newSource.transform.position = transform.position;
            newSource.Play();

            Destroy(soundObject, explosionSound.length);
        }

        Destroy(gameObject);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, affectedLayers);

        foreach (Collider nearbyObject in colliders)
        {

            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            PlayerStats player = nearbyObject.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
                player.ApplyExplosionForce(transform.position, force, radius);
            }

        }

        Destroy(transform.parent != null ? transform.parent.gameObject : gameObject);

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
