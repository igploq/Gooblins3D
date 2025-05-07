using UnityEngine;

public class Currency : MonoBehaviour
{
    public int currentCurrency = 0;

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
    }

    public void RemoveCurrency(int amount)
    {
        currentCurrency -= amount;
        if (currentCurrency < 0)
            currentCurrency = 0;
    }

    public int GetCurrency()
    {
        return currentCurrency;
    }
}
