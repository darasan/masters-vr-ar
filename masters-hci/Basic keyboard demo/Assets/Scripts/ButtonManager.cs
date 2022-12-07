using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
//using UnityEngine.UIElements;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Button[] buttons;
    GameObject[] buttonsGO;

    //public Button testBtn;

    private string[] letters = 
    {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", 
     "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

    public void OnA_clicked()
    {
        Debug.Log("A clicked! send btn event");

        //using (var e = new NavigationSubmitEvent() { target = testBtn } )
        //testBtn.SendEvent(e);
        //testBtn.onClick.Invoke();

        // buttons[0].onClick.Invoke(); "works but doesnt change colour. separate event. will need to just set colour separately? want at least basic working for now, to see easily what key pressed"
        // "good, check docs again...  "

        /* var go = buttonsGO[0];
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

        var go = buttonsGO[id];
        var ped = new PointerEventData(EventSystem.current);
       // ExecuteEvents.Execute(go, ped, ExecuteEvents.pointerEnterHandler);
       // ExecuteEvents.Execute(go, ped, ExecuteEvents.submitHandler); 
        
        //"crashes unity? check. if works, just add input mapping for A-Z in inputHandler, cant avoid. then just this code here"
        //"also could use dict to map 0-26 to A-Z, cleaner and works both directions"


    }

    void CreateButtons()
    {
        buttonsGO = new GameObject[26];
        

        float padding = 35.0f; //padding from left side of panel

        for(int i=0; i<buttons.Length; i++){

            int index = i; // Prevents the closure problem: https://stackoverflow.com/questions/271440/captured-variable-in-a-loop-in-c-sharp

            //Init button from prefab
            GameObject btn = Instantiate(buttonPrefab); //"setting pos but not correct - understand how to do. remember relative to parent"

            //Store GO
            buttonsGO[index] = btn;

            //Set position
            btn.transform.SetParent(this.transform); //set this panel as parent, else will instantiate at top level in hierarchy
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


     // Start is called before the first frame update
    void Start()
    {
        CreateButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
