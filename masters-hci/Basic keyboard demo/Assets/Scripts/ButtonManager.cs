using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using System; //for Action type

public class ButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject wordPrefab;
    public Button[] buttons;
    public GameObject scrollView;
    public GameObject scrollViewContent;
    public GameObject inputField; //Main text input field
    public Button startButton;
    public Toggle wordlistToggle;

    public static event Action startButtonClickedEvent;

    GameObject[] keys; //keyboard keys
    GameObject[] wordList; //word buttons
    int selectedWord = 0; //index of currently selected word in list
    Color darkGreen = new Color(0.29f, 0.74f, 0.35f, 1.0f);

    private string[] letters = 
    {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", 
     "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

    void PopulateWordList(List<string> words)
    {
        TextMeshProUGUI tmp_ugui;
        int index;

        //Debug.Log("num results: " + words.Count);

        if(words.Count>0) //don't change current list if no results
        {
            for(int i=0; i<wordList.Length; i++){ //limit to showing only wordlist length for now (5), not all
                index = i; 

                tmp_ugui = wordList[index].GetComponentInChildren<TextMeshProUGUI>();

                if(words.Count <= index)
                {
                    //We got less search results than words list size (5), just clear words at current index and higher
                    tmp_ugui.text = "";
                }

                else
                {
                    tmp_ugui.text = words[index];
                    //Debug.Log("Added to list: " + words[index]);
                }
            }
        }
    }

    private void WordCompletedSuccessfully()
    {
        Debug.Log("WordCompletedSuccessfully!");
        TMP_InputField inputfield = inputField.GetComponent<TMP_InputField>();
        inputfield.text = ""; 
    }

    public void OnToggleChanged(bool enabled)
    {
        scrollView.SetActive(enabled);
    }

    public void OnSoftKeyboardButtonClick(int id)
    {
        Debug.Log("Clicked: " + id.ToString());
    }

    void MoveToNextWord()
    {
        // Reset the currently selected button to the default colour.
        Image wordImage = wordList[selectedWord].GetComponentInChildren<Image>();
        wordImage.color = Color.white;
        selectedWord++;

        // Check that our new index does not move outside of our array.
        if(selectedWord >= wordList.Length)
        {
            selectedWord = 0;
        }
        // Set the currently selected word to the "selected" colour.
        wordImage = wordList[selectedWord].GetComponentInChildren<Image>();
        wordImage.color = darkGreen;
        Debug.Log("selectedWord: " + selectedWord);
    }

    void MoveToPreviousWord()
    {
        Image wordImage = wordList[selectedWord].GetComponentInChildren<Image>();
        wordImage.color = Color.white;

        selectedWord--;
        if(selectedWord < 0)
        {
            selectedWord = (wordList.Length - 1);
        }
        wordImage = wordList[selectedWord].GetComponentInChildren<Image>();
        wordImage.color = darkGreen;
        Debug.Log("selectedWord: " + selectedWord);
    }

    void CopySelectedWordToInputField()
    {
        string wordText = wordList[selectedWord].GetComponentInChildren<TextMeshProUGUI>().text;
        TMP_InputField inputfield = inputField.GetComponent<TMP_InputField>();

        inputfield.text = wordText;
        inputfield.MoveToEndOfLine(false, true);
    }

    void CreateKeyboard()
    {
        keys = new GameObject[26];

        float padding = 35.0f; //padding from left side of panel

        for(int i=0; i<buttons.Length; i++){

            int index = i; // Prevents the closure problem: https://stackoverflow.com/questions/271440/captured-variable-in-a-loop-in-c-sharp

            //Init button from prefab
            GameObject btn = Instantiate(buttonPrefab); //"setting pos but not correct - understand how to do. remember relative to parent"

            //Store game objects for each key
            keys[index] = btn;

            //Set position
            btn.transform.SetParent(this.transform, false); //set this panel as parent, else will instantiate at top level in hierarchy
            RectTransform rect = btn.GetComponent<RectTransform>();
            rect.SetLocalPositionAndRotation(new Vector3(index * 50.0f + padding, 0.0f, 0.0f), Quaternion.identity);
            rect.anchorMax = new Vector2(0.0f, 0.5f); //X: bottom  Y: middle
            rect.anchorMin = new Vector2(0.0f, 0.5f); //same for min

            //btn is the high level GO, store button component in array in inspector (adjust array size there)
            buttons[index] = btn.GetComponent<UnityEngine.UI.Button>();

            //Set text for button
            TextMeshProUGUI tmp_ugui = btn.GetComponentInChildren<TextMeshProUGUI>(); //Use GetCompInChildren when in hierarchy below (child), else wont find

            //Assign click callback
            buttons[index].onClick.AddListener(() => OnSoftKeyboardButtonClick(index));
        }
    }

    void CreateWordListButtons()
    {
        wordList = new GameObject[5];
        Image wordImage;
        
        for(int i=0; i<wordList.Length; i++){
            int index = i; 

            //Init from prefab
            GameObject word = Instantiate(wordPrefab);
            wordList[index] = word;

            //No need to set position, done by Vertical Layout Group component added to Content. Just set parent transform as Content
            word.transform.SetParent(scrollViewContent.transform, false);
            wordImage = wordList[index].GetComponentInChildren<Image>();
            if(index == 0){
                wordImage.color = darkGreen; //default to first selected in list. Matches selectedWord = 0 at startup
            }
            else{
                wordImage.color = Color.white;
            }
        }
    }

    public void StartButtonClicked()
    {
        Debug.Log("StartButtonClicked");
        startButtonClickedEvent.Invoke();

        Image image = startButton.GetComponentInChildren<Image>();
        TextMeshProUGUI btnText = startButton.GetComponentInChildren<TextMeshProUGUI>(); 

        if(WordSequencer.isPlaying){
            image.color = Color.red;
            btnText.text = "Stop";
        }

        else{ //Not playing, set ready to play
            image.color = darkGreen;
            btnText.text = "Start";
        }
    }

    //Called by sequencerFinishedEvent in Word Sequencer
    void SequencerFinished()
    {
        //Reset button / other UI
        Image image = startButton.GetComponentInChildren<Image>();
        TextMeshProUGUI btnText = startButton.GetComponentInChildren<TextMeshProUGUI>(); 

        image.color = darkGreen;
        btnText.text = "Start";
    }

    void UpKeyPressed()
    {
        MoveToPreviousWord();
    }

    void DownKeyPressed()
    {
        MoveToNextWord();
    }

    void ControlKeyPressed()
    {
        if(scrollView.activeInHierarchy)
        {
            CopySelectedWordToInputField();
        }
    }

    void OnEnable()
    {
        Debug.Log("OnEnable, subscribe events");
        WordSequencer.wordCompletedEvent += WordCompletedSuccessfully;
        WordSequencer.sequencerFinishedEvent += SequencerFinished;
        WordSequencer.newDictionarySearchResultsEvent += PopulateWordList;
        InputHandler.upKeyPressedEvent += UpKeyPressed;
        InputHandler.downKeyPressedEvent += DownKeyPressed;
        InputHandler.controlKeyPressedEvent += ControlKeyPressed;
    }

    void OnDisable()
    {
        Debug.Log("OnDisable, unsubscribe events");
        WordSequencer.wordCompletedEvent -= WordCompletedSuccessfully;
        WordSequencer.sequencerFinishedEvent -= SequencerFinished;
        WordSequencer.newDictionarySearchResultsEvent -= PopulateWordList;
        InputHandler.upKeyPressedEvent -= UpKeyPressed;
        InputHandler.downKeyPressedEvent -= DownKeyPressed;
        InputHandler.controlKeyPressedEvent -= ControlKeyPressed;
    }

     // Start is called before the first frame update
    void Start()
    {
        //CreateKeyboard();
        CreateWordListButtons();
        Debug.Log("selectedWord on start: " + selectedWord);
    }
}
