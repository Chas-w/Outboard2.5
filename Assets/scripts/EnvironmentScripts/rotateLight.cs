using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateLight : MonoBehaviour
{
    public float speed;
    Transform t;

    void Start()
    {
        t = transform;
    }

    void Update()
    {
        t.rotation = Quaternion.Euler(t.eulerAngles + Vector3.up * speed * Time.deltaTime);
    }
}
