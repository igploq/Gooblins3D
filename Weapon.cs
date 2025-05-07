using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 25; 
    public float attackRange = 2f;
    public bool isEquipped = false;

    public bool canPoison = false; 
    public int poisonDamage = 5;  
    public float poisonDuration = 10f;
    public float poisonInterval = 1f;

    public bool canFreeze = false;
    public int hitsToFreeze = 3;
    public float freezeDuration = 3f;

    private Dictionary<GameObject, int> freezeHitCounter = new Dictionary<GameObject, int>();



    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (!isEquipped) return;

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackRange))
        {
            if (hit.collider.CompareTag("LivingObject"))
            {
                EnemyStats enemyStats = hit.collider.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(damage);

                   
                    if (canPoison)
                    {
                        PoisonEffect poison = hit.collider.GetComponent<PoisonEffect>();
                        if (poison != null)
                        {
                            poison.ApplyPoison();
                        }
                        else
                        {
                          
                            poison = hit.collider.gameObject.AddComponent<PoisonEffect>();
                            poison.poisonDamage = poisonDamage;
                            poison.poisonDuration = poisonDuration;
                            poison.poisonInterval = poisonInterval;
                            poison.ApplyPoison();
                        }
                    }

                    if (canFreeze)
                    {
                        if (!freezeHitCounter.ContainsKey(hit.collider.gameObject))
                            freezeHitCounter[hit.collider.gameObject] = 0;

                        freezeHitCounter[hit.collider.gameObject]++;

                        if (freezeHitCounter[hit.collider.gameObject] >= hitsToFreeze)
                        {
                            FreezeEffect freeze = hit.collider.GetComponent<FreezeEffect>();
                            if (freeze == null)
                            {
                                freeze = hit.collider.gameObject.AddComponent<FreezeEffect>();
                            }

                            freeze.Freeze(freezeDuration);
                            freezeHitCounter[hit.collider.gameObject] = 0;
                        }
                    }

                }
            }
            if (hit.collider.CompareTag("Player"))
            {
            
                PlayerStats playerStats = hit.collider.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damage);

                    if (canPoison)
                    {
                        PoisonEffect poison = hit.collider.GetComponent<PoisonEffect>();
                        if (poison != null)
                        {
                            poison.ApplyPoison();
                        }
                        else
                        {
                            poison = hit.collider.gameObject.AddComponent<PoisonEffect>();
                            poison.poisonDamage = poisonDamage;
                            poison.poisonDuration = poisonDuration;
                            poison.poisonInterval = poisonInterval;
                            poison.ApplyPoison();
                        }
                    }
                }
            }
                if (hit.collider.CompareTag("NonLivingObject"))
            {
                Dummy dummy = hit.collider.GetComponent<Dummy>();
                if (dummy != null)
                {
                    dummy.TakeDamage(damage);
                }
            }
        }
    }
}

