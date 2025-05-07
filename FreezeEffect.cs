using UnityEngine;

public class FreezeEffect : MonoBehaviour
{
    private float freezeTimer = 0f;
    private bool isFrozen = false;

    private EnemyNavigation enemyNav;
    private EnemyAttack enemyAttack;

    public void Freeze(float duration)
    {
        if (!isFrozen)
        {
            enemyNav = GetComponent<EnemyNavigation>();
            enemyAttack = GetComponent<EnemyAttack>();

            if (enemyNav != null) enemyNav.enabled = false;
            if (enemyAttack != null) enemyAttack.enabled = false;

            isFrozen = true;
            freezeTimer = duration;

        }
    }

    void Update()
    {
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;

            if (freezeTimer <= 0f)
            {
                Unfreeze();
            }
        }
    }

    private void Unfreeze()
    {
        if (enemyNav != null) enemyNav.enabled = true;
        if (enemyAttack != null) enemyAttack.enabled = true;

        isFrozen = false;

    }
}
