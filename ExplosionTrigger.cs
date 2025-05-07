using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
    public Explosion explosionScript;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            if (explosionScript != null)
            {
                explosionScript.Explode();
            }
            
        }
    }
}
