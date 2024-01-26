using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneChange : MonoBehaviour
{

    [SerializeField] string SceneToLoad;

    // Start is called before the first frame update
    void OnEnable()
    {
        //scene change
        SceneManager.LoadScene(SceneToLoad);
        Debug.Log("scene change");
    }
}
