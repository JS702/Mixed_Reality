using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsoleInGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;

    }
    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;

    }
    void HandleLog(string logString,string stackTrace,LogType type)
    {
        if(type == LogType.Error)
        {
            txt.text += '\n' + "Error: "+ logString;
        }
        else
        {
            txt.text += '\n'  +"Other: "+ logString;
        }
    }
       
}
