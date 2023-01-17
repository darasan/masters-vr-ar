using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class InputHandler : MonoBehaviour
{
    
    public InputAction playerControls;
    public GameObject panel;
    //private ButtonManager btn_mgr = null;

    public static event Action leftKeyPressedEvent;
    public static event Action rightKeyPressedEvent;
    public static event Action upKeyPressedEvent;
    public static event Action downKeyPressedEvent;
    public static event Action spaceKeyPressedEvent;

    private void OnEnable() => playerControls.Enable();
    private void OnDisable() => playerControls.Disable();


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start()");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnLeftKeyPressed()
    {
        Debug.Log("OnLeftKeyPressed");
        leftKeyPressedEvent.Invoke();
    }

    void OnRightKeyPressed()
    {
        Debug.Log("OnRightKeyPressed");
        rightKeyPressedEvent.Invoke();
    }

    void OnUpKeyPressed()
    {
        Debug.Log("OnUpKeyPressed");
        upKeyPressedEvent.Invoke();
    }

    void OnDownKeyPressed()
    {
        Debug.Log("OnDownKeyPressed");
        downKeyPressedEvent.Invoke();
    }

    void OnSpaceKeyPressed()
    {
        Debug.Log("OnSpaceKeyPressed");
        spaceKeyPressedEvent.Invoke();
    }

    void OnA_press()
    {
       // Debug.Log("OnAPress");
       // btn_mgr = panel.GetComponent<ButtonManager>();
       // btn_mgr.OnButtonClick(0);
    }

    void OnB_press()
    {
       // btn_mgr = panel.GetComponent<ButtonManager>();
       // btn_mgr.OnButtonClick(1); 
    }

    void OnC_press()
    {
       // btn_mgr = panel.GetComponent<ButtonManager>();
       // btn_mgr.OnButtonClick(2); 
    }

}
