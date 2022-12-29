using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* Class to manage loading of target words to be typed onto the screen */

public class WordSequencer : MonoBehaviour
{
    public TextMeshProUGUI wordDisplay;

    //The words to be typed by the user
    private string[] targetWords = 
    {"A5", "is", "the", "best", "and", "thats", "a", "fact"};

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSequencer()
    {
        StartCoroutine(StartWordSequencer());
    }

    IEnumerator StartWordSequencer()
    {
        int index = 0;

        Debug.Log("StartWordSequencer");
        foreach (string word in targetWords)
        {  
            wordDisplay.text = targetWords[index];
            index++;
            yield return new WaitForSeconds(1);
        }
        Debug.Log("StartWordSequencer: done.");
    }
}
