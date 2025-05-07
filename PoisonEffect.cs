using System.Collections;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    public int poisonDamage = 5; 
    public float poisonDuration = 10f;
    public float poisonInterval = 1f; 

    private float poisonTimer = 0f; 
    private bool isPoisoned = false;

    private void Update()
    {
        if (isPoisoned)
        {
            poisonTimer += Time.deltaTime;

            if (poisonTimer >= poisonInterval)
            {
                ApplyPoisonDamage();
                poisonTimer = 0f;
            }

            if (poisonTimer >= poisonDuration)
            {
                StopPoison();
            }
        }
    }

    public void ApplyPoison()
    {
        isPoisoned = true;
    }

    private void ApplyPoisonDamage()
    {

        EnemyStats enemyStats = GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(poisonDamage);
        }
        PlayerStats playerStats = GetComponent<PlayerStats>();
        if (playerStats != null)
        {

            playerStats.TakeDamage(poisonDamage);

        }
    }
        private void StopPoison()
    {
        isPoisoned = false;
        poisonTimer = 0f;
    }
}
