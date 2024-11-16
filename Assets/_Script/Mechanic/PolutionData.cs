using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase.Firestore;

public class PolutionData : MonoBehaviour
{
    public float airPolution = 0;
    public Text airText;

    public float airPolutionIncome = 0;
    public float airPolutionTemp = 0;
    public Slider airPolutionBar;
    int maxAirPolution = 500;
    int maxAirPolution2 = 350;
    int maxAirPolution3 = 500;


    public float waterPolution = 0;
    public Text waterText;

    public float waterPolutionIncome = 0;
    public float waterPolutionTemp = 0;
    public Slider waterPolutionBar;
    int maxWaterPolution = 250;
    int maxWaterPolution2 = 350;
    int maxWaterPolution3 = 500;

    public float soilPolution = 0;
    public Text soilText;

    public float soilPolutionIncome = 0;
    public float soilPolutionTemp = 0;
    public Slider soilPolutionBar;
    int maxSoilPolution = 250;
    int maxSoilPolution2 = 350;
    int maxSoilPolution3 = 500;

    [SerializeField]
    public ObjectsDatabaseSO database;
    //[SerializeField]
    //public BencanaImage bencanaImage;

    public int currentPollutionBarLimit = 0;

    private void Awake()
    {
        FetchPollutionLimitData();
    }

    public void UpdatePolution(Polution polution)
    {
        polution.airPolution = airPolution;
        polution.airPolutionIncome = airPolutionIncome;
        polution.airPolutionTemp = airPolutionTemp;

        polution.waterPolution = waterPolution;
        polution.waterPolutionIncome = waterPolutionIncome;
        polution.waterPolutionTemp = waterPolutionTemp;

        polution.soilPolution = soilPolution;
        polution.soilPolutionIncome = soilPolutionIncome;
        polution.soilPolutionTemp = soilPolutionTemp;
    }
    public void UpdatePolutionData(Polution polution)
    {
        airPolution = polution.airPolution;
        airPolutionIncome = polution.airPolutionIncome;
        airPolutionTemp = polution.airPolutionTemp;

        waterPolution = polution.waterPolution;
        waterPolutionIncome = polution.waterPolutionIncome;
        waterPolutionTemp = polution.waterPolutionTemp;

        soilPolution = polution.soilPolution;
        soilPolutionIncome = polution.soilPolutionIncome;
        soilPolutionTemp = polution.soilPolutionTemp;
        UpdatePolutionText();
    }

    public void incrementPolution()
    {
        if (airPolution != maxAirPolution)
        {
            airPolution += airPolutionTemp;
            if (airPolution > maxAirPolution)
            {
                airPolution = maxAirPolution;
            }
        }
        if (waterPolution != maxWaterPolution)
        {

            waterPolution += waterPolutionTemp;
            if (waterPolution > maxWaterPolution)
            {
                waterPolution = maxWaterPolution;
            }
        }
        if (soilPolution != maxSoilPolution)
        {
            soilPolution += soilPolutionTemp;

            if (soilPolution > maxSoilPolution)
            {
                soilPolution = maxSoilPolution;
            }
        }

        //print("test" + currencyIncome + " " + currency);
        UpdatePolutionText();
    }
    public void natureIncome(float air, float water, float soil)
    {
        if (airPolution != 0)
        {
            airPolution -= air;
            if (airPolution < 0)
            {
                airPolution = 0;
            }
        }
        if (waterPolution != 0)
        {
            waterPolution -= water;
            if (waterPolution < 0)
            {
                waterPolution = 0;
            }
        }
        if (soilPolution != 0)
        {
            soilPolution -= soil;

            if (soilPolution < 0)
            {
                soilPolution = 0;
            }
        }
        UpdatePolutionText();
    }
    void Start()
    {
        UpdatePolutionText();
    }
    public void AddAirPolution(float amount)
    {
        airPolutionIncome += amount;
    }

    public void AddSoilPolution(float amount)
    {
        soilPolutionIncome += amount;
    }

    public void AddWaterPolution(float amount)
    {
        waterPolutionIncome += amount;
    }

    public void DecreaseAirPolution(float amount)
    {
        airPolutionIncome -= amount;
        CheckForPollutionBarLimitDecreased();
    }

    public void DecreaseSoilPolution(float amount)
    {
        soilPolutionIncome -= amount;
        CheckForPollutionBarLimitDecreased();
    }

    public void DecreaseWaterPolution(float amount)
    {
        waterPolutionIncome -= amount;
        CheckForPollutionBarLimitDecreased();
    }

    public void DecreaseAirPollutionByPercentage(float percentage)
    {
        airPolution *= percentage;
    }

    public void DecreaseWaterPollutionByPercentage(float percentage)
    {
        waterPolution *= percentage;
    }

    public void DecreaseSoilPollutionByPercentage(float percentage)
    {
        soilPolution *= percentage;
    }

    public void UpdatePolutionText()
    {
        if (airText != null)
        {
            airText.text = "Air Polution:";
            soilText.text = "Soil Polution:";
            waterText.text = "Water Polution:";
        }
    }

    void Update()
    {
        polutionBarFiller();
        //if (soilPolution >= 5)
        //{
        //    soilPolution = soilPolution * 90 / 100;
        //    bencanaImage.Func_PlayUIAnim();
        //}
        

    }
    void polutionBarFiller()
    {
        airPolutionBar.value = airPolution;
        airPolutionBar.maxValue = maxAirPolution3;
        waterPolutionBar.value = waterPolution;
        waterPolutionBar.maxValue = maxWaterPolution3;
        soilPolutionBar.value = soilPolution;
        soilPolutionBar.maxValue = maxSoilPolution3;
    }

    public Tuple<string, int> getActivePolution()
    {
        if (airPolution >= 250)
            return Tuple.Create("Air", (int)airPolution);
        else if (waterPolution >= 250)
            return Tuple.Create("Water", (int)airPolution);
        else
            return Tuple.Create("Soil", (int)airPolution);
    }

    public bool isPolutionBarExceeded()
    {
        if (((airPolution >= 250 || waterPolution >= 250 || soilPolution >= 250) && currentPollutionBarLimit == 0) ||
            ((airPolution >= 350 || waterPolution >= 350 || soilPolution >= 350) && currentPollutionBarLimit == 250) ||
                ((airPolution >= 500 || waterPolution >= 500 || soilPolution >= 500) && currentPollutionBarLimit == 350))
        {
            Debug.Log("exceeded true" + currentPollutionBarLimit);
            return true;
        }
        //Debug.Log("exceeded false" + currentPollutionBarLimit);
        return false;
    }

    public void CheckForPollutionBarLimitDecreased()
    {
        if (airPolution < 500 && waterPolution < 500 && soilPolution < 500 && currentPollutionBarLimit == 500)
            currentPollutionBarLimit = 300;
        else if (airPolution < 350 && waterPolution < 350 && soilPolution < 350 && currentPollutionBarLimit == 350)
            currentPollutionBarLimit = 250;
        else
            currentPollutionBarLimit = 0;
    }

    private async void FetchPollutionLimitData()
    {
        DocumentSnapshot snapshot = await FirebaseFirestore.DefaultInstance.Collection("users").Document("player").GetSnapshotAsync();

        if (snapshot.Exists)
        {
            currentPollutionBarLimit = snapshot.GetValue<int>("current_pollution_bar_limit");
        }
    }

    public void SetNewPollutionBarLimit(int newLimit)
    {
        currentPollutionBarLimit = newLimit;
    }
}