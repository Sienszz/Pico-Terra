using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlacementSystem : MonoBehaviour
{
    public GameObject constructionTemplate;
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private InputManager inputManager;
    public Grid grid;
    [SerializeField] private ObjectsDatabaseSO database;
    
    private int selectedObjectIndex = -1;
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private PreviewSystem preview;
    private GridData buildingData ;
    public GridData BuildingData
    {
        get { return buildingData; }
    }

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private List<GameObject> placedGameObjects = new();

    private Transform container;
    private MainHallGenerator mainHallGenerator;
    [SerializeField]
    private BuildingOnClick buildingOnClick;
    [SerializeField]
    private GameObject onClickBuilding;
    [SerializeField]
    private Text buildingName;


    public currencyCount currencyCount;
    public PolutionData polutionData;
    public NatureData natureData;
    public PopulationData populationData;
    public EnergyData energyData;


    public GameObject ErrorResource;
    public GameObject backgroundDark;
    public SwitchingLevels switchingLevels;
    public GameObject ErrorMaksimalHexagon;
    public GameObject ErrorKendaraanDipakai;
    

    public Transform buildOrCancel;
    public Transform confirmButton;
    public Transform cancelButton;


    public GameObject SelectKendaraan;
    public GameObject isSelectedCart;
    public GameObject isSelectedTruck;
    public GameObject isSelectedTrain;
    public GameObject isSelectedShip;
    public Button buttonSelectKendaraan;
    public string kendaraanNameSelected;
    public Sprite[] SpriteKendaraan;

    public GameObject KendaraanLV2;
    public GameObject KendaraanLV3;
    public GameObject Kendaraancurrent;
    public GameObject KendaraanNextLevel;

    public GameObject deleteMessageBuilding;
    public GameObject deleteMessageMarket;
    public GameObject DeleteMessage;
    public int KendaraanCart = 0;
    public int KendaraanTruck = 0;
    public int KendaraanTrain = 0;
    public int KendaraanShip = 0;


    private void Start()
    {
        buildOrCancel = GameObject.Find("BuildOrCancel").transform;
        confirmButton = buildOrCancel.Find("ConfirmButton");
        cancelButton = buildOrCancel.Find("CancelButton");
        StopPlacement();
            mainHallGenerator = FindObjectOfType<MainHallGenerator>();
        buildingData = new();
        confirmButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            PlaceStructure();
        });
        cancelButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            StopPlacement();
        });
    }
    private void Awake()
    {

    }
    public void StartPlacement(int ID)
    {
        Debug.Log(ID);
        Debug.Log("masuk sini berapa kali");
        StopPlacement();
        
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if(selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        
        if (CheckBuildValidity(ID) && !checkBuildingCount())
        {
            buildOrCancel.gameObject.SetActive(true);
            gridVisualization.SetActive(true);
            preview.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefeb,
                database.objectsData[selectedObjectIndex].Size);
            mouseIndicator.SetActive(true);
            {
                if(selectedObjectIndex > 0)
                {
                    Debug.Log(selectedObjectIndex);
                    inputManager.OnClicked += UpdatePosition;
                    Debug.Log("place structure");
                    inputManager.OnExit += StopPlacement;
                }
                
            }
            
        }
        else
        {
            backgroundDark.SetActive(true);
            ErrorResource.SetActive(true);
            ErrorMaksimalHexagon.SetActive(false);
            StartCoroutine(DisableObjectsAfterDelay());
        }
        if(checkBuildingCount())
        {
            backgroundDark.SetActive(true);
            ErrorMaksimalHexagon.SetActive(true);
            ErrorResource.SetActive(false);
            StartCoroutine(DisableObjectsAfterDelay());
        }
    }
    private bool checkBuildingCount()
    {
        int maksimalBuildingLevel = maksimalBuildingMainHallLevel(switchingLevels.current_level);
        GridData selectedData = buildingData;
        if (selectedData.CountBuildingAlreadyPlaced() == maksimalBuildingLevel)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private int maksimalBuildingMainHallLevel(int level)
    {
        if(level == 1){return 10;}
        else if(level == 2){return 20;}
        else if (level == 3) { return 24; }
        else if (level == 4) { return 30; }
        else if (level == 5) { return 34; }
        else if (level == 6) { return 50; }
        else if (level == 7) { return 56; }
        else if (level == 8) { return 74; }
        else if (level == 9) { return 82; }
        else if (level == 10) { return 112; }
        else
        {
            return 143;
        }
    }

    IEnumerator DisableObjectsAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        backgroundDark.SetActive(false);
        ErrorResource.SetActive(false);
        ErrorMaksimalHexagon.SetActive(false);
        ErrorKendaraanDipakai.SetActive(false);
    }

    public void selectKendaraan(string namaKendaraan)
    {

        if(namaKendaraan == "Cart")
        {
            isSelectedCart.SetActive(true);
            isSelectedShip.SetActive(false);
            isSelectedTrain.SetActive(false);
            isSelectedTruck.SetActive(false);
        }
        else if(namaKendaraan == "Truck")
        {
            isSelectedCart.SetActive(false);
            isSelectedShip.SetActive(false);
            isSelectedTrain.SetActive(false);
            isSelectedTruck.SetActive(true);
        }
        else if(namaKendaraan == "Train")
        {
            isSelectedCart.SetActive(false);
            isSelectedShip.SetActive(false);
            isSelectedTrain.SetActive(true);
            isSelectedTruck.SetActive(false);
        }
        else if(namaKendaraan == "Ship")
        {
            isSelectedCart.SetActive(false);
            isSelectedShip.SetActive(true);
            isSelectedTrain.SetActive(false);
            isSelectedTruck.SetActive(false);
        }
    }
    public void closePilihanKendaraan()
    {
        SelectKendaraan.SetActive(false);
    }
    private void PlaceStructure()
    {
        Debug.Log("placing");
        Vector3 mouseWorldPos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = lastDetectedPosition;

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        bool buyBuildingValidity = CheckBuildValidity(database.objectsData[selectedObjectIndex].ID);
        if (placementValidity == false || buyBuildingValidity == false)
        {
            return;
        }
        DataUpdateConstructing(database.objectsData[selectedObjectIndex].ID);
        var position = grid.CellToWorld(gridPosition);
        preview.UpdatePosition(position, false);

        String category = database.objectsData[selectedObjectIndex].Category;

        GridData selectedData = buildingData;
        selectedData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            placedGameObjects.Count - 1, database.objectsData[selectedObjectIndex].Name);
        StartCoroutine(ConstructingTime(gridPosition, database.objectsData[selectedObjectIndex].ID, position, category));
        
        Destroy(selectedData);
        StopPlacement();  
    }
   
    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = buildingData;
        GridData selectedDataMainHall = mainHallGenerator.BuildingData;
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size) && selectedDataMainHall.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private bool CheckBuildValidity(int ID)
    {
        bool checkPopulation = populationData.CheckUsedPopulation(database.objectsData[ID].PopulationNeed);
        bool checkCurrency = currencyCount.CheckCurrency(database.objectsData[ID].CurrencyNeed);
        bool checkEnergy = true;
        bool checkKendaraan = true;
        if (database.objectsData[ID].Category != "Energy" && database.objectsData[ID].Category != "Nature")
        {
            int food = database.objectsData[ID].Food;
            int mineral = database.objectsData[ID].Mineral;
            int electricity = database.objectsData[ID].Electrictiy;
            checkEnergy = energyData.CheckUsedEnergy(food, mineral, electricity);
        }
        if(database.objectsData[ID].KendaraanNeed == "Cart")
        {
            if(KendaraanCart > 0)
            {
                checkKendaraan = true;
            }
            else if(KendaraanCart<=0)
            {
                checkKendaraan = false;
            }

        }
        else if(database.objectsData[ID].KendaraanNeed == "Truck")
        {
            if (KendaraanTruck > 0)
            {
                checkKendaraan = true;
            }
            else if (KendaraanTruck <= 0)
            {
                checkKendaraan = false;
            }
        }
        else if (database.objectsData[ID].KendaraanNeed == "Train")
        {
            if (KendaraanTrain > 0)
            {
                checkKendaraan = true;
            }
            else if (KendaraanTrain <= 0)
            {
                checkKendaraan = false;
            }
        }
        else if (database.objectsData[ID].KendaraanNeed == "Ship")
        {
            if (KendaraanShip > 0)
            {
                checkKendaraan = true;
            }
            else if (KendaraanShip <= 0)
            {
                checkKendaraan = false;
            }
        }

        //Debug.Log(database.objectsData[ID].CurrencyNeed);
        //Debug.Log("check Popluation" + checkPopulation + " " + database.objectsData[ID] + " " + database.objectsData[ID].PopulationNeed);
        //Debug.Log("check Currency" + checkCurrency);
        //Debug.Log("check Energy" + checkEnergy);
        return checkPopulation && checkCurrency && checkEnergy && checkKendaraan;
    }

    public void StopPlacement()
    {
        buildOrCancel.gameObject.SetActive(false);
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        preview.StopShowingPreview();
        inputManager.OnClicked -= UpdatePosition;
        inputManager.OnExit -= StopPlacement;
        mouseIndicator.SetActive(false);
        lastDetectedPosition = Vector3Int.zero;
    }
    private void DataUpdateConstructing(int ID)
    {
        currencyCount.SubtractCurrency(database.objectsData[ID].CurrencyNeed);
        populationData.increaseUsedPopulation(database.objectsData[ID].PopulationNeed);
        if(database.objectsData[selectedObjectIndex].Category != "Energy" || database.objectsData[ID].Category != "Nature")
        {
            int food = database.objectsData[ID].Food;
            int mineral = database.objectsData[ID].Mineral;
            int electricity = database.objectsData[ID].Electrictiy;
            energyData.IncreaseUsedEnergy(food, mineral, electricity);
        }
    }

    IEnumerator ConstructingTime(Vector3Int gridPosition, int ID, Vector3 position, String category)
    {
        //Kendaraan
        if (database.objectsData[ID].Name.Contains("Transportation Factory"))
        {
            SelectKendaraan.SetActive(true);
            buttonSelectKendaraan.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (isSelectedCart.activeSelf)
                {
                    kendaraanNameSelected = "Cart";
                    KendaraanCart += 1;
                }
                else if (isSelectedShip.activeSelf)
                {
                    kendaraanNameSelected = "Ship";
                    KendaraanShip += 1;
                }
                else if (isSelectedTrain.activeSelf)
                {
                    kendaraanNameSelected = "Train";
                    KendaraanTrain += 1;
                }
                else if (isSelectedTruck.activeSelf)
                {
                    kendaraanNameSelected = "Truck";
                    KendaraanTruck += 1;
                }
                else
                {
                    kendaraanNameSelected = "Cart";
                    KendaraanCart += 1;
                }
                SelectKendaraan.SetActive(false);
            });

        }

        // Create Building Construction
        float constructingTime = database.objectsData[ID].BuildingTime;
        GameObject newObject = Instantiate(database.constructionObject[0].Prefeb);
        Vector3 pos = grid.CellToWorld(gridPosition);
        newObject.transform.position = new Vector3(pos.x, 0.11f, pos.z);
        newObject.transform.rotation = Quaternion.Euler(0, 60, 0);
        newObject.name = Guid.NewGuid().ToString(); // ini nanti nama object nya
        newObject.transform.tag = StructureBuilder.GetStructureTag(category);
        newObject.transform.parent = StructureBuilder.GetSubCategoryContainer(category, database.objectsData[ID].Type).transform;


        GameObject constructionDetail = Instantiate(constructionTemplate);
        //constructionTemplate
        constructionDetail.transform.parent = GameObject.Find(newObject.name).transform;
        Vector3 constructionPosition = newObject.transform.position;
        constructionPosition.y = 1.0f;
        constructionDetail.transform.position = constructionPosition;
        constructionDetail.GetComponent<TextMeshPro>().SetText("" + constructingTime);
        Building addedBuilding = new Building
        {
            PositionX = position.x,
            PositionY = position.y,
            PositionZ = position.z,
            ModelName = database.objectsData[ID].Name,
            Id = newObject.name,
            Category = category,
            WasteManagementLevel = 0,
            TimeStamp = DateTime.Now,
            isBuilded = false,
            KendaraanName = kendaraanNameSelected

        };

        FirestoreManager.SaveBuilding(addedBuilding);
        while (constructingTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            constructingTime -= 1.0f;
            constructionDetail.GetComponent<TextMeshPro>().SetText("" + constructingTime);
        }
        Destroy(constructionDetail);
        Destroy(newObject);
        DataUpdateConstructionCompleted(ID);

        PlacementSystem placementSystem = GetComponent<PlacementSystem>();

        GameObject newPrefab = Instantiate(database.objectsData[ID].Prefeb);
        newPrefab.transform.position = grid.CellToWorld(gridPosition);
        newPrefab.transform.rotation = Quaternion.Euler(0, 60, 0);
        newPrefab.transform.name = newObject.name;
        newPrefab.transform.tag = newObject.transform.tag;
        newPrefab.transform.parent = newObject.transform.parent;
        newObject = newPrefab;
        newObject.layer = LayerMask.NameToLayer("Building");
        BuildingOnClick scriptComponent = newObject.AddComponent<BuildingOnClick>();
        int buildingID = database.objectsData[ID].ID;
        scriptComponent.InitializeBuilding(buildingID, newObject,gridPosition,buildingData, placementSystem, switchingLevels);
        scriptComponent.constructionTemplate = constructionTemplate;
        scriptComponent.OnClickBuilding = onClickBuilding;
        scriptComponent.BuildingName = buildingName;
        scriptComponent.database = database;
        scriptComponent.currencyCount = currencyCount;
        scriptComponent.polutionData = polutionData;
        scriptComponent.natureData = natureData;
        scriptComponent.populationData = populationData;
        scriptComponent.energyData = energyData;
        scriptComponent.buildingWasteManagementLevel = 0;
        scriptComponent.baseModelName = database.objectsData[ID].Name;
        scriptComponent.airPollution = database.objectsData[ID].AirPolution;
        scriptComponent.waterPollution = database.objectsData[ID].WaterPolution;
        scriptComponent.soilPollution = database.objectsData[ID].SoilPolution;
        scriptComponent.layerToHit = LayerMask.GetMask("Building");
        scriptComponent.KendaraanName = kendaraanNameSelected;
        BoxCollider boxCollider = newObject.AddComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.4f, 0.2f, 0.4f);
        FirestoreManager.UpdateBuildingStatus(newObject.name, true);
        //newObj = GameObject.Find(newObject.name);
        //Debug.Log(newObj.name);
        
    }

    private void DataUpdateConstructionCompleted(int ID)
    {
        if (ID <= 3)
        {
            natureData.AddAirNature(database.objectsData[ID].AirPolution);
            natureData.AddSoilNature(database.objectsData[ID].SoilPolution);
            natureData.AddWaterNature(database.objectsData[ID].WaterPolution);
        }
        else if(ID == 113)
        {
            populationData.increaseHousePopulation(database.objectsData[ID].EnergyProduction);
        }
        else
        {
            currencyCount.AddCurrency(database.objectsData[ID].Currency);
            polutionData.AddAirPolution(database.objectsData[ID].AirPolution);
            polutionData.AddWaterPolution(database.objectsData[ID].WaterPolution);
            polutionData.AddSoilPolution(database.objectsData[ID].SoilPolution);
            if(database.objectsData[ID].Category == "Energy")
            {
                energyData.IncreaseMaxEnergy(database.objectsData[ID].EnergyProduction, database.objectsData[ID].Type);
            }else if(database.objectsData[ID].Type == "Market")
            {
                populationData.increaseMaxPopulation(database.objectsData[ID].EnergyProduction);
            }
        }
        populationData.decreaseUsedPopulation(database.objectsData[ID].PopulationNeed);
        
    }
    private void UpdatePosition()
    {
        Debug.Log("masuk update position");
        Vector3 mouseWorldPos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mouseWorldPos);
        if (lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
            Debug.Log(placementValidity);
            if(placementValidity == true)
            {
                mouseIndicator.transform.position = mouseWorldPos;
                preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
                Debug.Log("test");

                lastDetectedPosition = gridPosition;
            }
            
        }
    }
    public Sprite GetSprite(string spriteName)
    {
        if (spriteName == "Cart")
        {
            return SpriteKendaraan[0];
        }
        else if (spriteName == "Truck")
        {
            return SpriteKendaraan[1];
        }
        else if (spriteName == "Train")
        {
            return SpriteKendaraan[2];
        }
        else if (spriteName == "Ship")
        {
            return SpriteKendaraan[3];
        }
        else
        {
            return SpriteKendaraan[4];
        }

    }
    private void Update()
    {
       
            if (selectedObjectIndex < 0)
            return;
        
    }
}