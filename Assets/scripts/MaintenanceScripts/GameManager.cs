using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public GameObject timer;
    public float currentScore;
    public float finalScore;
    public float highestScore;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKey)
        {
            if (SceneManager.GetActiveScene().name == "MENU")
            {
                SceneManager.LoadScene("GAMEPLAYSCENE");
            }
            if (SceneManager.GetActiveScene().name == "SCOREBOARD")
            {
                SceneManager.LoadScene("MENU");
            }
        }

        if (SceneManager.GetActiveScene().name == "GAMEPLAYSCENE") 
        {
            timer = GameObject.Find("inGameUI");
            Timer score = timer.GetComponent<Timer>();
            currentScore = score.currentPoints;


            GameObject player = GameObject.Find("player");
             PlayerController playerController = player.GetComponent<PlayerController>();

            if (playerController.health <= 0)
            {
                SceneManager.LoadScene("SCOREBOARD");
            }
        }

        if (SceneManager.GetActiveScene().name == "SCOREBOARD")
        {
            finalScore = currentScore;
            if (finalScore > highestScore)
            {
                highestScore = currentScore;
            }
        }

    }
}
