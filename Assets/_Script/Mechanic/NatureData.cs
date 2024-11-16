using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureData : MonoBehaviour
{
    public float airNature = 0;
    public float waterNature = 0;
    public float soilNature = 0;
    public float soilNatureTemp = 0;
    public float waterNatureTemp = 0;
    public float airNatureTemp = 0;
    private TimerData timerData;
    // Start is called before the first frame update
    public void UpdateNature(Nature nature)
    {
        nature.airNature = airNature;
        nature.waterNature = waterNature;
        nature.soilNature = soilNature;
        nature.soilNatureTemp = soilNatureTemp;
        nature.waterNatureTemp = waterNatureTemp;
        nature.airNatureTemp = airNatureTemp;
    }

    public void UpdateNatureData(Nature nature)
    {
        airNature = nature.airNature;
        waterNature = nature.waterNature;
        soilNature = nature.soilNature;
        soilNatureTemp = nature.soilNatureTemp;
        waterNatureTemp = nature.waterNatureTemp;
        airNatureTemp = nature.airNatureTemp;
    }


    public void AddAirNature(float amount)
    {
        airNature += amount;
    }

    public void AddSoilNature(float amount)
    {
        soilNature += amount;
    }

    public void AddWaterNature(float amount)
    {
        waterNature += amount;
    }
}