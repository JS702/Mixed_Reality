using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
  
    public GameObject objectToSpawn;
    public GameObject Controller1;

    public GameObject Table;

    public GameObject[] TablePoints = new GameObject[2];

    // Update is called once per frame
    void Update()
    {
        // Wurde die Taste ___ auf dem Touch Controller gedrückt?
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && TablePoints.Length < 2)
        {
           GameObject AnkerTable =  Instantiate(objectToSpawn, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),Quaternion.identity);//Controller1.transform
           TablePoints[TablePoints.Length] = AnkerTable;
           if(TablePoints.Length == 2)
           {
                MakeTable();
           }
        }
        
    }
    public void MakeTable()
    {
        GameObject createdTable = Instantiate(Table, TablePoints[0].transform.position - TablePoints[1].transform.position, Quaternion.identity);
        // nicht vollstädnig
    }
}

