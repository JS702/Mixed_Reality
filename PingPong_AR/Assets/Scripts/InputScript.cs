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
        //all vectors take the height of the first point -> y1
        new Vector3(firstPoint.x,firstPoint.y,firstPoint.z) // x1,y1,z1
        new Vector3(firstPoint.x,firstPoint.y,secondPoint.z) // x1,y1,z2
        new Vector3(secondPoint.x,firstPoint.y,secondPoint.z) //x2,y1,z2
        new Vector3(secondPoint.x,firstPoint.y,firstPoint.z)// x2,y1,z1
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

    //TODO iwo hier einen Bereich angeben in dem die Targets spawnen k�nnen
}

