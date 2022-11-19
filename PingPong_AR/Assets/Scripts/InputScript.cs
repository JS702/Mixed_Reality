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

    [SerializeField] GameObject Cam;

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
    public void MakeTablePref() // Notiz: Zu Roation Probleme , Einbauen das man beim ersten die Roation irgentwie bestimmt (Kontrollewr so halten, Knopf) Dann Roation auf zweiten Würfel auch (konistenz) und dann zum Spawn auf den Tisch !!!!
    {
         Debug.Log("StartTableX");
         Vector3 spawnPoint = (tablePoints[1].transform.position + tablePoints[0].transform.position) /2f;
         spawnPoint.y = tablePoints[0].transform.position.y;
         Debug.Log(spawnPoint +"" + tablePoints[0].transform.position +"" + tablePoints[1].transform.position);

         GameObject Table = Instantiate(tablePref, spawnPoint, Quaternion.identity);

         float scaleX = Mathf.Abs(tablePoints[1].transform.position.x - tablePoints[0].transform.position.x);
         float scaleZ = Mathf.Abs(tablePoints[1].transform.position.z - tablePoints[0].transform.position.z);


         Table.transform.localScale = new Vector3(scaleX, 0.02f, scaleZ);



        Vector3 NearsAnker;

        Vector3 FarAnker;

        

        if (Vector3.Distance(Cam.transform.position, tablePoints[0].transform.position) < Vector3.Distance(Cam.transform.position, tablePoints[1].transform.position))
        {
            NearsAnker = tablePoints[0].transform.position;
            FarAnker = tablePoints[1].transform.position; 
        }
        else
        {
            NearsAnker = tablePoints[1].transform.position;
            FarAnker = tablePoints[0].transform.position;
        }

        //Vector3 SecoundPoint;
        Vector3 Corn1 = new Vector3(FarAnker.x, NearsAnker.y, NearsAnker.z);
        Vector3 Corn2 = new Vector3(NearsAnker.x, NearsAnker.y, FarAnker.z);

        // Other Test
        Vector3 TestNearVeckor = NearsAnker;
        TestNearVeckor.y += 0.1f;
        GameObject testObject0 = Instantiate(objectToSpawn, TestNearVeckor, Quaternion.identity);

        //testObject 
        GameObject testObject1 = Instantiate(objectToSpawn,Corn1,Quaternion.identity);
        GameObject testObject2 = Instantiate(objectToSpawn, Corn2, Quaternion.identity);
        

        Vector3[] NearSide;
        Vector3[] FarSide;
        if (Vector3.Distance(Cam.transform.position, Corn1) < Vector3.Distance(Cam.transform.position, Corn2))
        {
           // SecoundPoint = Corn1;
            NearSide = new Vector3[]{ NearsAnker, Corn1 };
            FarSide = new Vector3[] { FarAnker, Corn2 };
        }
        else
        {
            //  SecoundPoint = Corn2;
            NearSide = new Vector3[] { NearsAnker, Corn2 };
            FarSide = new Vector3[] { FarAnker, Corn1 };
        }

        Vector3 TestStuff= NearSide[1];
        TestStuff.y += 0.1f;
        Instantiate(objectToSpawn, TestStuff, Quaternion.identity);





        Vector3 BallPos = (FarSide[1] + FarSide[0] ) / 2;
        BallPos.y += 0.45f;
        Vector3 dir = BallPos - Table.transform.position;
      //  dir.y = 0;

        Instantiate(objectToSpawn, BallPos, Quaternion.identity);


        FindObjectOfType<BallSpawner>().Relocate(BallPos,dir*-1);

        FindObjectOfType<GameManager>().SpawnTarget(FarSide, NearSide); // Wenn GameManger mehr als einer benutzt wird es in einen Feld packen 


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

