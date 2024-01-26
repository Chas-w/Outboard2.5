using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//OutBoard
public class parallax : MonoBehaviour
{
    public float scrollSpeed;

    [SerializeField]
    Transform camera;


    [SerializeField]
    GameObject roadManagerObject;

    Camera camComponent;

    Sprite ourSprite;

    float spriteWidth;

    Vector3 cameraPreviousPos;

    private RoadManager roadManager;

    private Vector3 basePos;

    // Start is called before the first frame update
    void Start()
    {
        ourSprite = GetComponent<SpriteRenderer>().sprite;

        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        camComponent = camera.gameObject.GetComponent<Camera>();

        roadManager = roadManagerObject.GetComponent<RoadManager>();

        basePos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {



        float camPosDelta=0;

        float ZPos = roadManager.ZPos;
        if (roadManager.FindSegment(ZPos).curviness != 0)
        {
            //myBody.AddForce(new Vector2(-roadManager.FindSegment(ZPos).curviness * centrifugalForceMultiplier, 0f), ForceMode2D.Impulse);


            //This little bit is what we added!
            if (roadManager.FindSegment(ZPos).index > roadManager.segmentToCalculateLoopAt)
            {
                camPosDelta += -roadManager.endSegments[roadManager.FindSegment(ZPos).index - roadManager.segmentToCalculateLoopAt].curviness * scrollSpeed * roadManager.speed * Time.deltaTime;

                //camPosDelta += -Mathf.Sign(roadManager.endSegments[roadManager.FindSegment(ZPos).index - roadManager.segmentToCalculateLoopAt].curviness) * 9 * Time.deltaTime;
            }
            else
            {
                camPosDelta += -roadManager.FindSegment(ZPos).curviness * scrollSpeed * roadManager.speed * Time.deltaTime;

                //camPosDelta += -Mathf.Sign(roadManager.FindSegment(ZPos).curviness) * 9 * Time.deltaTime;
            }


            //Debug.Log(-roadManager.FindSegment(ZPos).curviness);
        }


        //float camPosDelta = cameraPreviousPos.x - camera.position.x;

        transform.position += new Vector3(camPosDelta, 0, 0);


        checkPositionReset();
    }

    //Every once in a while we'll have to move the tiled object back a bit to maintain the infinite scrolling illusion! This checks if it's time yet.
    void checkPositionReset()
    {

        float camHeight = 2f * camComponent.orthographicSize;

        float camWidth = 16f / 9f * camHeight;


        if (Mathf.Abs(transform.position.x - basePos.x) > 2f*spriteWidth)
        {
            transform.position -= new Vector3(transform.position.x-basePos.x,0,0);
        }
    }

}
