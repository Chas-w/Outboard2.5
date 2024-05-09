using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("External Variables")]
    public RoadManager roadManager;
    public cameraShake camShake;
    public Timer timer;
    public float speedUpCDTimerMax;
    public float speedUpTimerMax; //option for alternate timer so speed up and speed up cooldown last for differing times
    public float speedUpTimer;
    public float speedUpMultiplier;
    public bool canSpeed;
    public float centrifugalForceMultiplier = 0.3f;


    [Header("Player Road Move Variables")]
    [SerializeField] float hitSpeed;
    [SerializeField] float hitSpeedTimerMax;
    [SerializeField] float gradualSpeedMultiplier;
    [SerializeField] float timerGradSpeedMax;
    [SerializeField] AudioSource hitNoise;
    public bool hitObstacle;

    [Header("Player Horizontal Move Variables")]
    public Rigidbody2D myBody;
    public Transform leftScreen;
    public Transform rightScreen;
    public int HorzMovement;
    public float horzSpeed = 5f;
    public float horzIncriment = .75f;

    [Header("Input Buttons and Keys")]
    public KeyCode KeyRight;
    public KeyCode KeyLeft;
    public KeyCode KeyUp;

    [Header("Internal Player Variables")]
    public float healthMax;
    public float health;
    public bool speedUp;

    [Header ("Private Variables")]
    float hitSpeedTimer;
    float timerGradSpeed;
    float moveSpeed;
    //Player anim parameters
    bool leftPressed = false;
    bool rightPressed = false;

    Animator myAnim;

    SpriteRenderer myRend;


  
    // Start is called before the first frame update
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();

        myAnim = gameObject.GetComponent<Animator>();
        myRend = gameObject.GetComponent<SpriteRenderer>();


        health = healthMax;
        timerGradSpeed = timerGradSpeedMax;
    }

    // Update is called once per frame
    void Update()
    {
        MoveAnim(); 
        MoveInput();
    }
    void FixedUpdate()
    {
        playerMove();

        #region gradual speed up
        if (timerGradSpeed <= 0)
        {
            roadManager.normSpeed += gradualSpeedMultiplier;
            timer.maxSpeed += gradualSpeedMultiplier;
            timer.minSpeed += gradualSpeedMultiplier;
            timerGradSpeed = timerGradSpeedMax;
            //Debug.Log(timer.normSpeed);
        }
        else { timerGradSpeed--; }
        #endregion

        #region norm speed
        if (!hitObstacle)
        {
            if (roadManager.speed <= roadManager.normSpeed)
            {
                roadManager.speed++;
            }
            if (speedUp && roadManager.speed <= roadManager.maxSpeed)
            {
                //Debug.Log(speedUp);
                roadManager.speed += speedUpMultiplier;
            }
            if (!speedUp)
            {
                if (roadManager.speed >= roadManager.normSpeed)
                {
                    roadManager.speed -= speedUpMultiplier * 0.5f;
                }
            }
        }
        #endregion

        #region collide speed
        if (hitObstacle)
        {
            if (hitSpeedTimer > 0)
            {
                hitNoise.Play();
                roadManager.speed = hitSpeed;
                camShake.CameraShake();
                myAnim.SetBool("hitAnim", true);
                hitSpeedTimer--;
                
            }
            if (hitSpeedTimer <= 0)
            {
                health--;
                camShake.StopShake();
                myAnim.SetBool("hitAnim", false);
                hitObstacle = false;
                
            }
        }
        #endregion

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "staticObstacle")
        {
            hitObstacle = true;
            hitSpeedTimer = hitSpeedTimerMax;
        }
    }

    public void playerMove()
    {
        //Slidey movement
        if (moveSpeed < horzSpeed * HorzMovement) { moveSpeed += horzIncriment; }
        else if (moveSpeed > horzSpeed * HorzMovement) { moveSpeed -= horzIncriment; }


        //Calculating centrifugal force!
        float ZPos = roadManager.ZPos;
        float centrifugalForce = 0;

        if (roadManager.FindSegment(ZPos).index > roadManager.segmentToCalculateLoopAt)
        {
            centrifugalForce += -roadManager.endSegments[roadManager.FindSegment(ZPos).index - roadManager.segmentToCalculateLoopAt].curviness * Mathf.Pow(centrifugalForceMultiplier, 2) * roadManager.speed;
        }
        else
        {
            centrifugalForce += -roadManager.FindSegment(ZPos).curviness * Mathf.Pow(centrifugalForceMultiplier, 2) * roadManager.speed;
        }

        //apply movement to rigid body
        myBody.velocity = new Vector3(moveSpeed + centrifugalForce, 0f, 0f);
    }
    public void MoveInput()
    {
        //HORIZONTAL
        //Get Input && Define HorzMovement
        HorzMovement = 0; //defaults to zero
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //convert mouse position from screed cord. to word cord. 
        if (Input.GetKey(KeyRight) || /*TouchInput*/ (Input.GetMouseButton(0) && (mousePos.x > rightScreen.position.x))) //right input
        {
            HorzMovement = 1;
        }

        if (Input.GetKey(KeyLeft) || /*TouchInput*/ (Input.GetMouseButton(0) && (mousePos.x < leftScreen.position.x))) //left input
        {
            HorzMovement = -1;
        }

        //SPEEDUP
        /*
        if (speedUpTimer <= 0)
        {
            canSpeed = true;
        }
        */
        if (canSpeed && (Input.GetKeyDown(KeyCode.W) || (Input.GetMouseButton(0) && ((mousePos.x < rightScreen.position.x) && (mousePos.x > leftScreen.position.x)))))
        {
            speedUp = true;
            canSpeed = false;
        } else if ((Input.GetKeyUp(KeyCode.W) || (Input.GetMouseButtonUp(0) && ((mousePos.x < rightScreen.position.x) && (mousePos.x > leftScreen.position.x))))) { speedUp = false; canSpeed = true;  }
        /*
        if (speedUp && speedUpTimer <= speedUpCDTimerMax)
        {
            speedUpTimer++;
        }

        if (speedUpTimer >= speedUpCDTimerMax)
        {
            speedUp = false;
        } if (speedUpTimer >= 0 && !speedUp)
        {
            speedUpTimer--;
        }
        */

    }
    public void MoveAnim()
    {
        if (HorzMovement < 0) // turning left
        {
            myAnim.SetBool("leftHold", true); // continues to hold button down, transitions to holding anim 
            if (leftPressed == false) // first frame
            {
                leftPressed = true;
                myAnim.SetTrigger("leftPressed"); // anim activated one time 
            }
            rightPressed = false;
            myAnim.SetBool("rightHold", false);
            myAnim.ResetTrigger("rightPressed");
            //myAnim.ResetTrigger("leftPressed");
        }
        else if (HorzMovement > 0) // turning right
        {
            myAnim.SetBool("rightHold", true); // continues to hold button down, transitions to holding anim 
            if (rightPressed == false) // first frame
            {
                rightPressed = true;
                myAnim.SetTrigger("rightPressed"); // anim activated one time 
            }
            leftPressed = false;
            myAnim.SetBool("leftHold", false);
            //myAnim.ResetTrigger("rightPressed");
            myAnim.ResetTrigger("leftPressed");

        }
        else if (HorzMovement == 0) //reset all values/directions 
        {
            leftPressed = false;
            rightPressed = false;
            myAnim.ResetTrigger("rightPressed");
            myAnim.ResetTrigger("leftPressed");
            myAnim.SetBool("leftHold", false);
            myAnim.SetBool("rightHold", false);
        }
        //if (hitObstacle == true) { myAnim.SetBool("hitobstacle", false); }
        else { myAnim.SetBool("hitObstacle", false); }
    }

}
