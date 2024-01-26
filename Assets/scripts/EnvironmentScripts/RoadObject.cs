using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObject : MonoBehaviour
{
    //public Vector3 worldPos = Vector3.zero;

    public float xRoadPosition;

    public float zRoadPosition;

    public Sprite renderSprite;

    private SpriteRenderer myRenderer;


    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
