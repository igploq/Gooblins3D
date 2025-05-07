using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Hunter : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [Header("Footstep Sound")]
    public AudioClip footstepSound;
    public float stepDelay = 0.5f;

    private AudioSource audioSource;
    private bool isMoving = false;
    private bool isPlayingFootsteps = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

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
        if (player != null)
        {
            agent.SetDestination(player.position);
        }

        isMoving = agent.velocity.magnitude > 0.1f;

        if (isMoving && !isPlayingFootsteps)
        {
            StartCoroutine(PlayFootsteps());
        }
    }

    IEnumerator PlayFootsteps()
    {
        isPlayingFootsteps = true;

        if (footstepSound != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(footstepSound);
        }

        yield return new WaitForSeconds(stepDelay);

        isPlayingFootsteps = false;
    }
}
