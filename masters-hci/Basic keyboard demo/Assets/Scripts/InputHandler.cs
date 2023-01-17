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
    public static event Action controlKeyPressedEvent;
    public static event Action backspaceKeyPressedEvent;

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
        leftKeyPressedEvent.Invoke();
    }

    void OnRightKeyPressed()
    {
        rightKeyPressedEvent.Invoke();
    }

    void OnUpKeyPressed()
    {
        upKeyPressedEvent.Invoke();
    }

    void OnDownKeyPressed()
    {
        downKeyPressedEvent.Invoke();
    }

    void OnControlKeyPressed()
    {
        controlKeyPressedEvent.Invoke();
    }

     void OnBackspaceKeyPressed()
    {
        backspaceKeyPressedEvent.Invoke();
    }
}
