using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;

public class NameMaker : MonoBehaviour
{
    KeyCode KeyRight;
    KeyCode KeyLeft;
    KeyCode KeyUp;


    [SerializeField]
    TMP_Text[] letters;

    [SerializeField]
    Image checkmark;


    [SerializeField]
    int selectedDigitIndex = 0;

    [SerializeField]
    int currentAlphabetnumber = 0;

    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [SerializeField]
    private RectTransform upDownButtons;

    [SerializeField]
    float buttonsLerpSpeed = 3;

    public Color highlightColor = new Color(44f/255f, 202f/255f, 229f/255f, 1);


    public bool currentlyNaming = false;

    // Start is called before the first frame update

    private void Awake()
    {

        hsTable.onBeginNaming += BeginNaming;

    }

    void Start()
    {
        Debug.Log(alphabet.Length);

        
    }




    // Update is called once per frame
    void Update()
    {
        if (currentlyNaming)
        {




            Vector3 arrowsTargetPos;

            

            if (selectedDigitIndex == letters.Length)
            {
                //Make checkmark highlighted instead!

                checkmark.color = highlightColor;

                foreach (var letter in letters) {
                    letter.color = Color.white;
                    letter.fontStyle = FontStyles.Normal;

                }

                arrowsTargetPos = checkmark.transform.position;



                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SubmitName();
                }

            }
            else
            {
                checkmark.color = Color.white;

                letters[selectedDigitIndex].color = highlightColor;
                letters[selectedDigitIndex].fontStyle = FontStyles.Underline;


                letters[(selectedDigitIndex + 1) % letters.Length].color = Color.white;
                letters[selectedDigitIndex].fontStyle = FontStyles.Normal;

                letters[(selectedDigitIndex + 2) % letters.Length].color = Color.white;
                letters[selectedDigitIndex].fontStyle = FontStyles.Normal;

                arrowsTargetPos = letters[selectedDigitIndex].transform.position;

                letters[selectedDigitIndex].text = alphabet[currentAlphabetnumber].ToString();

                letters[selectedDigitIndex].text = alphabet[currentAlphabetnumber].ToString();

                //make checkmark not highlighted
            }
         

            upDownButtons.transform.position = Vector2.Lerp(
                upDownButtons.transform.position,
                arrowsTargetPos,
                buttonsLerpSpeed * Time.deltaTime);

            

            if (Input.GetKeyDown("w"))
            {
                IncreaseAlphabetNumber();






            }
            else if (Input.GetKeyDown("s"))
            {
                DecreaseAlphabetNumber();





            }


            if (Input.GetKeyDown("d"))
            {


                Debug.Log("RIGHT");

                IncreaseSelectedDigitIndex();


                SwitchSelectedLetter(selectedDigitIndex);


            }
            else if (Input.GetKeyDown("a"))
            {


                Debug.Log("LEFR");

                DecreaseSelectedDigitIndex();

                SwitchSelectedLetter(selectedDigitIndex);

            }
        }

    }


    public void SwitchSelectedLetter(int newLetterIndex)
    {
        selectedDigitIndex = newLetterIndex;



        if (selectedDigitIndex != letters.Length)
        {
            for (int i = 0; i < alphabet.Length; i++)
            {
                if (alphabet[i].ToString() == letters[selectedDigitIndex].text)
                {
                    currentAlphabetnumber = i;
                }
            }
        }

       

    }

    public void IncreaseAlphabetNumber()
    {

        Debug.Log("UP");
        currentAlphabetnumber = (currentAlphabetnumber + 1) % alphabet.Length;
    }

    public void DecreaseAlphabetNumber()
    {
        Debug.Log("DOWn");
        currentAlphabetnumber -= 1;
        if (currentAlphabetnumber < 0)
        {
            currentAlphabetnumber = alphabet.Length - 1;
        }
    }


    public void IncreaseSelectedDigitIndex()
    {
        selectedDigitIndex = (selectedDigitIndex + 1) % (letters.Length+1);
    }

    public void DecreaseSelectedDigitIndex()
    {
        selectedDigitIndex -= 1;
        if (selectedDigitIndex < 0)
        {
            selectedDigitIndex = letters.Length - 1;
        }
    }

    void BeginNaming()
    {
        currentlyNaming = true;
    }

    public void SubmitName()
    {
        string newName = "";
        for (int i = 0; i < letters.Length; i++)
        {
            newName += letters[i].text;
        }

        Debug.Log(newName);

        hsTable.onCompletedNaming.Invoke(newName);

    }
}
