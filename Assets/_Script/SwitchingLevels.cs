using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchingLevels : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    public PlacementSystem placementSystem;
    public GameObject[] levels;
    public int[] currencyNeeds;
    public currencyCount currencyCount;
    public GridVisualizationGenerator gridVisu;
    public Text level;
    public GameObject MainHallPanel;
    public GameObject ErrorUpgrade;
    public GameObject backgroundDark;

    public int current_level = 1;
    int flagLevel = 0;

    void Start()
    {
        level.text = "LV " + current_level;
    }
    public void Upgrade()
    {
        if (current_level < levels.Length)
        {
            if(CheckBuildValidity(flagLevel + 1))
            {
                current_level++;
                flagLevel++;
                currencyCount.SubtractCurrency(currencyNeeds[flagLevel]);
                MainHallPanel.SetActive(false);
                if (current_level == 3 || current_level == 6 || current_level == 9)
                {
                    SwitchObject(flagLevel);
                }
                gridVisu.GenerateTiles(current_level);
                level.text = "Level" + current_level;
                if (current_level == 8)
                {
                    editCameraView();
                }
            }
            else
            {
                MainHallPanel.SetActive(false);
                backgroundDark.SetActive(true);
                ErrorUpgrade.SetActive(true);
                StartCoroutine(DisableObjectsAfterDelay());
            }
            
        }
    }

    public void SwitchObject(int lvl)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (i == lvl)
                levels[i].SetActive(true);
            else
                levels[i].SetActive(false);
        }
    }

    private bool CheckBuildValidity(int selectedObjectIndex)
    {
        bool checkCurrency = currencyCount.CheckCurrency(currencyNeeds[selectedObjectIndex]);
        bool checkRecuirement = false;

        if (current_level == 1)
        {
            checkRecuirement = checkBuilding("LandGrass") && checkBuilding("Land Rock") && checkBuilding("Forest") && checkBuilding("EcengGondok") && checkBuilding("Small Farm");
        }
        else if (current_level == 2)
        {
            checkRecuirement = checkBuilding("Traditional Mine") && checkBuilding("Coal Mine");
        }
        else if (current_level == 3)
        {
            checkRecuirement = checkBuilding("Small Garage") && (checkBuilding("Small Farm WM 1") || checkBuilding("Small Farm WM 2") || checkBuilding("Small Farm WM 3")) && checkBuilding("Semi Modern Mine");
        }
        else if (current_level == 4)
        {
            checkRecuirement = checkBuilding("Traditional Market") &&
                               (checkBuilding("Traditional Mine WM 1") || checkBuilding("Traditional Mine WM 2") || checkBuilding("Traditional Mine WM 3") || checkBuilding("Semi Modern Mine WM 1") || checkBuilding("Semi Modern Mine WM 2") || checkBuilding("Semi Modern Mine WM 3")) &&
                               (checkBuilding("Medium Farm") || checkBuilding("Medium Farm WM 1") || checkBuilding("Medium Farm WM 2") || checkBuilding("Medium Farm WM 3"));
        }
        else if (current_level == 5)
        {
            checkRecuirement = checkBuilding("Traditional Factory") &&
                               (checkBuilding("Coal Mine WM 1") || checkBuilding("Coal Mine WM 2") || checkBuilding("Coal Mine WM 3")) &&
                               checkBuilding("Thermal Power Plant");
        }
        else if (current_level == 6)
        {
            checkRecuirement = checkBuilding("Terminal") &&
                               (checkBuilding("Small Garage WM 1") || checkBuilding("Small Garage WM 2") || checkBuilding("Small Garage WM 3")) &&
                               checkBuilding("Modern Mine") &&
                               checkBuilding("Medium Garage");
        }
        else if (current_level == 7)
        {
            checkRecuirement = checkBuilding("Small Resto") &&
                               (checkBuilding("Traditional Market WM 1") || checkBuilding("Traditional Market WM 2") || checkBuilding("Traditional Market WM 3")) &&
                               checkBuilding("Large Farm") &&
                               checkBuilding("Modern Market");
        }
        else if (current_level == 8)
        {
            checkRecuirement = checkBuilding("Small Shop") &&
                               (checkBuilding("Traditional Factory WM1") || checkBuilding("Traditional Factory WM2") || checkBuilding("Traditional Factory WM3")) &&
                               checkBuilding("Modern Thermal Power Plant") &&
                               checkBuilding("Modern Factory");
        }
        else if (current_level == 9)
        {
            checkRecuirement = checkBuilding("Large Garage") && checkBuilding("Super Market") &&
                               checkBuilding("Super Factory") && checkBuilding("Terminal And Station") &&
                               checkBuilding("Medium Resto") && checkBuilding("Medium Shop");
        }
        else if (current_level == 10)
        {
            checkRecuirement = (checkBuilding("Terminal WM 1") || checkBuilding("Terminal WM 2") || checkBuilding("Terminal WM 3") || checkBuilding("Terminal And Station WM 1") || checkBuilding("Terminal And Station WM 2") || checkBuilding("Terminal And Station WM 3")) &&
                               (checkBuilding("Small Resto WM 1") || checkBuilding("Small Resto WM 2") || checkBuilding("Small Resto WM 3") || checkBuilding("Medium Resto WM 1") || checkBuilding("Medium Resto WM 2") || checkBuilding("Medium Resto WM 3")) &&
                               (checkBuilding("Small Shop WM 1") || checkBuilding("Small Shop WM 2") || checkBuilding("Small Shop WM 3") || checkBuilding("Medium Shop WM 1") || checkBuilding("Medium Shop WM 2") || checkBuilding("Medium Shop WM 3")) &&
                               checkBuilding("Station Center") && checkBuilding("Large Resto") && checkBuilding("large Shop");
        }
        else if (current_level == 11)
        {
            checkRecuirement = checkBuilding("Renewable Power Plant");
        }
        return checkCurrency && checkRecuirement;
        
    }

    public int checkLevel()
    {
        return current_level;
    }
    public void OpenMainHallPanel()
    {

        MainHallPanel.SetActive(true);
    }
    public void closeMainHallPanel()
    {

        MainHallPanel.SetActive(false);
    }

    public bool checkBuilding(string buildingName)
    {
        GridData buildingData = placementSystem.BuildingData;
        bool check = buildingData.IsBuildingABuilt(buildingName);
        return check;
    }

    private void editCameraView()
    {
        mainCamera.transform.position = new Vector3(1, 2.52f, 4.5f);
        mainCamera.transform.rotation = Quaternion.Euler(42.532f, 188.84f, 2.427f);
        Camera.main.fieldOfView = 55;
    }

    IEnumerator DisableObjectsAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        backgroundDark.SetActive(false);
        ErrorUpgrade.SetActive(false);
    }

}

