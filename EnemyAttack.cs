using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float attackWindupTime = 0.5f;
    public float attackAngle = 60f;

    [Header("Detection Settings")]
    public LayerMask playerLayer;

    private float lastAttackTime;
    private bool isPreparingAttack = false;

    private EnemyStats enemyStats;
    private NavMeshAgent agent;

    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        DetectAndAttackPlayer();
    }

    void DetectAndAttackPlayer()
    {
        if (isPreparingAttack) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

        foreach (var hit in hits)
        {
            PlayerStats player = hit.GetComponent<PlayerStats>();
            if (player != null)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    StartCoroutine(PrepareAttack(player));
                    lastAttackTime = Time.time;
                }
                return;
            }
        }
    }

    IEnumerator PrepareAttack(PlayerStats player)
    {
        isPreparingAttack = true;

        // Остановить движение
        if (agent != null)
            agent.isStopped = true;

        yield return new WaitForSeconds(attackWindupTime);

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= attackRange)
            {
                Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

                if (angleToPlayer <= attackAngle / 2f)
                {
                    Attack(player);
                }
            }

            // Возобновить движение
            if (agent != null)
                agent.isStopped = false;

            isPreparingAttack = false;
        }

        void Attack(PlayerStats player)
        {
            player.TakeDamage(enemyStats.attackDamage);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}