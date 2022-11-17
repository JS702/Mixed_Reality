using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputScript : MonoBehaviour
{

    public GameObject objectToSpawn;
   // public GameObject controller;

    public GameObject tablePref;

    public List<GameObject> tablePoints = new List<GameObject>();

    public GameObject TestObjectInput;

    public bool isEnabled = true; // Wenn Tisch das auf false

    private void Start()
    {
       
    }

    public void MakeTable(Vector3 firstPoint, Vector3 secondPoint)
    {
        GameObject Table = new GameObject("Table");
        MeshFilter mf = Table.AddComponent(typeof(MeshFilter)) as MeshFilter;
        MeshRenderer mr = Table.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        Mesh m = new Mesh();
        m.vertices = new Vector3[4] {

        //all vectors take the height of the first point -> y1
        new Vector3(firstPoint.x, firstPoint.y, firstPoint.z),// x1,y1,z1
        new Vector3(firstPoint.x, firstPoint.y, secondPoint.z), // x1,y1,z2
        new Vector3(secondPoint.x, firstPoint.y, secondPoint.z), //x2,y1,z2
        new Vector3(secondPoint.x, firstPoint.y, firstPoint.z)// x2,y1,z1
                                                               };


        m.uv = new Vector2[4] {


            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };


        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        mf.mesh = m;
        m.RecalculateBounds();
        m.RecalculateNormals();
    }
    public void MakeTablePref()
    {
         Debug.Log("StartTableX");
         Vector3 spawnPoint = (tablePoints[1].transform.position + tablePoints[0].transform.position) /2f;
         spawnPoint.y = tablePoints[0].transform.position.y;
         Debug.Log(spawnPoint +"" + tablePoints[0].transform.position +"" + tablePoints[1].transform.position);

         GameObject Table = Instantiate(tablePref, spawnPoint, Quaternion.identity);

         float scaleX = Mathf.Abs(tablePoints[1].transform.position.x - tablePoints[0].transform.position.x);
         float scaleZ = Mathf.Abs(tablePoints[1].transform.position.z - tablePoints[0].transform.position.z);


         Table.transform.localScale = new Vector3(scaleX, 0.02f, scaleZ);
        

        // if (ScaleX < ScaleZ) und darüber dann besetimmen was die Kurse Kante ist , dann Per Inferung messen welche Kannte bzw Punkt weiter weg von Spiler ist , Aber der kannte wird Ziel und Ball Wurf erscheiden[Pre Implentation Notiz]

        
    }



    // Update is called once per frame
    void Update()
    {
        
        //// Wurde die Taste ___ auf dem Touch Controller gedr�ckt?
        if (OVRInput.GetDown(OVRInput.Button.Four) && isEnabled)
        {
            if (tablePoints.Count >= 2)
            {
                Debug.Log("StartTable");
                isEnabled = false;
                MakeTablePref();
                //MakeTable(tablePoints[1].transform.position, tablePoints[0].transform.position);

            }
            else
            {
                GameObject AnkerTable = Instantiate(objectToSpawn, OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch), Quaternion.identity);//Controller1.transform // 
                tablePoints.Add(AnkerTable);
                Debug.Log("Pressed" + tablePoints.Count);
            }
            

     
           
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            Debug.Log("Reset");
            SceneManager.LoadScene((SceneManager.GetActiveScene()).name);
        }



        //TODO iwo hier einen Bereich angeben in dem die Targets spawnen k�nnen
    }
}

