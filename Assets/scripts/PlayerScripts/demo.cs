using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class demo : MonoBehaviour
{
    [Header("Input Buttons and Keys")]
    public KeyCode KeyRight;
    public KeyCode KeyLeft;
    public KeyCode KeyUp;
   
    public int HorzMovement;
    public float speed = 5f;

    [Header("Player External")]
    public Rigidbody2D myBody;
    public Transform leftScreen;
    public Transform rightScreen;
    
    float moveSpeed;
    float horzIncriment = .75f;

    bool TouchRight; 
    bool TouchLeft;
    // Start is called before the first frame update
    void Start()
    {
        myBody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
     
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveInput();
        playerMove(); 
    }

    public void playerMove ()
    {   
        //Slidey movement
        if (moveSpeed < speed*HorzMovement) { moveSpeed += horzIncriment; } 
        else if (moveSpeed > speed*HorzMovement) { moveSpeed-= horzIncriment; } 

        //apply movement to rigid body
        myBody.velocity = new Vector3(moveSpeed, 0f, 0f);
    }

    public void MoveInput()
    {
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

        playerMove();
    }

   
}
