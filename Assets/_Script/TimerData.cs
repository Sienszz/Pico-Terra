using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerData : MonoBehaviour
{
    public float timerIncrement = 0.0f;
    //public float timerCurrency = 0.0f;
    public float timerPolution = 0.0f;
    public float timerNature = 0.0f;
    public float timerPopulation = 0.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Timer
        // Increment the timer using Time.deltaTime
        timerIncrement += Time.deltaTime;
        //timerCurrency += Time.deltaTime;
        timerPolution += Time.deltaTime;
        timerNature += Time.deltaTime;
        // Check if one second has passed
    }
}