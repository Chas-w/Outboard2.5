using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour
{
    [Header("Internal Move Variables")]
    [SerializeField] Transform cameraTransform;
    
    public float shakeFrequency;

    private Vector3 originalCamPos;

    // Start is called before the first frame update
    void Start()
    {
        originalCamPos = cameraTransform.position; //assign current cam pos
    }

    public void CameraShake()
    {
        //moves camera to random point chosen within circle around the camera
        cameraTransform.position = originalCamPos + Random.insideUnitSphere * shakeFrequency;
    }

    public void StopShake()
    {
        //return camerea to original position
        cameraTransform.position = originalCamPos;
    }
}
