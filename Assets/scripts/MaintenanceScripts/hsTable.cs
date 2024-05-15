using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


//This is a class to hold a single entry on the leaderboard.
[System.Serializable]
public class ScoreEntryClass
{
    [SerializeField]
    private float _myScore;

    

    // this is a property. When accessed it uses the underlying field,
    // but only exposes the contract, which will not be affected by the underlying field
    public float score
    {
        get
        {
            return _myScore;
        }
        set
        {
            _myScore = value;
        }
    }


    [SerializeField]
    private string _myName;

    // this is a property. When accessed it uses the underlying field,
    // but only exposes the contract, which will not be affected by the underlying field
    public string name
    {
        get
        {
            return _myName;
        }
        set
        {
            _myName = value;
        }
    }

}

[System.Serializable]
public class Scoreboard
{


    [SerializeField]
    public List<ScoreEntryClass> scoreBoard = new List<ScoreEntryClass>();

}



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

    //Down here is a buttload of stuff I(Lukas) copy/pasted from my seperate project I made to test this stuff.
    public delegate void OnBeginNaming();
    public static OnBeginNaming onBeginNaming;


    public delegate void OnCompletedNaming(string name);
    public static OnCompletedNaming onCompletedNaming;


    [SerializeField]
    float scoresPositioningPadding = 8;

    [SerializeField]
    Transform scoreEntryPrefab;

    [SerializeField]
    Transform[] scoreEntries = new Transform[4];

    string blankJsonScoreboard;

    private string scoreBoardJsonData;

    public int inputInt;

    private ScoreEntryClass inputScore = null;

    Scoreboard scores = new Scoreboard();

    [SerializeField]
    TMP_Text pickANameText;

    [SerializeField]
    GameObject nameMaker;

    private Transform myNewRecordTransform = null;


    private void Start()
    {
        nameMaker.SetActive(false);


        pickANameText.text = "YOUR SCORE: " + playerManager.currentScore.ToString("f0") + " PTS";

        for (int i = 0; i < scoreEntries.Length; i++)
        {
            scoreEntries[i] = Instantiate(scoreEntryPrefab, transform);

            scoreEntries[i].position = transform.position + Vector3.down * scoresPositioningPadding * i;

            string defaultPlace = (i + 1).ToString();

            string defaultName = "???";

            float defaultScore = 0; //Mathf.Pow(10, scoreEntries.Length - 1 - i) * 8;

            scoreEntries[i].Find("Place").GetComponent<TMP_Text>().text = defaultPlace;

            scoreEntries[i].Find("Name").GetComponent<TMP_Text>().text = defaultName;

            scoreEntries[i].Find("Score").GetComponent<TMP_Text>().text = defaultScore.ToString();

            scores.scoreBoard.Add(new ScoreEntryClass() { score = defaultScore, name = defaultName });
        }



        onCompletedNaming = FinishedNaming;

        blankJsonScoreboard = JsonUtility.ToJson(scores);

        print(blankJsonScoreboard);

        Scoreboard unJsonScoreboard = JsonUtility.FromJson<Scoreboard>(blankJsonScoreboard);

        print(unJsonScoreboard);

        print(unJsonScoreboard.scoreBoard.Count);


        Debug.Log("Next we log the player!");
        Debug.Log(playerManager);

        Debug.Log(onBeginNaming);

        AssembleScoreboard(new ScoreEntryClass { score = Mathf.Round(playerManager.currentScore), name = "???" });

    }
    void Update()
    {




        /* old code
        csNumb.text = "YOUR SCORE: " + playerManager.currentScore.ToString("f0") + " PTS";
        hsNumb.text = "HIGH SCORE: " + playerManager.highestScore.ToString("f0") + " PTS";

        */

        if (myNewRecordTransform != null)
        {
            Color currentHighlightColor = Color.Lerp(Color.white, new Color(44f / 255f, 202f / 255f, 229f / 255f, 1), Mathf.Sin(Time.realtimeSinceStartup) );

            myNewRecordTransform.Find("Name").GetComponent<TMP_Text>().color = currentHighlightColor;

            myNewRecordTransform.Find("Score").GetComponent<TMP_Text>().color = currentHighlightColor;

            myNewRecordTransform.Find("Place").GetComponent<TMP_Text>().color = currentHighlightColor;
        }


    }



    public void AssembleScoreboard(ScoreEntryClass newScore)
    {

        inputScore = newScore;

        //So first we need to load the data. The key will be "Scoreboard"

        scoreBoardJsonData = PlayerPrefs.GetString("Scoreboard", blankJsonScoreboard);

        scores = JsonUtility.FromJson<Scoreboard>(scoreBoardJsonData);



        //Then we need to measure the new score against the top x-amount to see if it fits.

        int indexWhereNewScoreFits = 0;
        bool foundSpotForNewScore = false;

        for (int i = 0; i < scoreEntries.Length; i++)
        {
            ScoreEntryClass currentScoreEntryToCompare = scores.scoreBoard[i];

            if (newScore.score > currentScoreEntryToCompare.score)
            {
                indexWhereNewScoreFits = i;

                foundSpotForNewScore = true;


                myNewRecordTransform = scoreEntries[i].transform;


                break;
            }
            else
            {


            }
        }

        //If it does fit, we need to move lesser scores down and remove the one that doesn't fit on the board anymore.

        if (foundSpotForNewScore)
        {

            //First we need to display the Name Maker!
            nameMaker.SetActive(true);


            scores.scoreBoard.Insert(indexWhereNewScoreFits, newScore);

            scores.scoreBoard.RemoveAt(scores.scoreBoard.Count - 1);

            Debug.Log("Found fit at..." + indexWhereNewScoreFits.ToString());

            pickANameText.text = "PICK A NAME: YOU MADE THE TOP " + scoreEntries.Length.ToString() + "!"; 

            

            //prompt player to name their score!
            //Focus jumps to the NameMaker script
            onBeginNaming.Invoke();

            //And after that, focus jumps back to this script for the score saving function!

        }else{
            playerManager.inputtedScore = true;

        }

        ShowBoard();
    }

    public ScoreEntryClass MakeNewScoreEntry(string name, float score)
    {
        ScoreEntryClass newScore = new ScoreEntryClass { score = score, name = name };

        return newScore;
    }


    void SaveScoreboard()
    {

        scoreBoardJsonData = JsonUtility.ToJson(scores);

        PlayerPrefs.SetString("Scoreboard", scoreBoardJsonData);
    }


    void ShowBoard()
    {
        for (int i = 0; i < scoreEntries.Length; i++)
        {
            Transform currentBoardEntry = scoreEntries[i];

            currentBoardEntry.Find("Name").GetComponent<TMP_Text>().text = scores.scoreBoard[i].name;

            currentBoardEntry.Find("Score").GetComponent<TMP_Text>().text = scores.scoreBoard[i].score.ToString();

            currentBoardEntry.Find("Place").GetComponent<TMP_Text>().text = (i+1).ToString();

        }
    }

    void FinishedNaming(string newName)
    {
        pickANameText.text = "PRESS SPACE TO PLAY AGAIN";


        inputScore.name = newName;

        SaveScoreboard();
        ShowBoard();

        playerManager.inputtedScore = true;



    }
}
