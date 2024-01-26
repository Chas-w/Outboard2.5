using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedOverlay : MonoBehaviour
{
    [SerializeField] float camIce;

    public Image overlay;

    public Timer timer;

    float speedOp;
    float opacity;


    void Start()
    {
        speedOp = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //get percent out of max speed
        speedOp = 1 - (timer.currentSpeed / timer.maxSpeed);
        //at max speed display overlay -- don't display at min speed
        opacity = ((255 * speedOp)/ camIce);

        overlay.color = new Color(255, 255, 255, opacity);
    }
}
