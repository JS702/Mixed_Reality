using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
  
    public GameObject objectToSpawn;
    public GameObject controller;

    public GameObject table;

    public GameObject[] tablePoints = new GameObject[2];

    public void MakeTable(Vector3 firstPoint, Vector3 secondPoint)
    {
        public GameObject Table = new GameObject("Table");
        MeshFilter mf= Table.AddComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer mr= Table.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        Mesh m = new Mesh();
        m.vertices= new Vector3[]
        {
        new Vector3(firstPoint.x,firstPoint.y,firstPoint.z)
        new Vector3(firstPoint.x,firstPoint.y,secondPoint.z)
        new Vector3(secondPoint.x,firstPoint.y,secondPoint.z)
        new Vector3(secondPoint.x,firstPoint.y,firstPoint.z)
        }
        m.uv = new Vector2[]
        {
        new Vector2(0,0)
        new Vector2(1,0)
        new Vector2(0,1)
        new Vector2(1,1)
        }
        m.triangles = new int[]{0,1,2,0,2,3};
        mf.mesh =m;
        m.RecalculateBounds();
        m.RecalculateNormals();
    }

    // Update is called once per frame
    void Update()
    {
        // Wurde die Taste ___ auf dem Touch Controller gedr�ckt?
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && tablePoints.Length < 2)
        {
           GameObject AnkerTable =  Instantiate(objectToSpawn, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch),Quaternion.identity);//Controller1.transform
           tablePoints[tablePoints.Length] = AnkerTable;
           if(tablePoints.Length == 2)
           {
                //Notiz an Nicolas: erstmal gucken, ob der cude überhaupt spawned
                //MakeTable(TablePoints[0],TablePoints[1]);
           }
        }
        
    }
    public void MakeTable()
    {
        GameObject createdTable = Instantiate(table, tablePoints[0].transform.position - tablePoints[1].transform.position, Quaternion.identity);
        // nicht vollst�ndig
        //Bin mir nicht sicher ob die Rotation passt,  die Mitte der Tisches ist ja einfach nur auf der Mitte des echten Tisches, aber die Ecken m�ssen nicht �bereinstimmen
    }

    //TODO iwo hier einen Bereich angeben in dem die Targets spawnen k�nnen
}

