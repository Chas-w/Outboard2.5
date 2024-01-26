using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class ImageEffectLensMod : MonoBehaviour
{
    [SerializeField] Material lensMat;
    [SerializeField] Material vMat;
    [SerializeField] Material caMat;
    [SerializeField] Material healthMat;
    [SerializeField] Material rippleMat;
    [SerializeField] Material speedLineMat;
    [SerializeField] Material shakeMat;
    [SerializeField] PlayerController playerController;

    //shader values to edit
    string d = "_distortion";
    string r = "_vr";
    string ca = "_intensity";
    string t = "_threshold";
    string s = "_saturation";
    string rs = "_rs";
    string sli = "_lineIntensity";
    string ss = "_shakeScale";

    //lens shader variable
    float maxDistort = -0.35f;
    float minDistort = -0.25f;
    float currentDistort;

    //vignette shader variable
    float maxRadius = 0.73f;
    float minRadius = 0.79f;
    float currentRadius;

    float saturation = 1f;

    //chromatic aberration variables
    float maxIntensity = 0.025f;
    float minIntensity = 0.01f;
    float currentIntensity;

    //health vars
    float threshold = 1.0f;

    //speed ripple
    float maxRipple = 0.004f;
    float minRipple = 0.002f;
    float currentRipple;

    //speed lines
    float maxLine = 0.3f;
    float minLine = 0.1f;
    float currentLine;

    //shake scale
    float maxShake = 0.006f;
    float minShake = 0.001f;
    float currentShake;

    void Start()
    {
        //get material componenet
        lensMat = GetComponent<ImageEffectLensMod>().lensMat;
        //return error if no property
        if (!lensMat.HasProperty(d))
        {
            Debug.LogError("the shader associated with the material on this game object is missing a necessary property. _distortion is required");
        }

        if (!vMat.HasProperty(r))
        {
            Debug.LogError("the shader associated with the material on this game object is missing a necessary property. _distortion is required");
        }

        currentDistort = -0.25f;
        currentRadius = 0.809f;
        currentIntensity = 0.01f;
        threshold = 1.0f;
        currentRipple = 0.001f;
        currentLine = 0.1f;
        currentShake = 0.001f;

        saturation = 1f;
    }
    void Update()
    {
        //if hit obstacle

        if (playerController.hitObstacle == true)
        {
            threshold = 0.23f;
            saturation = .4f;
        } 
        else if (playerController.health < playerController.healthMax && playerController.health > (playerController.health -1 ))
        {
            threshold = 0.15f;
            saturation = 0.75f;
        } 
        else if (playerController.health <= (playerController.healthMax - 1))
        {
            threshold = 0.015f;
            saturation = 0.6f;
        }
        else
        {
            threshold = 1.0f;
            saturation = 1.0f;
        }


        //if speeding up
        if (playerController.speedUp == true)
        {

            //lens
            if (currentDistort <= maxDistort)
            {
                currentDistort = maxDistort;
            }
            else
            {
                currentDistort -= 0.005f;
            }

            //vignette
            if (currentRadius <= maxRadius)
            {
                currentRadius = maxRadius;
            }
            else
            {
                currentRadius -= 0.0005f;
            }

            //chromatic ab.
            if (currentIntensity <= maxIntensity)
            {
                currentIntensity = maxIntensity;
            }
            else
            {
                currentIntensity += 0.000001f;
            }

            //ripple
            if (currentRipple <= maxRipple)
            {
                currentRipple = maxRipple;
            }
            else
            {
                currentRipple += 0.0001f;
            }

            //lines
            if (currentLine <= maxLine)
            {
                currentLine = maxLine;
            }
            else
            {
                currentLine += 0.00001f;
            }

            //shake
            if (currentShake <= maxShake)
            {
                currentShake = maxShake;
            }
            else
            {
                currentShake += 0.0001f;
            }

        } 
        else
        {
            //reset to default values if !speedUp
  
            //lens
            if (currentDistort < minDistort)
            {
                currentDistort += 0.005f;
            }
            else if (currentDistort >= minDistort)
            {
                currentDistort = minDistort;
            }

            //vignette
            if (currentRadius < minRadius)
            {
                currentRadius += 0.0005f;
            }
            else if (currentRadius >= minRadius)
            {
                currentRadius = minRadius;
            }

            //Chromatic ab
            if (currentIntensity < minIntensity)
            {
                currentIntensity -= 0.00001f;
            }
            else if (currentIntensity >= minIntensity)
            {
                currentIntensity = minIntensity;
            }

            //ripple
            if (currentRipple < minRipple)
            {
                currentRipple -= 0.00001f;
            }
            else if (currentRipple >= minRipple)
            {
                currentRipple = minRipple;
            }

            //lines
            if (currentLine < minLine)
            {
                currentLine -= 0.00001f;
            }
            else if (currentLine >= minLine)
            {
                currentLine = minLine;
            }

            //shake
            if (currentShake < minShake)
            {
                currentShake -= 0.00001f;
            }
            else if (currentShake >= minShake)
            {
                currentShake = minShake;
            }
        }

        //set distortion based on speed
        shakeMat.SetFloat(ss, currentShake);
        rippleMat.SetFloat(rs, currentRipple);
        speedLineMat.SetFloat(sli, currentLine);
        lensMat.SetFloat(d, currentDistort);
        vMat.SetFloat(r, currentRadius);
        vMat.SetFloat(s, saturation);
        caMat.SetFloat(ca, currentIntensity);
        healthMat.SetFloat(t, threshold);

    }
}
