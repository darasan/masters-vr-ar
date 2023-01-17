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
    public GameObject scrollViewContent; //TODO could get from scrollView above but make it work for now
    public GameObject inputField; //Main text input field
    public Button startButton;

    public static event Action startButtonClickedEvent;

    GameObject[] keys; //keyboard keys
    GameObject[] wordList; //word buttons
    int selectedWord = 0; //index of currently selected word in list
    Color darkGreen = new Color(0.29f, 0.74f, 0.35f, 1.0f);

    //Good comparison of events systems: https://gamedevbeginner.com/events-and-delegates-in-unity/


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

    public void OnA_clicked()
    {
        Debug.Log("A clicked! send btn event");

        //using (var e = new NavigationSubmitEvent() { target = testBtn } )
        //testBtn.SendEvent(e);
        //testBtn.onClick.Invoke();

        // buttons[0].onClick.Invoke(); "works but doesnt change colour. separate event. will need to just set colour separately? want at least basic working for now, to see easily what key pressed"
        // "good, check docs again...  "

        /* var go = keys[0];
        var ped = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(go, ped, ExecuteEvents.pointerEnterHandler);
        ExecuteEvents.Execute(go, ped, ExecuteEvents.submitHandler); */
    }

    public void OnB_clicked()
    {
        Debug.Log("B clicked!");
    }

    public void OnC_clicked()
    {
        Debug.Log("C clicked!");
    }

    public void OnD_clicked()
    {
        Debug.Log("D clicked!");
    }

    public void OnE_clicked()
    {
        Debug.Log("E clicked!");
    }

    public void OnTestBtn_clicked()
    {
        Debug.Log("Test btn clicked!");
    }

    public void OnButtonClick(int id)
    {
        // The id is the text of the button.
        Debug.Log("Clicked: " + id.ToString());
       
       // "next - add small jump/scale on key press so looks nice. If want to change highlight colour, do in prefab"
       // "can use id received here to index button array, then set transform/scale. May need lerp thing later, just jump to vals for now"
       // "then connect keyboard input. May use buttonDown func if not working on first touch"

        var go = keys[id];
        var ped = new PointerEventData(EventSystem.current);
       // ExecuteEvents.Execute(go, ped, ExecuteEvents.pointerEnterHandler);
       // ExecuteEvents.Execute(go, ped, ExecuteEvents.submitHandler); 
        
        //"crashes unity? check. if works, just add input mapping for A-Z in inputHandler, cant avoid. then just this code here"
        //"also could use dict to map 0-26 to A-Z, cleaner and works both directions"

    }

    void MoveToNextWord()
    {
        // Reset the currently selected button to the default colour.
        Image wordImage = wordList[selectedWord].GetComponentInChildren<Image>();
        wordImage.color = Color.white;
        // Increment our selected button index by 1.
        selectedWord++;
        // Check that our new index does not move outside of our array.
        if(selectedWord >= wordList.Length)
        {
            // If you want to reset to the first button, reset the index.
            selectedWord = 0;
            // If you do not, simply move it back by 1, instead.
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

        inputfield.text = wordText; //+= to concat words. Not doing now, just set one word so not added
        inputfield.MoveToEndOfLine(false, true); //move caret to end: https://forum.unity.com/threads/move-cursor-to-end-of-text.530903/#post-3525370
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
            TextMeshProUGUI tmp_ugui = btn.GetComponentInChildren<TextMeshProUGUI>(); //Use GetCompInChildren when in hierarchy below (child), else wont find. Think GetComp only for object assigned in inspector directly
            tmp_ugui.text = letters[index];                                           //Could set Text field public and assign in inspector, but more messy (less coded)

            //Debug.Log("Letter:");
            //Debug.Log(letters[index]);

            //Assign click callback
            buttons[index].onClick.AddListener(() => OnButtonClick(index));
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

            //wordList[index].action = PlayButtonAction; disable for now, not button so no action. may not need, just execute func depending on selected word (e,g copy to text field)
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

    void SpaceKeyPressed()
    {
        CopySelectedWordToInputField();
    }

    void OnEnable()
    {
        Debug.Log("OnEnable, subscribe events");
        WordSequencer.wordCompletedEvent += WordCompletedSuccessfully;
        WordSequencer.sequencerFinishedEvent += SequencerFinished;
        WordSequencer.newDictionarySearchResultsEvent += PopulateWordList;
        InputHandler.upKeyPressedEvent += UpKeyPressed;
        InputHandler.downKeyPressedEvent += DownKeyPressed;
        InputHandler.spaceKeyPressedEvent += SpaceKeyPressed;
    }

    void OnDisable()
    {
        Debug.Log("OnDisable, unsubscribe events");
        WordSequencer.wordCompletedEvent -= WordCompletedSuccessfully;
        WordSequencer.sequencerFinishedEvent -= SequencerFinished;
        WordSequencer.newDictionarySearchResultsEvent -= PopulateWordList;
        InputHandler.upKeyPressedEvent -= UpKeyPressed;
        InputHandler.downKeyPressedEvent -= DownKeyPressed;
        InputHandler.spaceKeyPressedEvent -= SpaceKeyPressed;
    }

     // Start is called before the first frame update
    void Start()
    {
        CreateKeyboard();
        CreateWordListButtons();
        Debug.Log("selectedWord on start: " + selectedWord);
    }
}
