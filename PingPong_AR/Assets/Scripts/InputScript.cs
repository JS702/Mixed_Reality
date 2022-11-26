using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputScript : MonoBehaviour
{

    public GameObject objectToSpawn;
   // public GameObject controller;

    public GameObject tablePref;

    [SerializeField] GameObject netPrefab;

    public List<GameObject> tablePoints = new List<GameObject>();

    public GameObject TestObjectInput;

    public bool isEnabled = true; // Wenn Tisch das auf false

    [SerializeField] GameObject Cam;

    public float scaleX;
    public float scaleZ;

    GameObject Table;
    GameObject tableFromMesh;

    Vector3[] vertices = new Vector3[4];
    Vector2[] uv = new Vector2[4];
    int[] triangles = new int[6];

    [SerializeField] Material material;

    int buttonPressCounter;

    [SerializeField] GameObject testCube;

    float yPosition;

    Vector3 controllerPosition;

    GameObject vertex0;
    GameObject vertex1;
    GameObject vertex2;
    GameObject vertex3;

    GameObject ballSpawnerCube;

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

        //GameObject Table = Instantiate(tablePref, spawnPoint, Quaternion.identity);

        Table = Instantiate(tablePref, spawnPoint, Quaternion.identity);

        //float scaleX = Mathf.Abs(tablePoints[1].transform.position.x - tablePoints[0].transform.position.x);
        //float scaleZ = Mathf.Abs(tablePoints[1].transform.position.z - tablePoints[0].transform.position.z);

        scaleX = Mathf.Abs(tablePoints[1].transform.position.x - tablePoints[0].transform.position.x);
        scaleZ = Mathf.Abs(tablePoints[1].transform.position.z - tablePoints[0].transform.position.z);


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

    void makeNet()
    {
        GameObject net = Instantiate(netPrefab, Table.transform.position, Quaternion.identity);
        net.transform.localScale = new Vector3(scaleX + 0.0001f, 0.125f, 0.01f);
        net.transform.position = new Vector3(net.transform.position.x, net.transform.position.y + 0.06f, net.transform.position.z);
    }

    void makeTableFromMesh()
    {
        Mesh mesh = new Mesh();

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        tableFromMesh = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));

        tableFromMesh.GetComponent<MeshFilter>().mesh = mesh;

        tableFromMesh.GetComponent<MeshRenderer>().material = material;

        MeshCollider tableCollider = tableFromMesh.AddComponent(typeof(MeshCollider)) as MeshCollider;

        Vector3 ballSpawnerPosition = vertices[0] + ((vertices[1] - vertices[0]) / 2);
        ballSpawnerPosition.y += 0.45f;

        FindObjectOfType<BallSpawner>().Relocate(ballSpawnerPosition, vertices[3] - ballSpawnerPosition);

        if (ballSpawnerCube)
        {
            Destroy(ballSpawnerCube);
        }

        ballSpawnerCube = Instantiate(testCube, ballSpawnerPosition, Quaternion.identity);
        Destroy(ballSpawnerCube.GetComponent<BoxCollider>());
    }

    IEnumerator makeTableRectangular()
    {
        Vector3 zeroToOne = vertices[1] - vertices[0]; //Vektor von Vertex 0 nach 1 (obere Kante)
        Vector3 zeroToTwo = vertices[2] - vertices[0]; //Vektor von Vertex 0 nach 2 (linke Kante)
        Vector3 oneToThree = vertices[3] - vertices[1]; //Vektor von Vertex 1 nach 3 (rechte Kante)

        //Richtet Vertex 3 (untere rechte Ecke) / Winkel bei Vertex 1 (obere rechte Ecke) neue aus
        if (Vector3.Angle(zeroToOne, oneToThree) > 91f)
        {
            Debug.Log("Fixing Vertex 3 / Angle at Vertex 1...");
            while (Vector3.Angle(zeroToOne, oneToThree) > 91f)
            {
                vertices[3] = vertices[3] + (zeroToOne / 100f);
                vertex3.transform.position = vertices[3];
                oneToThree = vertices[3] - vertices[1];
                yield return new WaitForSeconds(0.0001f);
            }
        } else if (Vector3.Angle(zeroToOne, oneToThree) < 89f)
        {
            Debug.Log("Fixing Vertex 3 / Angle at Vertex 1...");
            while (Vector3.Angle(zeroToOne, oneToThree) < 89f)
            {
                vertices[3] = vertices[3] - (zeroToOne / 100f);
                vertex3.transform.position = vertices[3];
                oneToThree = vertices[3] - vertices[1];
                yield return new WaitForSeconds(0.0001f);
            }
        }
        Debug.Log("Finished fixing Vertex 3 / Angle at Vertex 1");

        //Richtet Vertex 2 (untere linke Ecke) / Winkel bei Vertex 0 (obere linke Ecke) neue aus
        if (Vector3.Angle(zeroToOne, zeroToTwo) > 91f)
        {
            Debug.Log("Fixing Vertex 2 / Angle at Vertex 0...");
            while (Vector3.Angle(zeroToOne, zeroToTwo) > 91f)
            {
                vertices[2] = vertices[2] + (zeroToOne / 100f);
                vertex2.transform.position = vertices[2];
                zeroToTwo = vertices[2] - vertices[0];
                yield return new WaitForSeconds(0.0001f);
            }
        }
        else if (Vector3.Angle(zeroToOne, zeroToTwo) < 89f)
        {
            Debug.Log("Fixing Vertex 2 / Angle at Vertex 0...");
            while (Vector3.Angle(zeroToOne, zeroToTwo) < 89f)
            {
                vertices[2] = vertices[2] - (zeroToOne / 100f);
                vertex2.transform.position = vertices[2];
                zeroToTwo = vertices[2] - vertices[0];
                yield return new WaitForSeconds(0.0001f);
            }
        }
        Debug.Log("Finished fixing Vertex 2 / Angle at Vertex 0...");
        Debug.Log("New Angles: " + Vector3.Angle(zeroToOne, oneToThree) + ", " + Vector3.Angle(zeroToOne, zeroToTwo));

        //Vermittelt den Abstand von Vertex 2 und 3 zur oberen Kante (Kante zwischen Vertex 0 und 1)
        float difference = Mathf.Abs(zeroToTwo.magnitude - oneToThree.magnitude); //Unterschied der Entfernung von Vertex 2 und 3 zur oberen Kante
        Vector3 differenceToMove = Vector3.Normalize(zeroToTwo) * (difference / 2); //Vektor, um den Vertex 1 und 2 bewegt werden müssen, um den Abstand zu vermitteln

        if (Mathf.Abs(zeroToTwo.magnitude) > Mathf.Abs(oneToThree.magnitude))
        {
            vertices[2] = vertices[2] - differenceToMove;
            vertices[3] = vertices[3] + differenceToMove;
        } else
        {
            vertices[2] = vertices[2] + differenceToMove;
            vertices[3] = vertices[3] - differenceToMove;
        }

        vertex2.transform.position = vertices[2];
        vertex3.transform.position = vertices[3];

        Destroy(tableFromMesh);
        makeTableFromMesh();

        Debug.Log("Table is now rectangular");
    }

    // Update is called once per frame
    void Update()
    {
        
        //// Wurde die Taste ___ auf dem Touch Controller gedr�ckt?
        if (OVRInput.GetDown(OVRInput.Button.Four) && isEnabled)
        {
            /*
            if (tablePoints.Count >= 2)
            {
                Debug.Log("StartTable");
                isEnabled = false;
                MakeTablePref();
                //MakeTable(tablePoints[1].transform.position, tablePoints[0].transform.position);
                makeNet();

            }
            else
            {
                GameObject AnkerTable = Instantiate(objectToSpawn, OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch), Quaternion.identity);//Controller1.transform // 
                tablePoints.Add(AnkerTable);
                Debug.Log("Pressed" + tablePoints.Count);
            }
            */

            switch(buttonPressCounter)
            {
                
                case 0:
                    controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                    vertices[0] = controllerPosition;
                    yPosition = controllerPosition.y;
                    vertex0 = Instantiate(testCube, vertices[0], Quaternion.identity);
                    break;

                case 1:
                    controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                    vertices[1] = new Vector3(controllerPosition.x, yPosition, controllerPosition.z);
                    vertex1 = Instantiate(testCube, vertices[1], Quaternion.identity);
                    break;

                case 2:
                    controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                    vertices[2] = new Vector3(controllerPosition.x, yPosition, controllerPosition.z);
                    vertex2 = Instantiate(testCube, vertices[2], Quaternion.identity);
                    break;

                case 3:
                    controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                    vertices[3] = new Vector3(controllerPosition.x, yPosition, controllerPosition.z);
                    vertex3 = Instantiate(testCube, vertices[3], Quaternion.identity);
                    break;

                case 4:
                    makeTableFromMesh();
                    break;

                case 5:
                    StartCoroutine(makeTableRectangular());
                    break;
                
            }

            buttonPressCounter++;




        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            Debug.Log("Reset");
            SceneManager.LoadScene((SceneManager.GetActiveScene()).name);
        }



        //TODO iwo hier einen Bereich angeben in dem die Targets spawnen k�nnen
    }
}

