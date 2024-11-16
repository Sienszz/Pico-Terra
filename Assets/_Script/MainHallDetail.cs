using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainHallDetail : MonoBehaviour
{
    public Text currencyText;
    public Text lv;
    public currencyCount currentCurrency;
    private int currency;
    public ObjectsDatabaseSO database;
    public SwitchingLevels mainhallData;
    public MainhallRequirement requirement;
    private int mainhallPrice;
    private int currentLevel;


    void Start()
    {

        currentLevel = mainhallData.current_level;
        mainhallPrice = mainhallData.currencyNeeds[currentLevel];
        UpdateCurrencyText(GetCurrencyText());
        UpdateLv();

    }

    private void Update()
    {
        currentLevel = mainhallData.current_level;
        mainhallPrice = mainhallData.currencyNeeds[currentLevel];
        UpdateCurrencyText(GetCurrencyText());
        UpdateLv();
        requirement.updateRequirement();

    }

    public Text GetCurrencyText()
    {
        return currencyText;
    }

    public void UpdateCurrencyText(Text currencyText)
    {
        if (currencyText != null)
        {
            currency = currentCurrency.GetCurrency;
            currencyText.text = mainhallPrice.ToString();
        }
        currencyText.color = currency < mainhallPrice ? new Color(0.7f, 0.26f, 0.24f, 1f) : Color.black;
    }

    

    public void UpdateLv()
    {
        if (lv != null)
        {
            lv.text = "LV " + (currentLevel + 1).ToString();

        }
    }

}