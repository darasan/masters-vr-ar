using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    
    public InputAction playerControls;
    public GameObject panel;

    private void OnEnable() => playerControls.Enable();
    private void OnDisable() => playerControls.Disable();

    private ButtonManager btn_mgr = null;


    // Start is called before the first frame update
    void Start()
    {
         Debug.Log("Start()");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnA_press()
    {
        //Debug.Log("Heyyyy");
        btn_mgr = panel.GetComponent<ButtonManager>();
        btn_mgr.OnButtonClick(0); //"next 0 just add binding for B,C etc. Cant avoid I think."
    }

    void OnSpace()
    {
        Debug.Log("Space");
    }

}
