using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Rendering;

public class UILoader : MonoBehaviour
{
    [SerializeField]
    private BuildingOnClick buildingOnClick;
    [SerializeField]
    private GameObject onClickBuilding;
    [SerializeField]
    private Text buildingName;

    [SerializeField]
    private ObjectsDatabaseSO database;

    public currencyCount currencyCount;
    public PolutionData polutionData;
    public NatureData natureData;
    public PopulationData populationData;
    public EnergyData energyData;
    public PlacementSystem placementSystem;
    public SwitchingLevels switchingLevels;

    private float startTime;

    Energy energy = new Energy();
    Currency currency = new Currency();
    Nature nature = new Nature();
    Polution polution = new Polution();
    Population population = new Population();

    public GameObject constructionTemplate;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        //FirestoreManager.SaveBuilding(this);
        FirestoreManager.FetchBuildings(this);
        FirestoreManager.FetchData(this, energy, currency, nature, polution, population);
    }
    private void OnApplicationQuit()
    {
        SaveDataOnClose();
    }

    public void LoadDataOnOpen()
    {
        energyData.UpdateEnergyData(energy);
        currencyCount.UpdateCurrencyCount(currency);
        natureData.UpdateNatureData(nature);
        polutionData.UpdatePolutionData(polution);
        populationData.UpdatePopulationData(population);
    }
    public void SaveDataOnClose()
    {
        energyData.UpdateEnergy(energy);
        currencyCount.UpdateCurrency(currency);
        natureData.UpdateNature(nature);
        polutionData.UpdatePolution(polution);
        populationData.UpdatePopulation(population);
        FirestoreManager.SaveData(energy, currency, nature, polution, population);
    }
    public void PlaceExistingStructures(ArrayList buildings)
    {
        foreach (Building building in buildings)
        {
            foreach (ObjectData structure in database.objectsData)
            {
                
                if (structure.Name.Equals(building.ModelName))
                {
                    DateTime currentTime = DateTime.Now;  // Get the current time
                    DateTime buildTime = building.TimeStamp.ToLocalTime();
                    TimeSpan timeDiff = currentTime - buildTime; 
                    // misal 40 detik
                    float timeElapse = (float)timeDiff.TotalSeconds;
                    float timeremaining;


                    var position = new Vector3();
                    position.x = building.PositionX;
                    position.y = building.PositionY;
                    position.z = building.PositionZ;


                    // 40 < 60 = build
                    if(building.isBuilded == true)
                    {
                        alreadyBuilded(structure, building, position);
                    }
                    else
                    {
                        if (timeElapse < structure.BuildingTime)
                        {
                            timeremaining = structure.BuildingTime - timeElapse;

                        }
                        else
                        {
                            timeremaining = 0;
                        }
                        Debug.Log(timeremaining + " = " + structure.BuildingTime + " - " + timeElapse);
                        StartCoroutine(ConstructingTime(building, structure.ID, position, building.Category, timeremaining));
                    }
                    
                }
            }
        }
    }
    private void alreadyBuilded(ObjectData structure, Building building, Vector3 position)
    {
        GameObject gameObject;
        gameObject = Instantiate(structure.Prefeb);
        gameObject.transform.position = position;
        gameObject.transform.parent = StructureBuilder.GetSubCategoryContainer(building.Category, structure.Type).transform;
        gameObject.name = building.Id;
        gameObject.transform.tag = StructureBuilder.GetStructureTag(building.Category);
        gameObject.layer = LayerMask.NameToLayer("Building");
        BuildingOnClick scriptComponent = gameObject.AddComponent<BuildingOnClick>();
        Vector3 worldPosition = gameObject.transform.position;
        Vector3Int gridPosition = new Vector3Int(
            Mathf.RoundToInt(worldPosition.z),
            Mathf.RoundToInt(worldPosition.x),
            Mathf.RoundToInt(worldPosition.y)
            );
        int buildingID = structure.ID;
        int baseModelIndex = structure.Name.IndexOf("WM") - 1;

        scriptComponent.InitializeBuilding(buildingID, gameObject, gridPosition, placementSystem.BuildingData, placementSystem, switchingLevels);
        scriptComponent.constructionTemplate = constructionTemplate;
        scriptComponent.OnClickBuilding = onClickBuilding;
        scriptComponent.BuildingName = buildingName;
        scriptComponent.database = database;
        scriptComponent.currencyCount = currencyCount;
        scriptComponent.polutionData = polutionData;
        scriptComponent.natureData = natureData;
        scriptComponent.populationData = populationData;
        scriptComponent.energyData = energyData;
        scriptComponent.buildingWasteManagementLevel = building.WasteManagementLevel;
        scriptComponent.KendaraanName = building.KendaraanName;
        scriptComponent.layerToHit = LayerMask.GetMask("Building");

        if (baseModelIndex > -1)
        {
            scriptComponent.baseModelName = structure.Name.Substring(0, baseModelIndex);
            Debug.Log(structure.Name.Substring(0, baseModelIndex));
        }
        else
        {
            scriptComponent.baseModelName = structure.Name;
        }
        scriptComponent.airPollution = structure.AirPolution;
        scriptComponent.waterPollution = structure.WaterPolution;
        scriptComponent.soilPollution = structure.SoilPolution;

        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.45f, 0.2f, 0.45f);

    }

    IEnumerator ConstructingTime(Building building,int ID, Vector3 position, String category, float timeRemaining)
    {
        float constructingTime = timeRemaining;
        // Create Building Construction
        GameObject gameObject = Instantiate(database.objectsData[7].Prefeb);
        gameObject.transform.position = position;
        gameObject.transform.rotation = Quaternion.Euler(0, 60, 0);
        gameObject.transform.parent = StructureBuilder.GetSubCategoryContainer(building.Category, database.objectsData[ID].Type).transform;
        gameObject.name = building.Id;
        gameObject.transform.tag = StructureBuilder.GetStructureTag(building.Category);


        // Create Building Text Time
        GameObject constructionDetail = Instantiate(constructionTemplate);
        constructionDetail.transform.parent = GameObject.Find(gameObject.name).transform;
        Vector3 constructionPosition = gameObject.transform.position;
        constructionPosition.y = 1.0f;
        constructionDetail.transform.position = constructionPosition;
        constructionDetail.GetComponent<TextMeshPro>().SetText("Construction Time: " + constructingTime);
        while (constructingTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            constructingTime -= 1.0f;
            constructionDetail.GetComponent<TextMeshPro>().SetText("Construction Time: " + constructingTime);
        }
        Destroy(constructionDetail);
        Destroy(gameObject);
        DataUpdateConstructionCompleted(ID);
        GameObject newPrefab = Instantiate(database.objectsData[ID].Prefeb);
        newPrefab.transform.position = gameObject.transform.position;
        newPrefab.transform.rotation = Quaternion.Euler(0, 60, 0);
        newPrefab.transform.name = gameObject.name;
        newPrefab.transform.tag = gameObject.transform.tag;
        newPrefab.transform.parent = gameObject.transform.parent;

        gameObject = newPrefab;
        gameObject.layer = LayerMask.NameToLayer("Building");
        BuildingOnClick scriptComponent = gameObject.AddComponent<BuildingOnClick>();
        Vector3 worldPosition = gameObject.transform.position;
        Vector3Int gridPosition = new Vector3Int(
            Mathf.RoundToInt(worldPosition.z),
            Mathf.RoundToInt(worldPosition.x),
            Mathf.RoundToInt(worldPosition.y)
            );
        int buildingID = ID;
        int baseModelIndex = database.objectsData[ID].Name.IndexOf("WM") - 1;

        scriptComponent.InitializeBuilding(buildingID, gameObject, gridPosition, placementSystem.BuildingData, placementSystem, switchingLevels);
        scriptComponent.constructionTemplate = constructionTemplate;
        scriptComponent.OnClickBuilding = onClickBuilding;
        scriptComponent.BuildingName = buildingName;
        scriptComponent.database = database;
        scriptComponent.currencyCount = currencyCount;
        scriptComponent.polutionData = polutionData;
        scriptComponent.natureData = natureData;
        scriptComponent.populationData = populationData;
        scriptComponent.energyData = energyData;
        scriptComponent.buildingWasteManagementLevel = building.WasteManagementLevel;
        scriptComponent.KendaraanName = building.KendaraanName;
        scriptComponent.layerToHit = LayerMask.GetMask("Building");

        if (baseModelIndex > -1)
        {
            scriptComponent.baseModelName = database.objectsData[ID].Name.Substring(0, baseModelIndex);
            Debug.Log(database.objectsData[ID].Name.Substring(0, baseModelIndex));
        }
        else
        {
            scriptComponent.baseModelName = database.objectsData[ID].Name;
        }
        scriptComponent.airPollution = database.objectsData[ID].AirPolution;
        scriptComponent.waterPollution = database.objectsData[ID].WaterPolution;
        scriptComponent.soilPollution = database.objectsData[ID].SoilPolution;
        scriptComponent.layerToHit = LayerMask.GetMask("Building");
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.4f, 0.2f, 0.4f);
        FirestoreManager.UpdateBuildingStatus(gameObject.name, true);
    }

    private void DataUpdateConstructionCompleted(int ID)
    {
        if (ID <= 3)
        {
            natureData.AddAirNature(database.objectsData[ID].AirPolution);
            natureData.AddSoilNature(database.objectsData[ID].SoilPolution);
            natureData.AddWaterNature(database.objectsData[ID].WaterPolution);
        }
        else if (ID == 113)
        {
            Debug.Log("masuk kok");
            populationData.increaseHousePopulation(database.objectsData[ID].EnergyProduction);
        }
        else
        {
            print(database.objectsData[ID].Currency);
            currencyCount.AddCurrency(database.objectsData[ID].Currency);
            polutionData.AddAirPolution(database.objectsData[ID].AirPolution);
            polutionData.AddWaterPolution(database.objectsData[ID].WaterPolution);
            polutionData.AddSoilPolution(database.objectsData[ID].SoilPolution);
            if (database.objectsData[ID].Category == "Energy")
            {
                energyData.IncreaseMaxEnergy(database.objectsData[ID].EnergyProduction, database.objectsData[ID].Type);
            }
            else if (database.objectsData[ID].Type == "Market")
            {
                populationData.increaseMaxPopulation(database.objectsData[ID].EnergyProduction);
            }
        }
        populationData.decreaseUsedPopulation(database.objectsData[ID].PopulationNeed);
        Debug.Log(database.objectsData[ID].PopulationNeed);
    }
}
