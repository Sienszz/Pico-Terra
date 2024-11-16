using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyData : MonoBehaviour
{
    public int maxElectricity;
    public int maxMineral;
    public int maxFood;

    public int usedElectricity;
    public int usedMineral;
    public int usedFood;

    public Text electricityText;
    public Text mineralText;
    public Text foodText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateEnergyText();
    }
    public void UpdateEnergy(Energy energy)
    {
        energy.maxElectricity = maxElectricity;
        energy.maxMineral = maxMineral;
        energy.maxFood = maxFood;
        energy.usedElectricity = usedElectricity;
        energy.usedMineral = usedMineral;
        energy.usedFood = usedFood;
    }

    public void UpdateEnergyData(Energy energy)
    {
        maxElectricity = energy.maxElectricity;
        maxMineral = energy.maxMineral;
        maxFood = energy.maxFood;
        usedElectricity = energy.usedElectricity;
        usedMineral = energy.usedMineral;
        usedFood = energy.usedFood;
        UpdateEnergyText();
    }

    public bool CheckUsedEnergy(int food, int mineral, int electricity)
    {
        if (food + usedFood > maxFood)
        {
            return false;
        }
        if (mineral + usedMineral > maxMineral)
        {
            return false;
        }
        if (electricity + usedElectricity > maxElectricity)
        {
            return false;
        }
        return true;
    }
    public void IncreaseMaxEnergy(int amount, string name)
    {
        if(name == "Farm")
        {
            maxFood += amount;
        }
        else if(name == "Minerals")
        {
            maxMineral += amount;
        }
        else if(name == "Electricity")
        {
            maxElectricity += amount;
        }
        UpdateEnergyText();
    }
    public bool DecreaseMaxEnergy(int amount, string name)
    {
        if (name == "Farm")
        {
            if(maxFood - usedFood < amount)
            {
                return false;
            }
            maxFood -= amount;
        }
        else if (name == "Minerals")
        {
           if(maxMineral - usedMineral < amount)
            {
                return false;
            }
            maxMineral -= amount;
        }
        else if (name == "Electricity")
        {
            if(maxElectricity - usedElectricity < amount)
            {
                return false;
            }
            maxElectricity -= amount;
        }
        UpdateEnergyText();
        return true;
    }
    public void IncreaseUsedEnergy(int food, int mineral, int electricity)
    {
        usedFood += food;
        usedMineral += mineral;
        usedElectricity += electricity;
        UpdateEnergyText();
    }
    public void DecreaseUsedEnergy(int food, int mineral, int electricity)
    {
        usedFood -= food;
        usedMineral -= mineral;
        usedElectricity -= electricity;
        UpdateEnergyText();
    }
    public void UpdateEnergyText()
    {
        if (electricityText != null)
        {
            foodText.text = usedFood.ToString() + " / " + maxFood;
            mineralText.text = usedMineral.ToString() + " / " + maxMineral;
            electricityText.text = usedElectricity.ToString() + " / " + maxElectricity;
            
            
        }
    }
}
