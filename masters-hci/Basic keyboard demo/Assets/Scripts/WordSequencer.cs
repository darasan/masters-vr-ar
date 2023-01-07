using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

/* Class to manage loading of target words to be typed onto the screen */
public class WordSequencer : MonoBehaviour
{
    public static event Action wordCompletedEvent;

    public Button startButton;

    public TextMeshProUGUI wordDisplay;
    int currentWordIndex = 0; //target word index

    public static bool isPlaying = false;

    //The words to be typed by the user
    private string[] targetWords = 
    {"A5", "is", "the", "best", "and", "thats", "a", "fact"};

    void OnEnable()
    {
        Debug.Log("WordSeq OnEnable, subscribe events");
        ButtonManager.startButtonClickedEvent += StartButtonClicked;
    }

    void OnDisable()
    {
        Debug.Log("WordSeq OnDisable, unsubscribe events");
        ButtonManager.startButtonClickedEvent -= StartButtonClicked;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowNextWord()
    {
        currentWordIndex++;
        if(currentWordIndex>= targetWords.Length)
        {
            Debug.Log("Finished test."); 
            //clean up, stop timer, close log
        }
        else
        {
            //Show next word
            wordDisplay.text = targetWords[currentWordIndex];
        }
    }

    void CheckInputString(string input)
    {
        //Debug.Log("Compare input to: " + targetWords[currentWordIndex]);

        if(input.Equals(targetWords[currentWordIndex]))
        {
            Debug.Log("Strings match!" + input); 
            wordCompletedEvent.Invoke();
            ShowNextWord();
        }
    }

    public void InputFieldChanged(string input)
    {
        Debug.Log("Field changed: " + input); 
        CheckInputString(input);
    }

    public void StopWordSequencer()
    {
        Debug.Log("StopWordSequencer");

        isPlaying = false;
        currentWordIndex = -1; //will inc on first move to show index 0
        wordDisplay.text = "";
    }

    public void StartWordSequencer()
    {
        Debug.Log("StartWordSequencer");
        isPlaying = true;
        ShowNextWord();
    }

    void StartButtonClicked()
    {
        if(isPlaying){
            StopWordSequencer();
        }
        else{
            StartWordSequencer();
        }
        
    }

}