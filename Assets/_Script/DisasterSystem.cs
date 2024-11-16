using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Random = System.Random;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

public class DisasterSystem : MonoBehaviour
{
    public ObjectsDatabaseSO database;
    public PolutionData pollutionData;
    public PopulationData populationData;
    public ArrayList karmaPollution;
    public Tuple<string, int> activePollution;
    private bool karmaToday = false;

    [SerializeField]
    public BencanaImage bencanaImage;
    
    public bool isTriggeringDisaster = false;

    void Update()
    {
        if (pollutionData.isPolutionBarExceeded())
        {
            activePollution = pollutionData.getActivePolution();

            //ShowDisasterCanvas();
            if (isTriggeringDisaster == false)
            {
                isTriggeringDisaster = true;
                TriggerDisaster();
            }
        }

        if (karmaToday)
        {
            TriggerKarma();
        }
    }

    private void Awake()
    {
        FetchTodaysKarma();
    }

    private void FetchTodaysKarma()
    {
        karmaPollution = new ArrayList();
        FirebaseFirestore.DefaultInstance.Collection("users").Document("player").Collection("karma")
            .WhereLessThanOrEqualTo("TriggerDateTiime", DateTime.Now.AddDays(1))
            .GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            foreach (DocumentSnapshot document in task.Result)
            {
                karmaPollution.Add(document.ConvertTo<Karma>());
            }

            if (karmaPollution.Count > 0)
                karmaToday = true;
        });
    }

    private void TriggerKarma()
    {
        foreach (Karma karma in karmaPollution) {
            if (karma.TriggerDateTime >= DateTime.Now)
            {
                if (karma.PollutionType.Equals("Air"))
                    pollutionData.AddAirPolution(karma.PollutionAmount);
                else if (karma.PollutionType.Equals("Water"))
                    pollutionData.AddWaterPolution(karma.PollutionAmount);
                else
                    pollutionData.AddSoilPolution(karma.PollutionAmount);

                FirestoreManager.DeleteKarma(karma.Id);
                karmaPollution.Remove(karma);
            }
        }
    }

    private void ShowDisasterCanvas()
    {
        bencanaImage.showChoices();
    }

    private DateTime RandomKarmaDay()
    {
        DateTime startDate = DateTime.Now.AddDays(1);
        DateTime endDate = startDate.AddDays(6);
        Random random = new Random();
        TimeSpan timeSpan = endDate - startDate;
        TimeSpan newSpan = new TimeSpan(0, random.Next(0, (int)timeSpan.TotalMinutes), 0);
        DateTime newDate = startDate + newSpan;
        return newDate;
    }

    private void TriggerDisaster()
    {
        Debug.Log("trigger " + activePollution.ToString());
        if (activePollution.Item2 >= 500)
        {
            if (activePollution.Item1.Equals("Air"))
            {
                bencanaImage.ShowWarningCanvas(new DisasterUIData(
                    "FAST!! Air spirit is in critical condition!",
                    "AirBarWarning1",
                    "AirBarWarningOff1",
                    "Ekosistem mengalami kebanjiran",
                    0,
                    "Air Spirit"));
            }
            else if (activePollution.Item1.Equals("Water"))
            {
                bencanaImage.ShowWarningCanvas(new DisasterUIData(
                    "HELP! Water Spirit conditions is getting worse!",
                    "WaterBarWarning1",
                    "WaterBarWarningOff1",
                    "Ekosistem mengalami kebanjiran",
                    0,
                    "Water Spirit"));
            }
            else
            {
                bencanaImage.ShowWarningCanvas(new DisasterUIData(
                    "HELP! Land Spirit conditions is getting worse!",
                    "SoilBarWarning1",
                    "SoilBarWarningOff1",
                    "Ekosistem mengalami kebanjiran",
                    0,
                    "Land Spirit"));
            }
        }
        else if (activePollution.Item2 >= 350)
        {
            if (activePollution.Item1.Equals("Air"))
            {
                bencanaImage.ShowWarningCanvas(new DisasterUIData(
                    "HELP! Air Spirit conditions is getting worse!",
                    "AirBarWarning1",
                    "AirBarWarningOff1",
                    "10% FarmBarn mengalami kebakaran",
                    1,
                    "Air Spirit"));
            }
            else if (activePollution.Item1.Equals("Water"))
            {
                bencanaImage.ShowWarningCanvas(new DisasterUIData(
                    "HELP! Water Spirit conditions is getting worse!",
                    "WaterBarWarning1",
                    "WaterBarWarningOff1",
                    "10% FarmBarn mengalami kebakaran",
                    1,
                    "Water Spirit"));
            }
            else
            {
                bencanaImage.ShowWarningCanvas(new DisasterUIData(
                    "HELP! Land Spirit conditions is getting worse!",
                    "SoilBarWarning1",
                    "SoilBarWarningOff1",
                    "10% FarmBarn mengalami kebakaran",
                    1,
                    "Land Spirit"));
            }
        }
        else
        {
            if (activePollution.Item1.Equals("Air"))
            {
                bencanaImage.ShowWarningCanvas(new DisasterUIData(
                    "OH NO! Air Spirit is getting sick!",
                    "AirBarWarning1",
                    "AirBarWarningOff1",
                    "Populasi akan berkurang sebanyak " + populationData.currPopulation * 0.05,
                    1,
                    "Air Spirit"));
            }
            else if (activePollution.Item1.Equals("Water"))
            {
                bencanaImage.ShowWarningCanvas(new DisasterUIData(
                    "OH NO! Water Spirit is getting sick!",
                    "WaterBarWarning1",
                    "WaterBarWarningOff1",
                    "Penghasilan Transportation Factory akan berkurang sebanyak 15%",
                    1,
                    "Water Spirit"));
            }
            else
            {
                bencanaImage.ShowWarningCanvas(new DisasterUIData(
                    "OH NO! Land Spirit is getting sick!",
                    "SoilBarWarning1",
                    "SoilBarWarningOff1",
                    "Penghasilan currency FarmBarn akan berkurang sebanyak 20%",
                    1,
                    "Land Spirit"));
            }
        }
    }

    public void Sacrifice()
    {
        if (activePollution.Item2 >= 100)
            SacrificeRandomStructures(0.2);
        else if (activePollution.Item2 >= 80)
            SacrificeFarms();
        else
            SacrificeRandomStructures(0.05);

        FirestoreManager.SaveSacrificeKarmaTimestamp();
        DecreasePollution();
        bencanaImage.HideDisasterCanvas();
        isTriggeringDisaster = false;
    }

    public void Karma()
    {
        Karma karma = new Karma
        {
            Id = Guid.NewGuid().ToString(),
            PollutionType = activePollution.Item1,
            TriggerDateTime = RandomKarmaDay()
        };

        karmaPollution.Add(karma);
        FirestoreManager.SaveSacrificeKarmaTimestamp();
        FirestoreManager.SaveKarma(karma);
        DecreasePollution();
        bencanaImage.HideDisasterCanvas();
        isTriggeringDisaster = false;
    }

    public void Destruction()
    {
        if (activePollution.Item2 >= 500)
        {
            DestructionPollution100();
            SetNewPollutionBarLimit(500);
        }
        else if (activePollution.Item2 >= 350)
        {
            DestructionPollution80();
            SetNewPollutionBarLimit(350);
        }
        else
        {
            DestructionPollution60();
            SetNewPollutionBarLimit(250);
        }

        Debug.Log("bencana image " + bencanaImage);
        bencanaImage.HideDisasterCanvas();
        isTriggeringDisaster = false;
    }

    private void DecreasePollution()
    {
        if (activePollution.Item1.Equals("Air"))
            pollutionData.DecreaseAirPollutionByPercentage((float) 0.9);
        else if (activePollution.Item1.Equals("Water"))
            pollutionData.DecreaseWaterPollutionByPercentage((float) 0.9);
        else
            pollutionData.DecreaseSoilPollutionByPercentage((float) 0.9);
    }

    private void SetNewPollutionBarLimit(int limit)
    {
        pollutionData.SetNewPollutionBarLimit(limit);
        FirestoreManager.UpdatePollutionBarLimit(limit);
    }

    private void DestructionPollution60()
    {
        if (activePollution.Item1.Equals("Air"))
            DestructionAirPollution();
        else if (activePollution.Item1.Equals("Soil"))
            DestructionSoilPollution();
        else if (activePollution.Item1.Equals("Water"))
            DestructionWaterPollution();
    }

    public void GenerateRandomKarma()
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(10, 30);

        if (karmaPollution[0].Equals("Air"))
            pollutionData.AddAirPolution(randomValue);
        else if (karmaPollution[1].Equals("Water"))
            pollutionData.AddWaterPolution(randomValue);
        else
            pollutionData.AddSoilPolution(randomValue);
    }

    public void SacrificeFarms()
    {
        Transform farms = GameObject.Find("FarmBarnContainer").transform;
        int farmCount = farms.childCount;
        int numberOfBurnt = (int)(farmCount * 0.4);

        for (int i = 0; i < numberOfBurnt; i++)
        {
            GameObject randomFarm = farms.GetChild(UnityEngine.Random.Range(0, farmCount)).gameObject;
            ReplaceStructure(randomFarm, GetNatureReplacement());
        }
    }

    public void SacrificeRandomStructures(double sacrificePercentage)
    {
        GameObject[] affectableStructures = GameObject.FindGameObjectsWithTag("AffectableStructure");
        int structureCount = affectableStructures.Length;
        int numberOfSacrificed = (int)(structureCount * sacrificePercentage);

        for (int i = 0; i < numberOfSacrificed; i++)
        {
            GameObject randomStructure = affectableStructures[UnityEngine.Random.Range(0, structureCount)];
            ReplaceStructure(randomStructure, GetNatureReplacement());
        }
    }

    public void DestructionPollution80()
    {
        Transform farms = GameObject.Find("FarmContainer").transform;
        int farmCount = farms.childCount;
        int numberOfBurnt = (int)(farmCount);

        for (int i = 0; i < numberOfBurnt; i++)
        {
            GameObject randomFarm = farms.GetChild(UnityEngine.Random.Range(0, farmCount)).gameObject;
            DeleteStructure(randomFarm);
        }
    }

    private ObjectData GetNatureReplacement()
    {
        if (activePollution.Item1.Equals("Soil"))
            return GetObjectData("Land Rock");
        else if (activePollution.Item1.Equals("Water"))
            return GetObjectData("ecengGondok");
        else
            return GetObjectData("Forest");
    }

    private ObjectData GetObjectData(string name)
    {
        foreach (ObjectData structure in database.objectsData)
        {
            if (structure.Name == name)
                return structure;
        }
        return null;
    }

    private void DeleteStructure(GameObject structure)
    {
        GameObject.Find(structure.name).GetComponent<BuildingOnClick>().DeleteBuilding(structure);
        //selectedData.DeleteObjectAt(structure.transform.position, structure.transform.localScale);
        //FirestoreManager.DeleteBuilding(structure.name);
        //Destroy(structure);
        //FirestoreManager.DeleteBuilding(structure.name);
        //Destroy(structure);
    }

    private void ReplaceStructure(GameObject oldStructure, ObjectData newStructure)
    {
        GameObject newObject = Instantiate(newStructure.Prefeb);
        newObject.transform.position = oldStructure.transform.position;
        newObject.name = Guid.NewGuid().ToString();

        Building addedBuilding = new Building
        {
            PositionX = oldStructure.transform.position.x,
            PositionY = oldStructure.transform.position.y,
            PositionZ = oldStructure.transform.position.z,
            ModelName = newStructure.Name,
            Id = newObject.name,
            Category = newStructure.Category
        };

        DeleteStructure(oldStructure);
        FirestoreManager.SaveBuilding(addedBuilding);
    }

    public void DestructionPollution100()
    {
        populationData.decreaseMaxPopulation((int)(populationData.maxPopulation * 0.2));
    }

    public void DestructionAirPollution()
    {
        populationData.decreaseMaxPopulation((int)(populationData.maxPopulation * 0.05));
    }

    public void DestructionSoilPollution()
    {
        GetObjectData("FarmBarn").Currency = GetObjectData("FarmBarn").Currency * 80 / 100;
        pollutionData.soilPolution = pollutionData.soilPolution * 90 / 100;
        //bencanaImage.Func_PlayUIAnim();
    }

    public void DestructionWaterPollution()
    {
        GetObjectData("Factory Kendaraan").Currency = GetObjectData("Factory Kendaraan").Currency * 85 / 100;
        pollutionData.waterPolution = pollutionData.waterPolution * 90 / 100;
        //bencanaImage.Func_PlayUIAnim();
    }
}
