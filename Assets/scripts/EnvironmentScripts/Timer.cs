using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public PlayerController playerController;


    [SerializeField] TMP_Text pointComponenet;
    [SerializeField] TMP_Text speedComponenet;
    [SerializeField] TMP_Text milesComponenet;


    public float startTime;
    public float currentTime;

    bool timerStarted = false;

    //points
    [SerializeField] float pointMultiplier = 1.5f; public float currentPoints = 0;

    //speed
    [SerializeField] float speedMultiplier = 1f; public float currentSpeed = 0; 
    //speed min and max
    public float maxSpeed;
    public float minSpeed;

    //miles
    [SerializeField] float milesMultiplier = 0.5f; private float currentMiles = 0;

    void Start()
    {
        currentTime = startTime;
        timerStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            //increase multipliers if speedUp
            if (playerController.speedUp == true)
            {
                pointMultiplier = 1.75f;
                
                if (currentSpeed >= maxSpeed)
                {
                    speedMultiplier = maxSpeed;
                } else
                {
                    speedMultiplier += 0.25f;
                }

                milesMultiplier = 0.6f;
            } 
            else
            {
                //reset to default values if !speedUp
                //points
                pointMultiplier = 1.5f; 
                //speed 
                if (currentSpeed > minSpeed)
                {
                    speedMultiplier -= 0.25f;
                }
                else if (speedMultiplier <= minSpeed)
                {
                    speedMultiplier = minSpeed;
                }
                //miles
                milesMultiplier = 0.5f;
            }

            //update values
            currentTime += Time.deltaTime;

            //points
            currentPoints += currentTime * pointMultiplier;
            //speed
            currentSpeed = speedMultiplier;
            //miles
            currentMiles +=  0.01f * milesMultiplier;


            //display values
            pointComponenet.text = currentPoints.ToString("f0") + " PTS";
            speedComponenet.text = currentSpeed.ToString("f0") + " MPH";
            milesComponenet.text = currentMiles.ToString("f0") + " MI";

        }

    }
}
