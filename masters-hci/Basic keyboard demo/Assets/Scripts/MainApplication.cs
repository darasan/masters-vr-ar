using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Top level script for controlling the application

public class MainApplication : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Note that had to add script to parent object canvas, then drag canvas/script component onto click event, to see functions. Otherwise didnt show
    public void Quit()
    {
        Application.Quit(); 
    }
}
