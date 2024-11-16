using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PollutionDetail : MonoBehaviour
{
    public GameObject PanelPollutionDetail;
    public GameObject BGDark;
    public Text AverageText;
    public PolutionData pollutionData;
    public ObjectsDatabaseSO database;
    private Transform listItem;
    private Transform item;
    public Sprite[] spritePollution;
    public Slider PollutionBar;
    private void Start()
    {
        PanelPollutionDetail.SetActive(false);
        BGDark.SetActive(false);
    }
    private void Awake()
    {
        listItem = GameObject.Find("ListItemDetailPollution").transform;
        item = listItem.Find("ItemDetailPollution");
        item.gameObject.SetActive(false);

    }
        public void CloseDetail()
    {
        PanelPollutionDetail.SetActive(false);
        BGDark.SetActive(false);
    }
    public void DetailPolution(string polutionName)
    {
        listItem.DetachChildren();
        PanelPollutionDetail.SetActive(true);
        BGDark.SetActive(true);
        float polution = 0;
        if (polutionName == "Air")
        {
            polution = pollutionData.airPolution;
            GameObject.Find("ImagePollutionDetail").GetComponent<Image>().sprite = spritePollution[0];
            PollutionBar.value = pollutionData.airPolution;
        }
        else if (polutionName == "Water")
        {
            polution = pollutionData.waterPolution;
            GameObject.Find("ImagePollutionDetail").GetComponent<Image>().sprite = spritePollution[2];
            PollutionBar.value = pollutionData.waterPolution;
        }
        else
        {
            polution = pollutionData.soilPolution;
            GameObject.Find("ImagePollutionDetail").GetComponent<Image>().sprite = spritePollution[1];
            PollutionBar.value = pollutionData.soilPolution;
        }
        checkItem(polutionName);
        AverageText.text = "+ " + polution.ToString("F1");
        
    }
    private void deleteObject()
    {
        Transform[] children = listItem.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.name.Contains("ItemDetailPollution"))
            {
                Destroy(child.gameObject);
            }
            else
            {
                break;
            }
        }
    }
    public void checkItem(string pollutionName)
    {

        Transform structureContainer = GameObject.Find("StructureContainer").transform;
        Transform energy = structureContainer.Find("Energy").transform;
        Transform Production = structureContainer.Find("Production").transform;
        Transform Distributor = structureContainer.Find("Distributor").transform;
        structureContainerFind(energy, pollutionName);
        structureContainerFind(Production, pollutionName);
        structureContainerFind(Distributor, pollutionName);
    }
    public void structureContainerFind(Transform Category, string pollutionName)
    {
        if (Category != null)
        {
            foreach (Transform child in Category)
            {
                string childName = GetContainerName(child.name);
                int id = database.objectsData.FindIndex(data => data.Type == childName);
                if (pollutionName == "Air")
                {
                    if (database.objectsData[id].AirPolution != 0)
                    {
                        CreateDetail(database.objectsData[id].ImageBuilding, database.objectsData[id].AirPolution, child.childCount);
                    }
                }
                else if (pollutionName == "Soil")
                {
                    if (database.objectsData[id].SoilPolution != 0)
                    {
                        CreateDetail(database.objectsData[id].ImageBuilding, database.objectsData[id].SoilPolution, child.childCount);
                    }
                }
                else
                {
                    if (database.objectsData[id].WaterPolution != 0)
                    {
                        CreateDetail(database.objectsData[id].ImageBuilding, database.objectsData[id].WaterPolution, child.childCount);
                    }
                }

                //print("Nama GameObject di Production: " + childName);
            }
        }
    }
    public void CreateDetail(Sprite ImageBuilding, float Pollution, int Count)
    {
        float total = (float)(Pollution * Count);
        Transform itemTransform = Instantiate(item,listItem);
        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
        itemTransform.Find("BuildingImagePollutionDetail").GetComponent<Image>().sprite = ImageBuilding;
        itemTransform.Find("DetailPollutionPerBuilding").GetComponent<TextMeshProUGUI>().SetText("+" + total.ToString("F2"));
        itemTransform.Find("CountBuildingPollutionDetail").GetComponent<Text>().text = "X" + Count.ToString();
        itemTransform.gameObject.SetActive(true);
    }
    public string GetContainerName(string Name)
    {
        int containerIndex = Name.IndexOf("Container");
        if (containerIndex >= 0)
        {
            return Name.Substring(0, containerIndex);
        }
        return Name;
    }

}
