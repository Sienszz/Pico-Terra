
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_Shop : MonoBehaviour
{
    private Transform ScrollArea;
    private Transform itemContainerShop;
    private Transform itemTemplate;
    private Transform itemDescription;
    private Transform itemLevel;
    private Transform buildingDesc;
    private Transform buildingImageDetail;
    private Transform BuyButton;
    private Transform detailsContainer;
    private Transform currencyProduce;
    private Transform EnergyProduceIcon;
    private Transform energyProduce;
    private Transform polusiTanahSakit;
    private Transform polusiAirSakit;
    private Transform polusiUdaraSakit;
    private Transform currencyNeed;
    private Transform populationNeed;
    private Transform energy1Need;
    private Transform energy2Need;
    private Transform energy3Need;
    private Transform energy4Need;
    private Transform levelBuildingNeed;

    public GameObject energyButton;
    public GameObject productionButton;
    public GameObject distributorButton;
    public GameObject othersButton;
    public GameObject energyButtonDisable;
    public GameObject productionButtonDisable;
    public GameObject distributorButtonDisable;
    public GameObject othersButtonDisable;
    public GameObject detailNotSelected;
    public GameObject LevelError;
    public GameObject isSelected;
    public Sprite[] SpriteArray;

    public ObjectsDatabaseSO database;
    public PlacementSystem placementSystem;
    public GameObject shopPanel;
    public SwitchingLevels mainHall;
    public Text Category;

    public currencyCount currentCurrency;
    public PopulationData currentPopulation;
    public int currency;
    public int population;

    public int mainHallLevel;

    private void Awake()
    {
        ScrollArea = transform.Find("ScrollArea");
        itemContainerShop = GameObject.Find("ItemContainerShop").transform;
        itemTemplate = itemContainerShop.Find("ItemTemplate");
        itemTemplate.gameObject.SetActive(false);
        detailsContainer = transform.Find("DetailsContainer");
        itemDescription = detailsContainer.Find("ItemDescription");
        itemLevel = detailsContainer.Find("ItemDescriptionLVL");
        buildingDesc = detailsContainer.Find("BuildingDesc");
        buildingImageDetail = detailsContainer.Find("BuildingImageDetail");
        BuyButton = detailsContainer.Find("BuyButton");
        currencyProduce = GameObject.Find("CurrencyProduce").transform;
        energyProduce = GameObject.Find("EnergyProduce").transform;
        EnergyProduceIcon = GameObject.Find("EnergyProduceIcon").transform;
        polusiTanahSakit = GameObject.Find("PolusiTanahSakit").transform;
        polusiAirSakit = GameObject.Find("PolusiAirSakit").transform;
        polusiUdaraSakit = GameObject.Find("PolusiUdaraSakit").transform;
        currencyNeed = GameObject.Find("CurrencyNeed").transform;
        populationNeed = GameObject.Find("PopulationNeed").transform;
        energy1Need = GameObject.Find("Energy1Need").transform;
        energy2Need = GameObject.Find("Energy2Need").transform;
        energy3Need = GameObject.Find("Energy3Need").transform;
        energy4Need = GameObject.Find("Energy4").transform;
        levelBuildingNeed = GameObject.Find("LevelGaCukupText").transform;
    }

    private void Start()
    {
        mainHallLevel = mainHall.checkLevel();
        showCategory("Energy");
        energyButton.SetActive(true);
        energyButtonDisable.SetActive(false);
        productionButton.SetActive(false);
        productionButtonDisable.SetActive(true);
        distributorButton.SetActive(false);
        distributorButtonDisable.SetActive(true);
        othersButton.SetActive(false);
        othersButtonDisable.SetActive(true);
        energyButtonDisable.GetComponent<Button>().onClick.AddListener(() =>
        {

            ChangeCategory(1);
            energyButton.SetActive(true);
            energyButtonDisable.SetActive(false);
            productionButton.SetActive(false);
            productionButtonDisable.SetActive(true);
            distributorButton.SetActive(false);
            distributorButtonDisable.SetActive(true);
            othersButton.SetActive(false);
            othersButtonDisable.SetActive(true);
        });

        productionButtonDisable.GetComponent<Button>().onClick.AddListener(() =>
        {
            ChangeCategory(2);
            energyButton.SetActive(false);
            energyButtonDisable.SetActive(true);
            productionButton.SetActive(true);
            productionButtonDisable.SetActive(false);
            distributorButton.SetActive(false);
            distributorButtonDisable.SetActive(true);
            othersButton.SetActive(false);
            othersButtonDisable.SetActive(true);
        });

        distributorButtonDisable.GetComponent<Button>().onClick.AddListener(() =>
        {
            ChangeCategory(3);
            energyButton.SetActive(false);
            energyButtonDisable.SetActive(true);
            productionButton.SetActive(false);
            productionButtonDisable.SetActive(true);
            distributorButton.SetActive(true);
            distributorButtonDisable.SetActive(false);
            othersButton.SetActive(false);
            othersButtonDisable.SetActive(true);
        });

        othersButtonDisable.GetComponent<Button>().onClick.AddListener(() =>
        {
            ChangeCategory(4);
            energyButton.SetActive(false);
            energyButtonDisable.SetActive(true);
            productionButton.SetActive(false);
            productionButtonDisable.SetActive(true);
            distributorButton.SetActive(false);
            distributorButtonDisable.SetActive(true);
            othersButton.SetActive(true);
            othersButtonDisable.SetActive(false);
        });

    }

   
  

    private void showCategory(string categoryName)
    {
        var itemsInCategory = database.objectsData.Where(data => data.Category == categoryName).ToList();
        foreach (var itemData in itemsInCategory)
        {
            Category.text = itemData.Category.ToUpper();
            
            if (itemData.Category != "Nature")
            {
                if (itemData.Name.Contains("LV1") && !itemData.Name.Contains("WM"))
                {
                    CreateItemButton(itemData.Name, itemData.ImageBuilding, itemData.BuildingDescription, itemData.Currency, itemData.Type, itemData.EnergyProduction, itemData.SoilPolution, itemData.WaterPolution, itemData.AirPolution, itemData.CurrencyNeed, itemData.PopulationNeed, itemData.Food, itemData.Electrictiy, itemData.Mineral, itemData.LevelMainHall, itemData.KendaraanNeed);
                }
            }
            else if (itemData.Category == "Nature")
            {
                CreateItemButton(itemData.Name, itemData.ImageBuilding, itemData.BuildingDescription, itemData.Currency, itemData.Type,itemData.EnergyProduction, itemData.SoilPolution, itemData.WaterPolution, itemData.AirPolution, itemData.CurrencyNeed, itemData.PopulationNeed, itemData.Food, itemData.Electrictiy, itemData.Mineral, itemData.LevelMainHall, itemData.KendaraanNeed);
            }
            
        }
    }
    private void CreateItemButton(string itemName, Sprite itemImage, string itemDesc, int itemCurrencyProduce, string itemType, int itemEnergy, float damageTanah, float damageAir, float damageUdara, int uangNeed, int populasiNeed, int food, int electric, int minerals, int buildingLevel, string KendaraanNeed)
    {
        mainHallLevel = mainHall.checkLevel();
        Transform itemTransform = Instantiate(itemTemplate, itemContainerShop);
        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
        int id = database.objectsData.FindIndex(data => data.Name == itemName);

        itemTransform.Find("ItemName").GetComponent<TextMeshProUGUI>().SetText(itemName);
        itemTransform.Find("BuildingImage").GetComponent<Image>().sprite = itemImage;
        itemTransform.GetComponent<Button>().onClick.RemoveAllListeners();
        itemTransform.GetComponent<Button>().onClick.AddListener(() =>
        {
            currency = currentCurrency.GetCurrency;
            population = currentPopulation.currPopulation;
            //isSelected.SetActive(false) ;
            detailNotSelected.SetActive(false);
            itemDescription.GetComponent<TextMeshProUGUI>().SetText(GetTextBeforeLV(itemName).ToUpper());
            itemLevel.GetComponent<TextMeshProUGUI>().SetText(GetTextAfterLV(itemName));
            buildingDesc.GetComponent<TextMeshProUGUI>().SetText(itemDesc);
            buildingImageDetail.GetComponent<Image>().sprite = itemImage;
            currencyProduce.GetComponent<TextMeshProUGUI>().SetText("+" + itemCurrencyProduce.ToString());
            if(itemEnergy!=0)
            {
                energyProduce.GetComponent<TextMeshProUGUI>().SetText("+" + itemEnergy);
            }
            else
            {
                energyProduce.GetComponent<TextMeshProUGUI>().SetText("");
            }

            EnergyProduceIcon.GetComponent<Image>().sprite = GetSprite(itemType);
            polusiTanahSakit.GetComponent<TextMeshProUGUI>().SetText("+"+ damageTanah.ToString("F1"));
            polusiAirSakit.GetComponent<TextMeshProUGUI>().SetText("+" + damageAir.ToString("F1"));
            polusiUdaraSakit.GetComponent<TextMeshProUGUI>().SetText("+" + damageUdara.ToString("F1"));
            currencyNeed.GetComponent<TextMeshProUGUI>().color = currency < uangNeed ? new Color(0.7f, 0.26f, 0.24f, 1f) : Color.black;
            currencyNeed.GetComponent<TextMeshProUGUI>().SetText(uangNeed.ToString());
            populationNeed.GetComponent<TextMeshProUGUI>().color = population < populasiNeed ? new Color(0.7f, 0.26f, 0.24f, 1f) : Color.black;
            populationNeed.GetComponent<TextMeshProUGUI>().SetText(populasiNeed.ToString());
            energy1Need.GetComponent<TextMeshProUGUI>().SetText(food.ToString());
            energy2Need.GetComponent<TextMeshProUGUI>().SetText(electric.ToString());
            energy3Need.GetComponent<TextMeshProUGUI>().SetText(minerals.ToString());
            energy4Need.GetComponent<Image>().sprite = GetSprite(KendaraanNeed);
            levelBuildingNeed.GetComponent<TextMeshProUGUI>().SetText("");
            if (mainHallLevel >= database.objectsData[id].LevelMainHall)
            {
                LevelError.SetActive(false);
                BuyButton.GetComponent<Button>().interactable = true;
                BuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
                BuyButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    placementSystem.StartPlacement(id);
                    shopPanel.SetActive(false);
                });
            }
            else
            {
                LevelError.SetActive(true);
                levelBuildingNeed.GetComponent<TextMeshProUGUI>().SetText("You Need to Upgrade Your Main Hall to LV" + buildingLevel.ToString());
                BuyButton.GetComponent<Button>().interactable = false;
                
            }


        });
        itemTransform.gameObject.SetActive(true);
    }
    public Sprite GetSprite(string spriteName)
    {
        if(spriteName == "Farm" || spriteName == "Market")
        {
            return SpriteArray[0];
        }
        else if(spriteName == "Electricity")
        {
            return SpriteArray[1];
        }
        else if (spriteName == "Minerals")
        {
            return SpriteArray[2];
        }
        else if (spriteName == "House")
        {
            return SpriteArray[4];
        }
        else if (spriteName == "Cart")
        {
            return SpriteArray[5];
        }
        else if (spriteName == "Truck")
        {
            return SpriteArray[6];
        }
        else if (spriteName == "Train")
        {
            return SpriteArray[7];
        }
        else if (spriteName == "Ship")
        {
            return SpriteArray[8];
        }
        else if(spriteName == "Garage")
        {
            return SpriteArray[9];
        }
        else
        {
            return SpriteArray[3];
        }
        
    }
    public string GetTextBeforeLV(string input)
    {
        int lvIndex = input.IndexOf("LV");
        if (lvIndex != -1)
        {
            return input.Substring(0, lvIndex).Trim(); 
        }
        return input;
    }

    public string GetTextAfterLV(string input)
    {
        int lvIndex = input.IndexOf("LV");
        if (lvIndex != -1)
        {
            int startIndex = lvIndex + 2; 
            return "LV" + input.Substring(startIndex).Trim(); 
        }
        return "";
    }
    private void ChangeCategory(int index)
    {
        deleteObject();
        switch (index)
        {
            default:
            case 1:
                showCategory("Energy");
                return;
            case 2:
                showCategory("Production");
                return;
            case 3:
                showCategory("Distributor");
                return;
            case 4:
                showCategory("Nature");
                return;
        }
    }

    private void deleteObject()
    {
        Transform[] children = itemContainerShop.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.name.Contains("ItemTemplate"))
            {
                Destroy(child.gameObject); 
            }
        }
    }



}
