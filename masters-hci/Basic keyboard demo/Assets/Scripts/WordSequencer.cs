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
    public static event Action sequencerFinishedEvent;

    public Button startButton;

    public TextMeshProUGUI wordDisplay;
    int currentWordIndex = -1; //target word index. Will inc on first move to give index 0

    public static bool isPlaying = false;
    public LoggingSystem logger;

    //The words to be typed by the user
    private string[] targetWords = 
    {"This", "is", "a", "test", "for", "typing", "each", "word"};

    void OnEnable()
    {
        ButtonManager.startButtonClickedEvent += StartButtonClicked;
    }

    void OnDisable()
    {
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
            StopWordSequencer();
            sequencerFinishedEvent.Invoke();

            Debug.Log("Finished test.");
            logger.writeMessageToLog("Finished test.");
            //clean up, stop timer, close log
        }
        else
        {
            //Show next word
            wordDisplay.text = targetWords[currentWordIndex];
            logger.writeAOTMessageWithTimestampToLog("Show new target word: " + targetWords[currentWordIndex], " " , " ");
        }
    }

    void CheckInputString(string input)
    {
        //Debug.Log("Compare input to: " + targetWords[currentWordIndex]);
    
        if(isPlaying){
            //Log input word, target word
            logger.writeAOTMessageWithTimestampToLog (input, targetWords[currentWordIndex], " ");

            if(input.Equals(targetWords[currentWordIndex]))
            {
                //Debug.Log("Strings match!" + input); 
                wordCompletedEvent.Invoke();
                ShowNextWord();
            }
        }
    }

    public void InputFieldChanged(string input)
    {
        //Debug.Log("Field changed: " + input); 
        CheckInputString(input);
    }

    public void StopWordSequencer()
    {
        Debug.Log("StopWordSequencer");
        logger.writeMessageToLog("StopWordSequencer");
        logger.writeAOTMessageWithTimestampToLog("StopWordSequencer", " " , " ");

        isPlaying = false;
        currentWordIndex = -1;
        wordDisplay.text = "";
    }

    public void StartWordSequencer()
    {
        Debug.Log("StartWordSequencer");
        logger.writeAOTMessageWithTimestampToLog("StartWordSequencer", " " , " ");
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
