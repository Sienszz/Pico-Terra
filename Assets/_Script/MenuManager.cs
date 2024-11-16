using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject shopPanel;
    public PerspectivePan cameraController;
    public GameObject OnClickBuilding;
    public GameObject PollutionDetail;
    public GameObject BuildOrCancel;
    public void OpenShopPanel()
    {
        
        shopPanel.SetActive(true);
    }
    public void closeShopPanel()
    {
        
        shopPanel.SetActive(false);
    }
    public void Update()
    {
        if (BuildOrCancel.activeSelf == true)
        {
            OnClickBuilding.SetActive(false);
        }
        if (shopPanel.activeSelf == true)
        {
            OnClickBuilding.SetActive(false);
            PollutionDetail.SetActive(false);
        }

        if (OnClickBuilding.activeSelf == true)
        {
            PollutionDetail.SetActive(false);
        }
        if (PollutionDetail.activeSelf == true)
        {
            OnClickBuilding.SetActive(false);
        }
        if(shopPanel.activeSelf == true || OnClickBuilding.activeSelf == true)
        {
            cameraController.shopIsOpen = true;
        }
        else
        {
            cameraController.shopIsOpen = false;
        }
    }
}
