using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;



    private void OnTriggerEnter(Collider other)
    {
        Currency Currency = other.GetComponent<Currency>();
        if (Currency != null)
        {
            Currency.AddCurrency(value);
            Destroy(transform.parent.gameObject);
        }
    }


}
