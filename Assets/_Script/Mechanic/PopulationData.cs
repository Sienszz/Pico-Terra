using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationData : MonoBehaviour
{
    [SerializeField]
    public int housePopulation;// population gained from house (house)

    [SerializeField]
    public int maxPopulation; // maksimal populasi yang bisa di dapetin user (market) 
    // maxpopulation
    public int currPopulation; // populasi yang ada sekarang
    public int usedPopulation; // populasi yang lagi dipake buat kerja
    public Text populationText;
    public int populationDiff = 0; // perbedaan antara maksimal populasi dan populasi sekarang

    public void UpdatePopulation(Population population)
    {
        population.housePopulation = housePopulation;
        population.maxPopulation = maxPopulation;
        population.currPopulation = currPopulation;
        population.usedPopulation = usedPopulation;
        population.populationDiff = populationDiff;
    }
    public void UpdatePopulationData(Population population)
    {
        housePopulation = population.housePopulation;
        maxPopulation = population.maxPopulation;
        currPopulation = population.currPopulation;
        usedPopulation = population.usedPopulation;
        populationDiff = population.populationDiff;
        UpdatePopulationText();
    }

    public void increaseHousePopulation(int amount)
    {
        housePopulation += amount;
        checkPopulationDiff();
    }
    public void decreaseHousePopulation(int amount)
    {
        housePopulation -= amount;
        checkPopulationDiff();
    }
    public void increaseMaxPopulation(int amount)
    {
        maxPopulation += amount;
        checkPopulationDiff();
    }
    public void decreaseMaxPopulation(int amount)
    {
        maxPopulation -= amount;
        checkPopulationDiff();
    }
    public void increasePopulation(int amount)
    {
        currPopulation += amount;
        populationDiff -= amount;
    }
    public void decreasePopulation(int amount)
    {
        if(currPopulation-usedPopulation >= amount)
        {
            currPopulation -= amount;
        }
        else
        {
            currPopulation -= currPopulation - usedPopulation;

        }
        
        populationDiff -= amount;
    }
    public bool CheckUsedPopulation(int amount)
    {
        if (usedPopulation + amount <= currPopulation)
        {
            return true;
        }
        return false;
    }
    public void increaseUsedPopulation(int amount)
    {
        usedPopulation += amount;
    }
    public void decreaseUsedPopulation(int amount)
    {
        usedPopulation -= amount;
    }
    public void checkPopulationDiff()
    {
        if(housePopulation >= maxPopulation)
        {
            populationDiff = maxPopulation - currPopulation;
        }
        else
        {
            populationDiff = housePopulation - currPopulation;
        }
    }
    void Start()
    {
        checkPopulationDiff();
        UpdatePopulationText();
    }

    public void UpdatePopulationText()
    {
        if (populationText != null)
        {
            populationText.text = (currPopulation-usedPopulation).ToString() + " / " + currPopulation.ToString();
        }
    }
}