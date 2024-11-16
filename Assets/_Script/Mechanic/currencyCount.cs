using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class currencyCount : MonoBehaviour
{
    [SerializeField]
    public int currency;
    public int currencyIncome;
    public Text currencyText;

    void Start()
    {
        UpdateCurrencyText();
    }
    public void UpdateCurrency(Currency curr)
    {
        curr.currency = currency;
        curr.currencyIncome = currencyIncome;
    }
    public void UpdateCurrencyCount(Currency curr)
    {
        currency = curr.currency;
        currencyIncome = curr.currencyIncome;
        UpdateCurrencyText();
    }

    public int GetCurrency
    {
        get { return currency; }
    }

    public bool CheckCurrency(int amount)
    {
        if(currency >= amount)
        {
            return true;
        }
        return false;
    }
    public void AddCurrency(int amount)
    {
        currencyIncome += amount;
        //print(amount);
    }
    public void DecreaseCurrency(int amount)
    {
        currencyIncome -= amount;
    }
    public void incrementCurrency()
    {
        currency += currencyIncome;
        //print("test" + currencyIncome + " " + currency);
        UpdateCurrencyText();
    }

    public bool SubtractCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            UpdateCurrencyText();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateCurrencyText()
    {
        if (currencyText != null)
        {
            currencyText.text = currency.ToString();
        }
    }
}
