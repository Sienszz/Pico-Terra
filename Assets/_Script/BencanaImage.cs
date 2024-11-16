using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using System;

public class BencanaImage : MonoBehaviour
{
    public Image m_Image;
    //public GameObject sacrificeButton;
    //public GameObject karmaButton;
    //public GameObject rejectButton;
    public Sprite[] m_SpriteArray;
    public float initialSpeed = 0.04f; 
    public float normalSpeed = 0.02f; 
    private int m_IndexSprite;
    private Coroutine m_CoroutineAnim;

    //public void Func_PlayUIAnim()
    //{
    //    m_Image.gameObject.SetActive(true);
    //    StartCoroutine(Func_PlayAnimUI());
    //}

    //IEnumerator Func_PlayAnimUI()
    //{
    //    float speed = initialSpeed; 

    //    while (m_IndexSprite < m_SpriteArray.Length)
    //    {
    //        yield return new WaitForSeconds(speed);
    //        m_Image.sprite = m_SpriteArray[m_IndexSprite];
    //        m_IndexSprite += 1;

    //        if (m_IndexSprite > 0)
    //        {

    //            speed = normalSpeed;
    //        }
    //    }

    //    m_Image.gameObject.SetActive(false);
    //}



    public void ShowWarningCanvas(DisasterUIData data)
    {
        GameObject.Find("/UI/CanvasBencana/Warning/ErrorMessage").GetComponent<Text>().text = data.warningMessage;
        GameObject.Find("/UI/CanvasBencana/Warning").SetActive(true);

        StartCoroutine(SwitchImagesCoroutine(data));
        GameObject.Find("/UI/CanvasBencana/Warning").GetComponent<Button>().onClick.AddListener(() => ShowDisasterImpact(data));
    }

    public void ShowDisasterImpact(DisasterUIData data)
    {
        GameObject.Find("/UI/CanvasBencana/Warning").SetActive(false);

        GameObject.Find("/UI/CanvasBencana/Impact").SetActive(true);
        GameObject.Find("/UI/CanvasBencana/Impact/ImpactDetail").GetComponent<Text>().text = data.impactMessage;

        GameObject.Find("/UI/CanvasBencana/Impact").GetComponent<Button>().onClick.AddListener(() => ShowDisasterDialog1(data));
    }

    public void ShowDisasterDialog1(DisasterUIData data)
    {
        GameObject.Find("/UI/CanvasBencana/Impact").SetActive(false);

        GameObject.Find("/UI/CanvasBencana/Dialog").SetActive(true);

        switch (data.spiritName)
        {
            case "Water Spirit":
                GameObject.Find("/UI/CanvasBencana/Dialog/MainBG").GetComponent<Image>().sprite = Resources.Load<Sprite>("Disaster/WaterDisasterBG");
                break;
            case "Air Spirit":
                GameObject.Find("/UI/CanvasBencana/Dialog/MainBG").GetComponent<Image>().sprite = Resources.Load<Sprite>("Disaster/AirDisasterBG");
                break;
            default:
                GameObject.Find("/UI/CanvasBencana/Dialog/MainBG").GetComponent<Image>().sprite = Resources.Load<Sprite>("Disaster/LandDisasterBG");
                break;
        }

        if (data.dialog == 1)
            GameObject.Find("/UI/CanvasBencana/Dialog/DialogText").GetComponent<Text>().text = "I'm saddened that you've made the "+ data.spiritName + " sick";
        else
            GameObject.Find("/UI/CanvasBencana/Dialog/DialogText").GetComponent<Text>().text = "I'm extremely DISAPPOINTED that the " + data.spiritName + " is in bad shape because of what you did";

        GameObject.Find("/UI/CanvasBencana/Dialog").GetComponent<Button>().onClick.AddListener(() => ShowDisasterDialog2(data));
    }

    public void ShowDisasterDialog2(DisasterUIData data)
    {
        if (data.dialog == 1)
            GameObject.Find("/UI/CanvasBencana/Dialog/DialogText").GetComponent<Text>().text = "As an act of kindness, I'll provide you with several options to fix this";
        else
            GameObject.Find("/UI/CanvasBencana/Dialog/DialogText").GetComponent<Text>().text = "But now, the crucial thing is to make the " + data.spiritName + " healthy again for the city's sake";

        GameObject.Find("/UI/CanvasBencana/Dialog").GetComponent<Button>().onClick.AddListener(() => ShowDisasterDialog3(data));
    }

    public void ShowDisasterDialog3(DisasterUIData data)
    {
        if (data.dialog == 1)
            GameObject.Find("/UI/CanvasBencana/Dialog/DialogText").GetComponent<Text>().text = "You can only use these options once a day, so choose wisely";
        else
            GameObject.Find("/UI/CanvasBencana/Dialog/DialogText").GetComponent<Text>().text = "I'll give you two options to resolve this issue. This is your LAST and FINAL CHANCE";

        GameObject.Find("/UI/CanvasBencana/Dialog").GetComponent<Button>().onClick.AddListener(() => showChoices());
    }

    IEnumerator SwitchImagesCoroutine(DisasterUIData data)
    {
        int currentImageIndex = 0;

        while (true)
        {
            if (currentImageIndex == 1)
            {
                GameObject.Find("/UI/CanvasBencana/Warning/EmergencyLight").GetComponent<Image>().sprite = Resources.Load<Sprite>("Disaster/EmergencyLightOff");
                GameObject.Find("/UI/CanvasBencana/Warning/PollutionBar").GetComponent<Image>().sprite = Resources.Load<Sprite>("Disaster/" + data.pollutionBarOff);
                currentImageIndex = 0;
            }
            else
            {
                GameObject.Find("/UI/CanvasBencana/Warning/EmergencyLight").GetComponent<Image>().sprite = Resources.Load<Sprite>("Disaster/EmergencyLightOn");
                GameObject.Find("/UI/CanvasBencana/Warning/PollutionBar").GetComponent<Image>().sprite = Resources.Load<Sprite>("Disaster/" + data.pollutionBarOn);
                currentImageIndex = 1;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public async void showChoices()
    {
        GameObject.Find("/UI/CanvasBencana/Dialog").SetActive(false);
        GameObject.Find("/UI/CanvasBencana/Pilihan").SetActive(true);
        DocumentSnapshot snapshot = await FirebaseFirestore.DefaultInstance.Collection("users").Document("player").GetSnapshotAsync();

        if (snapshot.Exists)
        {
            DateTime sacrificeKarmaTimestamp = snapshot.GetValue<Timestamp>("sacrifice_karma_timestamp").ToDateTime();
            Debug.Log(sacrificeKarmaTimestamp.Date);
            if (sacrificeKarmaTimestamp.Date < DateTime.Now.Date)
            {
                Debug.Log("sbelum hari ini");
                GameObject.Find("/UI/CanvasBencana/Pilihan/Sacrifice").gameObject.SetActive(true);
                GameObject.Find("/UI/CanvasBencana/Pilihan/Karma").gameObject.SetActive(true);
            }
        }
    }

    public void HideDisasterCanvas()
    {
        Debug.Log("hideee");
        GameObject.Find("/UI/CanvasBencana/Pilihan").gameObject.SetActive(false);
        GameObject.Find("/UI/CanvasBencana/Pilihan/Sacrifice").gameObject.SetActive(false);
        GameObject.Find("/UI/CanvasBencana/Pilihan/Karma").gameObject.SetActive(false);
    }
}
