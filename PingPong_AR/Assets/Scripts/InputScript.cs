using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
  
    public GameObject objectToSpawn;
    public GameObject controller;

    public GameObject table;

    public GameObject[] tablePoints = new GameObject[2];

    // Update is called once per frame
    void Update()
    {
        // Wurde die Taste ___ auf dem Touch Controller gedrückt?
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && tablePoints.Length < 2)
        {
           GameObject AnkerTable =  Instantiate(objectToSpawn, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),Quaternion.identity);//Controller1.transform
           tablePoints[tablePoints.Length] = AnkerTable;
           if(tablePoints.Length == 2)
           {
                MakeTable();
           }
        }
        
    }
    public void MakeTable()
    {
        GameObject createdTable = Instantiate(table, tablePoints[0].transform.position - tablePoints[1].transform.position, Quaternion.identity);
        // nicht vollständig
        //Bin mir nicht sicher ob die Rotation passt,  die Mitte der Tisches ist ja einfach nur auf der Mitte des echten Tisches, aber die Ecken müssen nicht übereinstimmen
    }

    //TODO iwo hier einen Bereich angeben in dem die Targets spawnen können
}

