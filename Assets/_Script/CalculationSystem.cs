using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CalculationSystem : MonoBehaviour
{
    public currencyCount currencyCount;
    public PolutionData polutionData;
    public NatureData natureData;
    public TimerData timerData;
    public PopulationData populationData;
    public DisasterSystem disasterSystem;
    private bool invoked = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timerData.timerIncrement >= 0.6f)
        {
            CurrencyIncrement();
            NatureIncrement();
            PolutionIncrement();
            timerData.timerIncrement = 0.0f;
            //disasterSystem.SacrificeFarms();
            //disasterSystem.SacrificeRandomStructures(0.5);
        }
        if (timerData.timerPolution >= 6.0f)
        {
            polutionData.incrementPolution();
            ResetPolutionTemp();

            timerData.timerPolution = 0.0f;
        }
        if (timerData.timerNature >= 18.0f)
        {
            polutionData.natureIncome(natureData.airNatureTemp, natureData.waterNatureTemp, natureData.soilNatureTemp);
            ResetNatureTemp();

            timerData.timerNature = 0.0f;

        }
        if (populationData.populationDiff != 0)
        {
           
            if (!invoked)
            {
                if(populationData.populationDiff < 0)
                {
                    //print("population diff " + populationData.populationDiff);
                    Invoke("decreasePopulation", 1.0f);
                    invoked = true;
                }
                if(populationData.populationDiff > 0)
                {
                    Invoke("increasePopulation", 1.0f);
                    invoked = true;
                }
            }
        }
        UpdateText();
    }
    private void decreasePopulation()
    {
        populationData.decreasePopulation(-1*populationData.populationDiff);
        invoked = false;
    }
    private void increasePopulation()
    {
        populationData.increasePopulation(1);
        invoked = false;
    }

    private void UpdateText()
    {
        currencyCount.UpdateCurrencyText();
        polutionData.UpdatePolutionText();
        populationData.UpdatePopulationText();

    }
    private void CurrencyIncrement()
    {
        currencyCount.currency += currencyCount.currencyIncome;
    }
    private void NatureIncrement()
    {
        natureData.soilNatureTemp += natureData.soilNature;
        natureData.waterNatureTemp += natureData.waterNature;
        natureData.airNatureTemp += natureData.airNature;
    }
    private void PolutionIncrement()
    {
        //print("PolutiomTemp " + polutionData.airPolutionTemp + " income " + polutionData.airPolutionIncome);
        polutionData.airPolutionTemp += polutionData.airPolutionIncome;
        polutionData.waterPolutionTemp += polutionData.waterPolutionIncome;
        polutionData.soilPolutionTemp += polutionData.soilPolutionIncome;
    }


    private void ResetNatureTemp()
    {
        natureData.airNatureTemp = 0;
        natureData.waterNatureTemp = 0;
        natureData.soilNatureTemp = 0;
    }
    private void ResetPolutionTemp()
    {
        polutionData.airPolutionTemp = 0;
        polutionData.waterPolutionTemp = 0;
        polutionData.soilPolutionTemp = 0;
    }
}
