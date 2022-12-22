using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


public class InputScript : MonoBehaviour
{

    public GameObject objectToSpawn;

    public GameObject tablePref;

    [SerializeField] GameObject netPrefab;
    [SerializeField] GameObject scoreBoardPrefab;
    [SerializeField] GameObject player;
    GameObject net;
    GameObject ScoreBoard;
    
    [SerializeField] GameObject racketRubber;
    public bool blackUp = false;

    [SerializeField] GameManager gameMang;

    public List<GameObject> tablePoints = new List<GameObject>();

    public GameObject TestObjectInput;

    public bool isEnabled = true; // Wenn Tisch das auf false

    [SerializeField] GameObject Cam;

    public float scaleX;
    public float scaleZ;

    GameObject tableFromMesh;

    public Vector3[] vertices = new Vector3[4];
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

    Vector3[] sortedVertices = new Vector3[4];
    Vector3 previousVertex;

   // [SerializeField] LayerMask PhysikLayerTable; // Wenn zwischen Tisch und Rackt collsion einfach defaukt angeben

    private Vector3 getNearestPoint(Vector3 point)
    {
        Vector3 nearestPoint = new Vector3(100000f, 100000f, 100000f);
        foreach (Vector3 vertex in vertices)
        {
            if (point != vertex && Vector3.Distance(vertex, point) < Vector3.Distance(nearestPoint, point))
            {
                nearestPoint = vertex;
            }
        }
        return nearestPoint;
    }

    private Vector3 getFarthestPoint(Vector3 point)
    {
        Vector3 farthestPoint = new Vector3(0, 0, 0);
        foreach (Vector3 vertex in vertices)
        {
            if (farthestPoint == new Vector3(0, 0, 0) && point != vertex)
            {
                farthestPoint = vertex;
            }
            else if (point != vertex && Vector3.Distance(vertex, point) > Vector3.Distance(farthestPoint, point))
            {
                farthestPoint = vertex;
            }
        }
        return farthestPoint;
    }

    public Vector3[] getNear()
    {
        Vector3 nearestPoint = getNearestPoint(player.transform.position);
        return new Vector3[] { nearestPoint, getNearestPoint(nearestPoint) };
        //return new Vector3[] { vertices[0], vertices[1] };

    }

    public Vector3[] getFar()
    {
        Vector3 farthestPoint = getFarthestPoint(player.transform.position);
        return new Vector3[] { farthestPoint, getNearestPoint(farthestPoint) };
        //return new Vector3[] { vertices[2], vertices[3] };
    }

    public Vector3 getParallel()
    {
        return getNear()[1] - getNear()[0];
        //return vertices[1] - vertices[0];
    }
    public float GetLenght()
    {
        return Vector3.Distance(getNear()[0], getFar()[1]);
    }


    void flipNormalVector()
    {
        Vector3 temp0 = new Vector3(sortedVertices[0].x, sortedVertices[0].y, sortedVertices[0].z);
        Vector3 temp1 = new Vector3(sortedVertices[1].x, sortedVertices[1].y, sortedVertices[1].z);

        sortedVertices[0] = sortedVertices[2];
        sortedVertices[1] = sortedVertices[3];

        sortedVertices[2] = temp0;
        sortedVertices[3] = temp1;

        makeTableFromMesh();
    }
    Vector3 getClosestVertex(Vector3 start)
    {
        Vector3 closestVertex = new Vector3(100000000f, 100000000f, 100000000f);

        foreach (Vector3 vertex in vertices)
        {
            if (vertex != start && vertex != previousVertex)
            {
                if (Mathf.Abs(Vector3.Distance(start, vertex)) < Mathf.Abs(Vector3.Distance(start, closestVertex)))
                {
                    closestVertex = vertex;
                }
            }
        }
        return closestVertex;
    }


    Vector3 getFarthestVertex(Vector3 start)
    {
        Vector3 farthestVertex = start;

        foreach (Vector3 vertex in vertices)
        {
            if (vertex != start && vertex != previousVertex)
            {
                if (Mathf.Abs(Vector3.Distance(start, vertex)) > Mathf.Abs(Vector3.Distance(start, farthestVertex)))
                {
                    farthestVertex = vertex;
                }
            }
        }
        return farthestVertex;
    }

    Vector3 getLastVertex()
    {
        Vector3 lastVertex = sortedVertices[0]; //irgendwas, wird ohnehin �berschrieben, aber sonst weint Visual Studio rum
        foreach (Vector3 vertex in vertices)
        {
            if (!sortedVertices.Contains(vertex))
            {
                lastVertex = vertex;
                break;
            }
        }
        return lastVertex;
    }

    void sortVertices()
    {
        sortedVertices[0] = vertices[0];
        previousVertex = sortedVertices[0];

        sortedVertices[1] = getClosestVertex(sortedVertices[0]);
        previousVertex = sortedVertices[1];

        sortedVertices[2] = getFarthestVertex(sortedVertices[1]);
        previousVertex = sortedVertices[2];

        sortedVertices[3] = getLastVertex();
        previousVertex = sortedVertices[3];
    }

    
    void makeNet()
    {
        if (net)
        {
            Destroy(net);
        }
        if (ScoreBoard)
        {
            Destroy(ScoreBoard);
        }

        Vector3 zeroToOne = sortedVertices[1] - sortedVertices[0]; //Vektor von Vertex 0 nach 1 (obere Kante)
        Vector3 tableCenter = sortedVertices[0] - ((sortedVertices[0] - sortedVertices[3]) / 2); //Mitte des Tisches (bzw zwischen Vertex 0 und 3)
        
        net = Instantiate(netPrefab, tableCenter, Quaternion.identity);
        net.transform.rotation = Quaternion.FromToRotation(Vector3.right, zeroToOne); //Netz parallel zur oberen Kante aufstellen
        net.transform.localScale = new Vector3(zeroToOne.magnitude, 0.125f, 0.01f); //Breite vom Netz der oberen Kante anpassen
        net.transform.position = new Vector3(net.transform.position.x, net.transform.position.y + 0.06f, net.transform.position.z); //Netz ein Stück hochsetzen

        tableCenter.y += 2;
        ScoreBoard = Instantiate(scoreBoardPrefab, tableCenter, Quaternion.identity);
        // net.transform.parent =  ScoreBoard.transform;
        ScoreBoard.transform.rotation = net.transform.rotation;
        
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

        sortVertices();


        Vector3 side1 = vertices[1] - vertices[0];
        Vector3 side2 = vertices[2] - vertices[0];

        Vector3 normal = Vector3.Cross(side1, side2);
            
        if (normal.y < 0f) //Normalenvektor zeigt nach unten
        {
            flipNormalVector();
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        tableFromMesh = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));

        tableFromMesh.GetComponent<MeshFilter>().mesh = mesh;

        tableFromMesh.GetComponent<MeshRenderer>().material = material;

        MeshCollider tableCollider = tableFromMesh.AddComponent(typeof(MeshCollider)) as MeshCollider;

      //  tableFromMesh.layer = PhysikLayerTable; // Funksiniert anscheiden gerade noch nicht
        
        

        makeNet();

        Debug.Log("Table Length: " + Vector3.Distance(sortedVertices[0], sortedVertices[2]));
    }

    IEnumerator makeTableRectangular()
    {
        //vertices = sortedVertices;
        Vector3 zeroToOne = vertices[1] - vertices[0]; //Vektor von Vertex 0 nach 1 (obere Kante)
        Vector3 zeroToTwo = vertices[2] - vertices[0]; //Vektor von Vertex 0 nach 2 (linke Kante)
        Vector3 oneToThree = vertices[3] - vertices[1]; //Vektor von Vertex 1 nach 3 (rechte Kante)

        //Richtet Vertex 3 (untere rechte Ecke) / Winkel bei Vertex 1 (obere rechte Ecke) neue aus
        if (Vector3.Angle(zeroToOne, oneToThree) > 91f)
        {
            while (Vector3.Angle(zeroToOne, oneToThree) > 91f)
            {
                vertices[3] = vertices[3] + (zeroToOne / 100f);
                vertex3.transform.position = vertices[3];
                oneToThree = vertices[3] - vertices[1];
             //   yield return new WaitForEndOfFrame();
            }
        } else if (Vector3.Angle(zeroToOne, oneToThree) < 89f)
        {
            while (Vector3.Angle(zeroToOne, oneToThree) < 89f)
            {
                vertices[3] = vertices[3] - (zeroToOne / 100f);
                vertex3.transform.position = vertices[3];
                oneToThree = vertices[3] - vertices[1];
           //     yield return new WaitForEndOfFrame();
            }
        }

        //Richtet Vertex 2 (untere linke Ecke) / Winkel bei Vertex 0 (obere linke Ecke) neue aus
        if (Vector3.Angle(zeroToOne, zeroToTwo) > 91f)
        {
            while (Vector3.Angle(zeroToOne, zeroToTwo) > 91f)
            {
                vertices[2] = vertices[2] + (zeroToOne / 100f);
                vertex2.transform.position = vertices[2];
                zeroToTwo = vertices[2] - vertices[0];
            //    yield return new WaitForEndOfFrame();
            }
        }
        else if (Vector3.Angle(zeroToOne, zeroToTwo) < 89f)
        {
            while (Vector3.Angle(zeroToOne, zeroToTwo) < 89f)
            {
                vertices[2] = vertices[2] - (zeroToOne / 100f);
                vertex2.transform.position = vertices[2];
                zeroToTwo = vertices[2] - vertices[0];
             //   yield return new WaitForEndOfFrame();
            }
        }

        yield return new WaitForEndOfFrame();

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

    }

    // Update is called once per frame
    void Update()
    {
        
        //// Wurde die Taste ___ auf dem Touch Controller gedr�ckt?
        if (OVRInput.GetDown(OVRInput.Button.Four) && isEnabled)
        {

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
                    StartCoroutine(makeTableRectangular());
                    break;

                case >=5:
                    gameMang.StartGame();
                    break;
                
            }

            buttonPressCounter++;
        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            Debug.Log("Reset");
            SceneManager.LoadScene((SceneManager.GetActiveScene()).name);
        }
        
        //Rotates the racket between forehand and backhand
        if (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger)) {
            if (!blackUp) {
                racketRubber.transform.localEulerAngles = new Vector3(
                    racketRubber.transform.localEulerAngles.x,
                    racketRubber.transform.localEulerAngles.y + 180,
                    racketRubber.transform.localEulerAngles.z - 90);
            } else {
                racketRubber.transform.localEulerAngles = new Vector3(
                    racketRubber.transform.localEulerAngles.x,
                    racketRubber.transform.localEulerAngles.y - 180,
                    racketRubber.transform.localEulerAngles.z + 90);
            }
            blackUp = !blackUp;
        }



        //TODO iwo hier einen Bereich angeben in dem die Targets spawnen k�nnen
    }
}