using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 10f;
    public float staminaDrainRate = 20f;
    public float staminaRegenDelay = 1.5f;

    private float lastStaminaUseTime;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        lastStaminaUseTime = -staminaRegenDelay;
    }

    void Update()
    {
        RegenerateStamina();
    }

    void RegenerateStamina()
    {
        if (Time.time > lastStaminaUseTime + staminaRegenDelay && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player damaged! Heatlh: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died");

        gameObject.layer = LayerMask.NameToLayer("Ghost");
        gameObject.tag = "Ghost";


        GetComponent<PlayerMovement>().enabled = false;

        
        if (TryGetComponent<GhostMovement>(out var ghost))
        {
            ghost.enabled = true;
        }

        
    }


    public void DrainStamina(float amount)
    {
        currentStamina -= amount * Time.deltaTime;
        currentStamina = Mathf.Max(currentStamina, 0f);
        lastStaminaUseTime = Time.time;
    }

    public bool HasStamina()
    {
        return currentStamina > 0f;
    }

    public void ApplyExplosionForce(Vector3 explosionPosition, float force, float radius)
    {
        Vector3 direction = (transform.position - explosionPosition).normalized;
        float distance = Vector3.Distance(transform.position, explosionPosition);
        float effect = 1 - (distance / radius);

        Vector3 push = direction * force * effect;

        if (TryGetComponent<PlayerMovement>(out var movement))
        {
            movement.AddImpact(push);
        }
    }

}
