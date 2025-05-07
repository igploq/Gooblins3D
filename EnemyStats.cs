using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Attack Settings")]
    public int attackDamage = 10;

    [Header("Drop Settings")]
    public GameObject coinPrefab;
    public float dropRadius = 1f;
    public float forceStrength = 2f;

    private Currency currency;

    void Start()
    {
        currentHealth = maxHealth;
        currency = GetComponent<Currency>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("DOMINATION!");

        if (currency != null && coinPrefab != null)
        {
            int totalCoins = currency.GetCurrency();
            for (int i = 0; i < totalCoins; i++)
            {
                Vector3 randomOffset = Random.insideUnitSphere * dropRadius;
                randomOffset.y = Mathf.Abs(randomOffset.y);

                Vector3 spawnPosition = transform.position + randomOffset;
                GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

                Rigidbody coinRigidbody = coin.GetComponent<Rigidbody>();
                if (coinRigidbody != null)
                {
                    Vector3 randomForce = new Vector3(
                        Random.Range(-forceStrength, forceStrength),
                        Random.Range(1f, forceStrength),
                        Random.Range(-forceStrength, forceStrength)
                    );
                    coinRigidbody.AddForce(randomForce, ForceMode.Impulse);
                }
            }
        }

        Destroy(gameObject);
    }
}
