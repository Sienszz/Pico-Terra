using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainhallRequirement : MonoBehaviour
{
    public ObjectsDatabaseSO database;
    public Text buildingName;
    public SwitchingLevels mainhallData;
    private int currentLevel;

    public void Start()
    {
        currentLevel = mainhallData.current_level;
        GameObject card = transform.GetChild (0).gameObject;
        GameObject flag;
        var itemsLevel = database.objectsData.Where(data => data.LevelMainHall == currentLevel).ToList();

        foreach (var itemData in itemsLevel)
        {
            if (itemData.Name.Contains("WM"))
            {
                if (itemData.Name.Contains("WM 1"))
                {
                    flag = Instantiate(card, transform);
                    flag.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(itemData.Name.Replace("1", ""));
                    flag.transform.GetChild(1).GetComponent<Image>().sprite = itemData.ImageBuilding;
                }
            }else
            {
                flag = Instantiate(card, transform);
                flag.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(itemData.Name);
                flag.transform.GetChild(1).GetComponent<Image>().sprite = itemData.ImageBuilding;
            }
        }
        card.gameObject.SetActive(false);
    }

    public void updateRequirement()
    {
        deleteObject();

        GameObject card = transform.GetChild(0).gameObject;
        GameObject flag;
        card.gameObject.SetActive(true);
        currentLevel = mainhallData.current_level;
        var itemsLevel = database.objectsData.Where(data => data.LevelMainHall == currentLevel).ToList();
        int checkWM = -1;
        
        foreach (var itemData in itemsLevel)
        {
            if (itemData.Name.Contains("WM"))
            {
                if (mainhallData.checkBuilding(itemData.Name))
                {
                    checkWM = 1;
                }
                if (itemData.Name.Contains("WM 1"))
                {
                    flag = Instantiate(card, transform);
                    flag.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(itemData.Name.Replace("1", ""));
                    flag.transform.GetChild(1).GetComponent<Image>().sprite = itemData.ImageBuilding;
                    if (checkWM == 1)
                    {
                        flag.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        flag.transform.GetChild(2).gameObject.SetActive(true);
                        checkWM = 0;
                    }
                }
            }
            else
            {
                flag = Instantiate(card, transform);
                flag.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(itemData.Name);
                flag.transform.GetChild(1).GetComponent<Image>().sprite = itemData.ImageBuilding;
                if (mainhallData.checkBuilding(itemData.Name))
                {
                    flag.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    flag.transform.GetChild(2).gameObject.SetActive(true);
                }
                
            }
        }
        card.gameObject.SetActive(false);
    }

    private void deleteObject()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.name.Contains("ItemTemplate"))
            {
                Destroy(child.gameObject);
            }
        }
    }


}
