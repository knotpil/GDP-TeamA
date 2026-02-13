using UnityEngine;
using System;

public class CurrencyManager : MonoBehaviour
{
    private double currency = 0;

    public void payment(double p)
    {
        currency += p;
        currency = Math.Truncate(currency * 100) / 100;
        Debug.Log("Earned $" + p + ", TOTAL: $" + currency);
    }

    public bool buy(double b)
    {
        if(currency - b < 0) return false;
        currency -= b;
        currency = Math.Truncate(currency * 100) / 100;
        Debug.Log("Spent $" + b + ", TOTAL: $" + currency);
        return true;
    }

    public double getCurrency() {return currency;}
}
