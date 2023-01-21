using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

/* Class to manage loading of target words to be typed onto the screen */
public class WordSequencer : MonoBehaviour
{
    public static event Action wordCompletedEvent;
    public static event Action sequencerFinishedEvent;
    public static event Action<List<string>> newDictionarySearchResultsEvent;

    public Button startButton;
    public GameObject usernameField;
    public GameObject keyboardType;
    private TMP_InputField username;
    private TMP_Dropdown kbdType;

    public TextMeshProUGUI wordDisplay; //the current target word
    int currentWordIndex = -1; //target word index. Will inc on first move to give index 0
    public TextMeshProUGUI progressDisplay; //shows number of words completed / remaining

    public static bool isPlaying = false;
    public LoggingSystem logger;

    public WordDictionary dict;
    private List<string> targetWords = new List<string>();

    int totalBackspaceKeyPresses = 0;
    int totalControlKeyPresses = 0;
    int totalUpKeyPresses = 0;
    int totalDownKeyPresses = 0;

    private void UpdateProgressDisplay()
    {
        progressDisplay.text = currentWordIndex + "/" + targetWords.Count;
    }

    private void LoadTargetWordsFromFile(string path)
    {
        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            Debug.Log("Could not open file at path: " + path);
        }

        else
        {
            Debug.Log("File opened OK: " + path);
        }

        // Open the file and read each line into a string array, then add to target words
        string[] readText = File.ReadAllLines(path);
        foreach (string word in readText)
        {
            targetWords.Add(word);
        }
    }

    void ShowNextWord()
    {
        currentWordIndex++;
        if(currentWordIndex>= targetWords.Count)
        {
            StopWordSequencer();
            sequencerFinishedEvent.Invoke();

            Debug.Log("Finished test.");
            logger.writeMessageToLog("Finished test.");
            progressDisplay.text = "Finished test!";

            logger.writeAOTMessageWithTimestampToLog("totalBackspaceKeyPresses: " + totalBackspaceKeyPresses, " " , " ");
            logger.writeAOTMessageWithTimestampToLog("totalControlKeyPresses: " + totalControlKeyPresses, " " , " ");
            logger.writeAOTMessageWithTimestampToLog("totalUpKeyPresses: " + totalUpKeyPresses, " " , " ");
            logger.writeAOTMessageWithTimestampToLog("totalDownKeyPresses: " + totalDownKeyPresses, " " , " ");
            //clean up, stop timer, close log
        }
        else
        {
            //Show next word
            wordDisplay.text = targetWords[currentWordIndex];
            UpdateProgressDisplay();
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
        List<string> searchResults = dict.SearchDictionaryForString(input); //Search the dictionary on the fly for each input string
        newDictionarySearchResultsEvent.Invoke(searchResults); //Trigger event to send results to ButtonManager
    }

    public void StopWordSequencer()
    {
        Debug.Log("StopWordSequencer");
        logger.writeMessageToLog("StopWordSequencer");
        logger.writeAOTMessageWithTimestampToLog("StopWordSequencer", " " , " ");

        isPlaying = false;
        currentWordIndex = -1;
        wordDisplay.text = "";
        //TODO - clear input field also
    }

    public void StartWordSequencer()
    {
        Debug.Log("StartWordSequencer");
        logger.writeAOTMessageWithTimestampToLog("StartWordSequencer", " " , " ");
        logger.writeAOTMessageWithTimestampToLog("Username: " + username.text, " " , " ");
        logger.writeAOTMessageWithTimestampToLog("Keyboard type: " + kbdType.value, " " , " ");

        //Reset statistics
        totalBackspaceKeyPresses = totalControlKeyPresses = totalDownKeyPresses =  totalUpKeyPresses = 0;
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

    //Just using these events for logger. Could really have 1 event and pass the key as parameter...much shorter
    void LeftKeyPressed()
    {
        logger.writeAOTMessageWithTimestampToLog("LeftKeyPressed", " " , " ");
    }

     void RightKeyPressed()
    {
        logger.writeAOTMessageWithTimestampToLog("RightKeyPressed", " " , " ");
    }

    void UpKeyPressed()
    {
        totalUpKeyPresses++;
        logger.writeAOTMessageWithTimestampToLog("UpKeyPressed", " " , " ");
    }

    void DownKeyPressed()
    {
        totalDownKeyPresses++;
        logger.writeAOTMessageWithTimestampToLog("DownKeyPressed", " " , " ");
    }

    void ControlKeyPressed()
    {
        totalControlKeyPresses++;
        logger.writeAOTMessageWithTimestampToLog("ControlKeyPressed", " " , " ");
    }

    void BackspaceKeyPressed()
    {
        totalBackspaceKeyPresses++;
        logger.writeAOTMessageWithTimestampToLog("BackspaceKeyPressed", " " , " ");
    }

    void OnEnable()
    {
        ButtonManager.startButtonClickedEvent += StartButtonClicked;
        InputHandler.leftKeyPressedEvent += LeftKeyPressed;
        InputHandler.rightKeyPressedEvent += RightKeyPressed;
        InputHandler.upKeyPressedEvent += UpKeyPressed;
        InputHandler.downKeyPressedEvent += DownKeyPressed;
        InputHandler.controlKeyPressedEvent += ControlKeyPressed;
        InputHandler.backspaceKeyPressedEvent += BackspaceKeyPressed;
    }

    void OnDisable()
    {
        ButtonManager.startButtonClickedEvent -= StartButtonClicked;
        InputHandler.leftKeyPressedEvent -= LeftKeyPressed;
        InputHandler.rightKeyPressedEvent -= RightKeyPressed;
        InputHandler.upKeyPressedEvent -= UpKeyPressed;
        InputHandler.downKeyPressedEvent -= DownKeyPressed;
        InputHandler.controlKeyPressedEvent -= ControlKeyPressed;
        InputHandler.backspaceKeyPressedEvent -= BackspaceKeyPressed;
    }

    // Start is called before the first frame update
    void Start()
    {
        username = usernameField.GetComponent<TMP_InputField>();
        kbdType = keyboardType.GetComponent<TMP_Dropdown>();

        string path = Application.persistentDataPath + "/targetWords.txt";
        logger.writeAOTMessageWithTimestampToLog("Application.persistentDataPath:" + Application.persistentDataPath, " " , " ");
        
        LoadTargetWordsFromFile(path);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
