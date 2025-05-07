using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRange = 10f;
    public float visionAngle = 60f;
    public LayerMask playerLayer;

    [Header("Chase Settings")]
    public float timeToForgetPlayer = 10f;

    private NavMeshAgent agent;
    private Transform targetPlayer;
    private bool chasingPlayer = false;
    private float chaseTimer = 0f;

    [Header("Patrol Settings")]
    public float patrolRadius = 10f;
    public float patrolWaitTime = 2f;

    private Vector3 patrolDestination;
    private bool hasPatrolDestination = false;
    private float waitTimer = 0f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        DetectPlayer();

        if (targetPlayer != null)
        {
            agent.SetDestination(targetPlayer.position);

            if (chasingPlayer)
                chaseTimer = 0f;
        }
        else if (chasingPlayer)
        {
            chaseTimer += Time.deltaTime;

            if (chaseTimer >= timeToForgetPlayer)
            {
                chasingPlayer = false;
                chaseTimer = 0f;
                agent.ResetPath();
                Debug.Log("Враг потерял игрока. Переход к патрулированию!");
            }
        }
        else
        {
            Patrol();
        }
    }


    void DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);

        bool foundPlayer = false;

        foreach (var hit in hits)
        {
            Vector3 directionToPlayer = (hit.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            if (angle < visionAngle / 2f)
            {
                Ray ray = new Ray(transform.position + Vector3.up, directionToPlayer);
                RaycastHit rayHit;

                if (Physics.Raycast(ray, out rayHit, detectionRange))
                {
                    if (rayHit.transform.CompareTag("Player"))
                    {
                        targetPlayer = hit.transform;
                        chasingPlayer = true;
                        foundPlayer = true;
                        return;
                    }
                }
            }
        }

        if (!foundPlayer)
        {
            if (!chasingPlayer)
                targetPlayer = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Радиус поиска
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Угол зрения
        Vector3 forward = transform.forward * detectionRange;

        Quaternion leftRotation = Quaternion.Euler(0, -visionAngle / 2f, 0);
        Quaternion rightRotation = Quaternion.Euler(0, visionAngle / 2f, 0);

        Vector3 leftDirection = leftRotation * forward;
        Vector3 rightDirection = rightRotation * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftDirection);
        Gizmos.DrawLine(transform.position, transform.position + rightDirection);
    }

    void SetNewPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolDestination = hit.position;
            agent.SetDestination(patrolDestination);
            hasPatrolDestination = true;
        }
    }

    void Patrol()
    {
        if (!hasPatrolDestination)
        {
            SetNewPatrolDestination();
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                waitTimer += Time.deltaTime;

                if (waitTimer >= patrolWaitTime)
                {
                    waitTimer = 0f;
                    hasPatrolDestination = false;
                }
            }
        }
    }

}
