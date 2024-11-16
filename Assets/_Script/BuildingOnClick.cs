using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingOnClick : MonoBehaviour
{
    [SerializeField] private Grid grid;
    private List<GameObject> placedGameObjects = new();
    private GridData mainHallData, buildingData;

    public GameObject constructionTemplate;
    public int buildingID;
    private Vector3Int gridPosition;
    public GameObject OnClickBuilding;
    public Text BuildingName;
    public ObjectsDatabaseSO database;
    private Vector3 offset;
    [SerializeField]
    public Vector3Int location;
    private GameObject structure;
    public PlacementSystem placementSystem;
    private Vector2Int objSize;

    public SwitchingLevels switchingLevel;
    public currencyCount currencyCount;
    public PolutionData polutionData;
    public NatureData natureData;
    public PopulationData populationData;
    public EnergyData energyData;

    public int buildingWasteManagementLevel;
    public string baseModelName;
    public float airPollution;
    public float waterPollution;
    public float soilPollution;
    public string KendaraanName;

    private bool isTouching = false;
    private Vector3 touchStartPosition;
    private Vector3 touchEndPosition;
    public float timerTouch = 0.5f;
    public float lastTouch;

    public LayerMask layerToHit;
    float maxDistance = 30;

    private int wasteManagementStartId;

    public void InitializeBuilding(int id, GameObject objectBuilding, Vector3Int grid, GridData gridData, PlacementSystem placementsystem, SwitchingLevels switchingLevels)
    {
        buildingID = id;
        structure = objectBuilding;
        gridPosition = grid;
        buildingData = gridData;
        placementSystem = placementsystem;
        switchingLevel = switchingLevels;
    }
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                float extendDistance = 10.0f;
                Vector3 extendedPoint = ray.GetPoint(extendDistance);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, maxDistance, layerToHit))
                {
                    float distanceToHit = Vector3.Distance(ray.origin, hit.point);
                    //Debug.Log("Hit at a distance of " + distanceToHit + " units");
                    //Debug.Log(hit.collider.gameObject == gameObject);
                    //Debug.Log(hit.collider.gameObject);
                    //Debug.Log(gameObject);
                    if (hit.collider.gameObject == gameObject)
                    {
                        
                        Debug.Log("nah masuk sini 3");
                        touchStartPosition = touch.position;
                        Debug.Log("print");
                        isTouching = true;
                    }

                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (isTouching)
                {
                    touchEndPosition = touch.position;
                    float touchDistance = Vector3.Distance(touchStartPosition, touchEndPosition);
                    float touchThreshold = 40f; // Adjust the threshold as needed
                    Debug.Log("distance " + touchDistance + " " + touchThreshold);
                    isTouching = false;
                    if (touchDistance < touchThreshold)
                    {
                        HandleTouchAsButton();
                    }
                }
            }
        }
    }
    //private void OnMouseUpAsButton()
    //{
    //    if (Input.mousePresent)
    //    {
    //        HandleTouchAsButton();
    //    }

    //}
    private void HandleTouchAsButton()
    {
        if (!OnClickBuilding.activeSelf)
        { 
            Debug.Log("masuk ke handle");
            Debug.Log(switchingLevel);
            OnClickBuilding.SetActive(true);
            ShowBuildingActions();

            wasteManagementStartId = database.wasteManagements.FindIndex(data => data.Type.Equals(database.objectsData[buildingID].Type));

            for (int i = 0; i < 3; i++)
            {
                ShowWasteManagementActions(wasteManagementStartId + i, i + 1);
            }

            objSize = database.objectsData[buildingID].Size;
            Button deleteButton = OnClickBuilding.transform.Find("BuildingActions").Find("DeleteBuildingStruct").GetComponent<Button>();
            Button confirmDeleteButton = placementSystem.DeleteMessage.transform.Find("DeleteBuilding").Find("ButtonDelete").GetComponent<Button>();
            Button confirmDeleteButtonMarket = placementSystem.DeleteMessage.transform.Find("DeleteBuildingMarket").Find("ButtonDelete").GetComponent<Button>();
            Button moveButton = OnClickBuilding.transform.Find("BuildingActions").Find("Move").GetComponent<Button>();
            Button upgradeButton = OnClickBuilding.transform.Find("BuildingActions").Find("UpgradeBuildingStruct").GetComponent<Button>();
            Button upgradeWM1Button = OnClickBuilding.transform.Find("WasteManagementActions/WM1/UpgradeWasteManagement").GetComponent<Button>();
            Button upgradeWM2Button = OnClickBuilding.transform.Find("WasteManagementActions/WM2/UpgradeWasteManagement").GetComponent<Button>();
            Button upgradeWM3Button = OnClickBuilding.transform.Find("WasteManagementActions/WM3/UpgradeWasteManagement").GetComponent<Button>();

            //deleteButton.onClick.AddListener(() => DeleteBuilding(structure));
            deleteButton.onClick.AddListener(() => OpenDeleteMessage());
            confirmDeleteButton.onClick.AddListener(() => DeleteBuilding(structure));
            confirmDeleteButtonMarket.onClick.AddListener(() => DeleteBuilding(structure));
            Button wasteManagementButton = OnClickBuilding.transform.Find("WasteManagementButton").GetComponent<Button>();
            Button buildingButton = OnClickBuilding.transform.Find("BuildingButton").GetComponent<Button>();

            moveButton.onClick.AddListener(() => MoveBuilding(structure));
            upgradeButton.onClick.AddListener(() => UpgradeBuilding(structure, buildingID + 4));
            upgradeWM1Button.onClick.AddListener(() => UpgradeWasteManagement(structure, 1));
            upgradeWM2Button.onClick.AddListener(() => UpgradeWasteManagement(structure, 2));
            upgradeWM3Button.onClick.AddListener(() => UpgradeWasteManagement(structure, 3));
            wasteManagementButton.onClick.AddListener(() => WasteManagementButtonClicked());
            buildingButton.onClick.AddListener(() => BuildingButtonClicked());
        }
    }

    public void UpgradeWasteManagement(GameObject structure, int wmLevel)
    {
        int currentWMLevel = 0;
        int nameLength = BuildingName.text.Length;
        
        if (BuildingName.text.Contains("WM"))
            currentWMLevel = int.Parse(BuildingName.text[nameLength - 1].ToString());

        int id = wmLevel - currentWMLevel + buildingID;

        if (currencyCount.CheckCurrency(database.objectsData[id].CurrencyNeed) == true)
        {
            buildingWasteManagementLevel += 1;
            currencyCount.SubtractCurrency(database.objectsData[id].CurrencyNeed);
            StartCoroutine(ConstructingTimeUpgrade(structure, id));
            OnClickBuilding.SetActive(false);
        }
        else
        {
            GameObject.Find("CanvasShop&BuildingClick&ManiHall/OnClickBuilding/BuildError").SetActive(true);
            StartCoroutine(DisableObjectsAfterDelay());
        }
    }

    public void OpenDeleteMessage()
    {
        if (database.objectsData[buildingID].Category == "Production")
        {
            placementSystem.deleteMessageMarket.SetActive(true);
            OnClickBuilding.SetActive(false);
        }
        else
        {
            placementSystem.deleteMessageBuilding.SetActive(true);
            OnClickBuilding.SetActive(false);
        }
    }

    public void CloseDeleteMessage()
    {
        if (database.objectsData[buildingID].Category == "Production")
        {
            placementSystem.deleteMessageMarket.SetActive(false);
        }else
        {
            placementSystem.deleteMessageBuilding.SetActive(false);
        }
    }

    IEnumerator DisableObjectsAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        GameObject.Find("CanvasShop&BuildingClick&ManiHall/OnClickBuilding/BuildError").SetActive(false);
    }

    public void DeleteBuilding(GameObject structure)
    {
        GridData selectedData = buildingData;
        Debug.Log("ini delete " + selectedData + gridPosition + objSize);
        selectedData.DeleteObjectAt(gridPosition, objSize);
        //FirestoreManager.DeleteBuilding(structure.name);
        CloseDeleteMessage();
        Destroy(structure);
        OnClickBuilding.SetActive(false);
    }


    public void MoveBuilding(GameObject structure)
    {
        DeleteBuilding(structure);
        placementSystem.StartPlacement(buildingID);
        //delete building
        //create lagi placementSystem.StartPlacement(id);
    }

    public void UpgradeBuilding(GameObject structure, int id)
    {
        //if (CheckBuildValidity(id) == true && database.objectsData[id-1].IsLevelMax == false)
        if (CheckBuildValidity(id) ==  true)
        {
            airPollution = database.objectsData[id].AirPolution;
            waterPollution = database.objectsData[id].WaterPolution;
            soilPollution = database.objectsData[id].SoilPolution;
            baseModelName = database.objectsData[id].Name;

            DataUpdateConstructing(id);
            
            StartCoroutine(ConstructingTimeUpgrade(structure, id));
            OnClickBuilding.SetActive(false);
        }
    }
    public void CloseBuildingDetail()
    {
        //GameObject.Find("CanvasShop&BuildingClick&ManiHall/OnClickBuilding/LevelError").SetActive(false);
        GameObject.Find("CanvasShop&BuildingClick&ManiHall/OnClickBuilding/WasteManagementActions").SetActive(false);
        OnClickBuilding.SetActive(false);
    }

    private void DataUpdateConstructing(int selectedObjectIndex)
    {
        currencyCount.SubtractCurrency(database.objectsData[selectedObjectIndex].CurrencyNeed);
        populationData.increaseUsedPopulation(database.objectsData[selectedObjectIndex].PopulationNeed);
        if (database.objectsData[selectedObjectIndex].Category != "Energy" || database.objectsData[selectedObjectIndex].Category != "Nature")
        {
            int food = database.objectsData[selectedObjectIndex].Food - database.objectsData[selectedObjectIndex - 1].Food;
            int mineral = database.objectsData[selectedObjectIndex].Mineral - database.objectsData[selectedObjectIndex - 1].Mineral;
            int electricity = database.objectsData[selectedObjectIndex].Electrictiy - database.objectsData[selectedObjectIndex - 1].Electrictiy;
            energyData.IncreaseUsedEnergy(food, mineral, electricity);
        }
    }

    private bool CheckBuildValidity(int selectedObjectIndex)
    {
        switchingLevel = FindObjectOfType<SwitchingLevels>();
        Debug.Log(switchingLevel.current_level);
        bool checkCurrency = currencyCount.CheckCurrency(database.objectsData[selectedObjectIndex].CurrencyNeed);
        bool checkPopulation = populationData.CheckUsedPopulation(database.objectsData[selectedObjectIndex].PopulationNeed);
        bool checkMainHall = (switchingLevel.current_level >= database.objectsData[selectedObjectIndex].LevelMainHall) ? true : false;
        bool checkEnergy = true;
        if (database.objectsData[selectedObjectIndex].Category != "Energy" && database.objectsData[selectedObjectIndex].Category != "Nature")
        {
            int food = database.objectsData[selectedObjectIndex].Food - database.objectsData[selectedObjectIndex-1].Food;
            int mineral = database.objectsData[selectedObjectIndex].Mineral - database.objectsData[selectedObjectIndex-1].Mineral;
            int electricity = database.objectsData[selectedObjectIndex].Electrictiy - database.objectsData[selectedObjectIndex-1].Electrictiy;
            checkEnergy = energyData.CheckUsedEnergy(food, mineral, electricity);
            Debug.Log("test" + checkEnergy);
        }
        return checkPopulation && checkCurrency && checkEnergy && checkMainHall;
    }

    IEnumerator ConstructingTimeUpgrade(GameObject structure, int ID)
    {
        

        //Kendaraan
        if (database.objectsData[ID].Name.Contains("Transportation Factory"))
        {
            if (KendaraanName == "Cart")
            {
                placementSystem.KendaraanCart -= 1;
            }
            else if (KendaraanName == "Truck")
            {
                placementSystem.KendaraanTruck -= 1;
            }
            else if (KendaraanName == "Train")
            {
                placementSystem.KendaraanTrain -= 1;
            }
            else if (KendaraanName == "Ship")
            {
                placementSystem.KendaraanShip -= 1;
            }
            placementSystem.SelectKendaraan.SetActive(true);
            placementSystem.buttonSelectKendaraan.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (placementSystem.isSelectedCart.activeSelf)
                {
                    KendaraanName = "Cart";
                    placementSystem.KendaraanCart += 1;

                }
                else if (placementSystem.isSelectedShip.activeSelf)
                {
                    KendaraanName = "Ship";
                    placementSystem.KendaraanShip += 1;
                }
                else if (placementSystem.isSelectedTrain.activeSelf)
                {
                    KendaraanName = "Train";
                    placementSystem.KendaraanTrain += 1;
                }
                else if (placementSystem.isSelectedTruck.activeSelf)
                {
                   KendaraanName = "Truck";
                    placementSystem.KendaraanTruck += 1;
                }
                else
                {
                    KendaraanName = "Cart";
                    placementSystem.KendaraanCart += 1;
                }
                placementSystem.SelectKendaraan.SetActive(false);
            });

        }

        // Create Building Construction
        float constructingTime = database.objectsData[ID].BuildingTime;
        // Create Building Text Time
        GameObject constructionDetail = Instantiate(constructionTemplate);
        constructionDetail.transform.parent = GameObject.Find(structure.name).transform;
        Vector3 constructionPosition = structure.transform.position;
        constructionPosition.y = 1.0f;
        constructionDetail.transform.position = constructionPosition;
        constructionDetail.GetComponent<TextMeshPro>().SetText("Construction Time: " + constructingTime);
        Building addedBuilding = new Building
        {
            PositionX = structure.transform.position.x,
            PositionY = structure.transform.position.y,
            PositionZ = structure.transform.position.z,
            ModelName = database.objectsData[ID].Name,
            Id = structure.name,
            Category = database.objectsData[ID].Category,
            WasteManagementLevel = buildingWasteManagementLevel,
            TimeStamp = DateTime.Now,
            isBuilded = false
        };
        FirestoreManager.SaveBuilding(addedBuilding);
        while (constructingTime > 0.0f)
        {
            Debug.Log(constructingTime);
            yield return new WaitForSeconds(1.0f);
            Debug.Log("time = " + constructingTime);
            constructingTime -= 1.0f;
            constructionDetail.GetComponent<TextMeshPro>().SetText("Construction Time: " + constructingTime);
            Debug.Log("time = " + constructingTime);
        }
        Debug.Log("check Point 3");
        Destroy(constructionDetail);
        Destroy(structure);
        //Destroy(newObject);
        DataUpdateConstructionCompleted(ID);
        Debug.Log("check Point 4");

        GameObject newObject = Instantiate(database.objectsData[ID].Prefeb);
        newObject.transform.position = structure.transform.position;
        newObject.name = structure.name; // ini nanti nama object nya
        newObject.transform.tag = structure.transform.tag;
        newObject.transform.parent = structure.transform.parent;
        newObject.layer = LayerMask.NameToLayer("Building");

        //GameObject newPrefab = Instantiate(database.objectsData[ID].Prefeb);
        //newPrefab.transform.position = newObject.transform.position;
        //newPrefab.transform.name = newObject.name;
        //newPrefab.transform.tag = newObject.transform.tag;
        //newPrefab.transform.parent = newObject.transform.parent;
        //newObject = newPrefab;
        Debug.Log("check Point 5");
        BuildingOnClick scriptComponent = newObject.AddComponent<BuildingOnClick>();
        int buildingID = database.objectsData[ID].ID;
        scriptComponent.InitializeBuilding(buildingID, newObject, gridPosition, buildingData, placementSystem, switchingLevel);
        scriptComponent.constructionTemplate = constructionTemplate;
        scriptComponent.OnClickBuilding = OnClickBuilding;
        scriptComponent.BuildingName = BuildingName;
        scriptComponent.database = database;
        scriptComponent.currencyCount = currencyCount;
        scriptComponent.polutionData = polutionData;
        scriptComponent.natureData = natureData;
        scriptComponent.populationData = populationData;
        scriptComponent.energyData = energyData;
        scriptComponent.baseModelName = baseModelName;
        scriptComponent.airPollution = airPollution;
        scriptComponent.waterPollution = waterPollution;
        scriptComponent.soilPollution = soilPollution;
        scriptComponent.buildingWasteManagementLevel = buildingWasteManagementLevel;
        scriptComponent.layerToHit = LayerMask.GetMask("Building");
        scriptComponent.KendaraanName = KendaraanName; 
        BoxCollider boxCollider = newObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.4f, 0.2f, 0.4f);
        FirestoreManager.UpdateBuildingStatus(structure.name, true);
        //newObj = GameObject.Find(newObject.name);
        //Debug.Log(newObj.name);
        
    }
    private void DataUpdateConstructionCompleted(int ID)
    {
        if (ID <= 31)
        {
            print(database.objectsData[ID].Currency);
            currencyCount.AddCurrency(database.objectsData[ID].Currency);
            polutionData.AddAirPolution(database.objectsData[ID].AirPolution);
            polutionData.AddWaterPolution(database.objectsData[ID].WaterPolution);
            polutionData.AddSoilPolution(database.objectsData[ID].SoilPolution);
            if (database.objectsData[ID].Category == "Energy")
            {
                energyData.IncreaseMaxEnergy(database.objectsData[ID].EnergyProduction - database.objectsData[ID - 1].EnergyProduction, database.objectsData[ID].Type);
            }
            populationData.decreaseUsedPopulation(database.objectsData[ID].PopulationNeed);
        } else
        {
            Debug.Log("ini bangun " + database.objectsData[ID].Name);
            if (buildingWasteManagementLevel > 1) {
                RevertToDefaultPollution(ID - 1);
            }

            Debug.Log("dikurangin " + airPollution * database.objectsData[ID].AirPolution);
            polutionData.DecreaseAirPolution(airPollution * database.objectsData[ID].AirPolution);
            polutionData.DecreaseWaterPolution(waterPollution * database.objectsData[ID].WaterPolution);
            polutionData.DecreaseSoilPolution(soilPollution * database.objectsData[ID].SoilPolution);

            if (database.objectsData[ID].Name.Contains("Farm WM 3"))
            {
                energyData.IncreaseMaxEnergy(2, "Electricity");
            }
        }
    }

    private void RevertToDefaultPollution(int ID)
    {
        if (database.objectsData[ID].AirPolution > 0)
        {
            polutionData.AddAirPolution(airPollution * database.objectsData[ID].AirPolution);
        }

        if (database.objectsData[ID].WaterPolution > 0)
        {
            polutionData.AddWaterPolution(waterPollution * database.objectsData[ID].WaterPolution);
        }

        if (database.objectsData[ID].SoilPolution > 0)
        {
            polutionData.AddSoilPolution(soilPollution * database.objectsData[ID].SoilPolution);
        }
    }

    private void SetCurrentProduce()
    {
        Vector3[] positions = new[] {
            new Vector3(140, 30, 0), new Vector3(200, -30, 0)
        };

        int currPos = 0;

        if (database.objectsData[buildingID].EnergyProduction > 0)
        {
            GameObject.Find("CurrentBuilding/Produce/Material").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("CurrentBuilding/Produce/Material/MaterialAmount").GetComponent<Text>().text = "+" + database.objectsData[buildingID].EnergyProduction.ToString();
            GameObject.Find("CurrentBuilding/Produce/Material").SetActive(true);

            GameObject.Find("UpgradedBuilding/Produce/Material").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("UpgradedBuilding/Produce/Material/MaterialAmount").GetComponent<Text>().text = "+" + database.objectsData[buildingID + 4].EnergyProduction.ToString();
            GameObject.Find("UpgradedBuilding/Produce/Material").SetActive(true);

            currPos++;
        }

        if(database.objectsData[buildingID].Currency > 0)
        {
            GameObject.Find("CurrentBuilding/Produce/Currency").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("CurrentBuilding/Produce/Currency/CurrencyAmount").GetComponent<Text>().text = "+" + database.objectsData[buildingID].Currency.ToString();
            GameObject.Find("CurrentBuilding/Produce/Currency").SetActive(true);

            GameObject.Find("UpgradedBuilding/Produce/Currency").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("UpgradedBuilding/Produce/Currency/CurrencyAmount").GetComponent<Text>().text = "+" + database.objectsData[buildingID + 4].Currency.ToString();
            GameObject.Find("UpgradedBuilding/Produce/Currency").SetActive(true);

            currPos++;
        }

        if (currPos == 0)
            currPos = 1;

        GameObject.Find("CurrentBuilding/Produce/ProduceBG").GetComponent<RectTransform>().sizeDelta = new Vector2(200 * currPos, 60);
        GameObject.Find("UpgradedBuilding/Produce/ProduceBG").GetComponent<RectTransform>().sizeDelta = new Vector2(200 * currPos, 60);
    }

    private void SetCurrentDamage()
    {
        Vector3[] positions = new[] {
            new Vector3(80, 30, 0), new Vector3(300, 30, 0), new Vector3(500, 30, 0)
        };

        int currPos = 0;

        if (database.objectsData[buildingID].SoilPolution > 0)
        {
            GameObject.Find("CurrentBuilding/Damage/SoilDamage").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("CurrentBuilding/Damage/SoilDamage/SoilAmount").GetComponent<Text>().text = "+" + database.objectsData[buildingID].SoilPolution.ToString();
            GameObject.Find("CurrentBuilding/Damage/SoilDamage").SetActive(true);

            GameObject.Find("UpgradedBuilding/Damage/SoilDamage").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("UpgradedBuilding/Damage/SoilDamage/SoilAmount").GetComponent<Text>().text = "+" + database.objectsData[buildingID + 4].SoilPolution.ToString();
            GameObject.Find("UpgradedBuilding/Damage/SoilDamage").SetActive(true);

            currPos++;
        }

        if (database.objectsData[buildingID].WaterPolution > 0)
        {
            GameObject.Find("CurrentBuilding/Damage/WaterDamage").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("CurrentBuilding/Damage/WaterDamage/WaterAmount").GetComponent<Text>().text = "+" + database.objectsData[buildingID].WaterPolution.ToString();
            GameObject.Find("CurrentBuilding/Damage/WaterDamage").SetActive(true);

            GameObject.Find("UpgradedBuilding/Damage/WaterDamage").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("UpgradedBuilding/Damage/WaterDamage/WaterAmount").GetComponent<Text>().text = "+" + database.objectsData[buildingID + 4].WaterPolution.ToString();
            GameObject.Find("UpgradedBuilding/Damage/WaterDamage").SetActive(true);

            currPos++;
        }

        if (database.objectsData[buildingID].AirPolution > 0)
        {
            GameObject.Find("CurrentBuilding/Damage/AirDamage").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("CurrentBuilding/Damage/AirDamage/AirAmount").GetComponent<Text>().text = "+" + database.objectsData[buildingID].AirPolution.ToString();
            GameObject.Find("CurrentBuilding/Damage/AirDamage").SetActive(true);

            GameObject.Find("UpgradedBuilding/Damage/AirDamage").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("UpgradedBuilding/Damage/AirDamage/AirAmount").GetComponent<Text>().text = "+" + database.objectsData[buildingID + 4].AirPolution.ToString();
            GameObject.Find("UpgradedBuilding/Damage/AirDamage").SetActive(true);

            currPos++;
        }

        if (currPos == 0)
            currPos = 1;

        GameObject.Find("CurrentBuilding/Damage/DamageBG").GetComponent<RectTransform>().sizeDelta = new Vector2(250 * currPos, 60);
        GameObject.Find("UpgradedBuilding/Damage/DamageBG").GetComponent<RectTransform>().sizeDelta = new Vector2(250 * currPos, 60);
    }

    private void SetNeeds()
    {
        Vector3[] positions = new[] {
            new Vector3(110, -50, 0), new Vector3(240, -50, 0), new Vector3(360, -50, 0)
        };

        int currPos = 0;

        GameObject.Find("Needs/Currency/CurrencyAmount").GetComponent<Text>().text = database.objectsData[buildingID + 4].CurrencyNeed.ToString();
        GameObject.Find("Needs/Population/PopulationAmount").GetComponent<Text>().text = database.objectsData[buildingID + 4].PopulationNeed.ToString();

        if (database.objectsData[buildingID + 4].Mineral > 0)
        {
            GameObject.Find("OnClickBuilding/BuildingActions/Needs/Mineral").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("OnClickBuilding/BuildingActions/Needs/Mineral/MineralAmount").GetComponent<Text>().text = database.objectsData[buildingID + 4].Mineral.ToString();
            GameObject.Find("OnClickBuilding/BuildingActions/Needs/Mineral").SetActive(true);

            currPos++;
        }

        if (database.objectsData[buildingID + 4].Food > 0)
        {
            GameObject.Find("OnClickBuilding/BuildingActions/Needs/Food").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("OnClickBuilding/BuildingActions/Needs/Food/FoodAmount").GetComponent<Text>().text = database.objectsData[buildingID + 4].Food.ToString();
            GameObject.Find("OnClickBuilding/BuildingActions/Needs/Food").SetActive(true);

            currPos++;
        }

        if (database.objectsData[buildingID + 4].Electrictiy > 0)
        {
            GameObject.Find("OnClickBuilding/BuildingActions/Needs/Electricity").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            GameObject.Find("OnClickBuilding/BuildingActions/Needs/Electricity/ElectricityAmount").GetComponent<Text>().text = database.objectsData[buildingID + 4].Electrictiy.ToString();
            GameObject.Find("OnClickBuilding/BuildingActions/Needs/Electricity").SetActive(true);
        }
    }

    private void ShowBuildingActions()
    {
        GameObject.Find("BuildingImage").GetComponent<Image>().sprite = database.objectsData[buildingID].ImageBuilding;
        GameObject.Find("DeskripsiBuilding").GetComponent<Text>().text = database.objectsData[buildingID].BuildingDescription;
        if (database.objectsData[buildingID].Name.Contains("Transportation Factory"))
        {
            placementSystem.Kendaraancurrent.SetActive(true);
            placementSystem.KendaraanNextLevel.SetActive(true);
            GameObject.Find("KendaraanImageBuildingOnClick").GetComponent<Image>().sprite = placementSystem.GetSprite(KendaraanName);
            if (database.objectsData[buildingID + 4].Name.Contains("LV") && database.objectsData[buildingID + 4].Name.Substring(database.objectsData[buildingID + 4].Name.IndexOf("LV") + 2).Contains("2"))
            {
                placementSystem.KendaraanLV2.SetActive(true);
                placementSystem.KendaraanLV3.SetActive(false);
            }
            else if (database.objectsData[buildingID + 4].Name.Contains("LV") && database.objectsData[buildingID + 4].Name.Substring(database.objectsData[buildingID + 4].Name.IndexOf("LV") + 2).Contains("3"))
            {
                placementSystem.KendaraanLV3.SetActive(true);
                placementSystem.KendaraanLV2.SetActive(false);
            }
        }
        else
        {
            placementSystem.Kendaraancurrent.SetActive(false);
            placementSystem.KendaraanNextLevel.SetActive(false);
        }
        SetBuildingName();
        SetCurrentProduce();
        SetCurrentDamage();
        SetNeeds();

        SetMaterialSprite(database.objectsData[buildingID].Type);
    }
    

    private void SetBuildingName()
    {
        String currentBuildingName = database.objectsData[buildingID].Name;

        BuildingName.text = currentBuildingName.Substring(0, currentBuildingName.IndexOf("LV") - 1);

        GameObject.Find("CurrentBuilding/NamaBuilding/LevelBuilding").GetComponent<Text>().text = currentBuildingName.Substring(currentBuildingName.IndexOf("LV"), 3);

        String upgradedBuildingName = database.objectsData[buildingID + 4].Name;
        GameObject.Find("UpgradedBuilding/NamaBuilding").GetComponent<Text>().text = upgradedBuildingName.Substring(0, upgradedBuildingName.IndexOf("LV") - 1);
        GameObject.Find("UpgradedBuilding/NamaBuilding/LevelBuilding").GetComponent<Text>().text = upgradedBuildingName.Substring(upgradedBuildingName.IndexOf("LV"), 3);
    }

    private void SetMaterialSprite(String type)
    {
        if (type.Equals("Farm") || type.Equals("Electricity") || type.Equals("Minerals"))
        {
            GameObject.Find("CurrentBuilding/Produce/Material").GetComponent<Image>().sprite = Resources.Load<Sprite>(type);
            GameObject.Find("UpgradedBuilding/Produce/Material").GetComponent<Image>().sprite = Resources.Load<Sprite>(type);
        }
        else
        {
            GameObject.Find("CurrentBuilding/Produce/Material").SetActive(false);
            GameObject.Find("UpgradedBuilding/Produce/Material").SetActive(false);
        }
    }

    public void WasteManagementButtonClicked()
    {
        GameObject.Find("CanvasShop&BuildingClick&ManiHall/OnClickBuilding/WasteManagementActions").SetActive(true);

        int levelRequired;

        if (buildingID % 4 != 0)
        {
            Debug.Log("ini wm");
            levelRequired = database.objectsData[buildingID].LevelMainHall;
        }
        else
            levelRequired = database.objectsData[buildingID + 1].LevelMainHall;

        Debug.Log("level req " + levelRequired);
        Debug.Log("level " + switchingLevel.current_level);

        if (switchingLevel.current_level < levelRequired)
        {
            GameObject.Find("CanvasShop&BuildingClick&ManiHall/OnClickBuilding/LevelError/LevelErrorMessage").GetComponent<Text>().text = "You Need to Upgrade Your Main Hall to LV " + levelRequired;
            GameObject.Find("CanvasShop&BuildingClick&ManiHall/OnClickBuilding/LevelError").SetActive(true);
        }

    }

    public void BuildingButtonClicked()
    {
        GameObject.Find("CanvasShop&BuildingClick&ManiHall/OnClickBuilding/WasteManagementActions").SetActive(false);
        GameObject.Find("CanvasShop&BuildingClick&ManiHall/OnClickBuilding/LevelError").SetActive(false);
    }

    private void ShowWasteManagementActions(int id, int count)
    {
        Transform wm = GameObject.Find("CanvasShop&BuildingClick&ManiHall/OnClickBuilding/WasteManagementActions/WM" + count).transform;
        Debug.Log("idnya " + id);
        wm.Find("WasteManagementImage").GetComponent<Image>().sprite = database.wasteManagements[id].Image;
        wm.Find("WasteManagementName").GetComponent<Text>().text = database.wasteManagements[id].Name;
        wm.Find("WasteManagementDescription").GetComponent<Text>().text = database.wasteManagements[id].Description;

        SetPollutionReduction(wm, id);

        wm.Find("Currency").Find("AvailableCurrency").GetComponent<Text>().text = currencyCount.currency.ToString();
        wm.Find("Currency").Find("AvailableCurrency").Find("CurrencyAmount").GetComponent<Text>().text = database.wasteManagements[id].CurrencyNeeds.ToString();
    }

    private void SetPollutionReduction(Transform wm, int id)
    {
        Debug.Log("reduction");
        Vector3[] positions = new[] {
            new Vector3(-77, -84, 0), new Vector3(109, -84, 0), new Vector3(-77, -154, 0)
        };

        int currPos = 0;

        if (!database.wasteManagements[id].SoilPollution.Equals(""))
        {
            wm.Find("SoilPollution").Find("PollutionAmount").GetComponent<Text>().text = database.wasteManagements[id].SoilPollution;
            wm.Find("SoilPollution").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            wm.Find("SoilPollution").gameObject.SetActive(true);
            currPos++;
        }

        if (!database.wasteManagements[id].AirPollution.Equals(""))
        {
            wm.Find("AirPollution").Find("PollutionAmount").GetComponent<Text>().text = database.wasteManagements[id].AirPollution;
            wm.Find("AirPollution").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            wm.Find("AirPollution").gameObject.SetActive(true);
            currPos++;
        }

        if (!database.wasteManagements[id].WaterPollution.Equals(""))
        {
            wm.Find("WaterPollution").Find("PollutionAmount").GetComponent<Text>().text = database.wasteManagements[id].WaterPollution;
            wm.Find("WaterPollution").GetComponent<RectTransform>().anchoredPosition3D = positions[currPos];
            wm.Find("WaterPollution").gameObject.SetActive(true);
        }
    }


}


// yg diubah
// wasteManagementButtonClicked -> buat LevelError/LevelErrorMessage
