using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevConsole : MonoBehaviour
{
    public Canvas canvas;
    public TMP_Text text;
    public static DevConsole instance;
    string myLog = "*begin log";
    bool doShow = false;
    int line = 1;
    void OnEnable() { Application.logMessageReceived += Log; }
    void OnDisable() { Application.logMessageReceived -= Log; }
    void Update() { if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.C)) { doShow = !doShow; } }

    private void Awake() 
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }
    public void Log(string logString, string stackTrace, LogType type)
    {
       
        myLog = myLog + "\n"+ line + " - " + logString;
        line++;
   }
   void OnGUI()
    {
        if (!doShow) { canvas.enabled = false; }
        else
        {
            canvas.enabled = true;
            text.text = myLog;
        }
        
    }
}
