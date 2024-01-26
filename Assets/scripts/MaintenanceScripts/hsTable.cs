using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class hsTable : MonoBehaviour
{
    // references https://www.youtube.com/watch?v=iAbaqGYdnyI&t=49s for code help 
    public TMP_Text hsNumb;
    public TMP_Text csNumb;
    public GameObject GameManager;
    public GameManager playerManager;

    public static hsTable Instance { get; private set; }

    private void Awake()
    {
        GameManager = GameObject.Find("GameManager");
        playerManager = GameManager.GetComponent<GameManager>();

    }

   // float highestScore = 0;
    //float currentScore;
    //float finalScore;
    private void Start()
    {
        
    }
    void Update()
    {

        csNumb.text = "YOUR SCORE: " + playerManager.currentScore.ToString("f0") + " PTS";
        hsNumb.text = "HIGH SCORE: " + playerManager.highestScore.ToString("f0") + " PTS";

    }
}
