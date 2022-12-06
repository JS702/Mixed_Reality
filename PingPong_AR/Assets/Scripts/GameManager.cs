using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int score = 0;
    public GameObject target;
    public GameObject targetPrefab;
    public Vector3[] site1; // Die zwei Ecken von einer Seite; m�ssen im Input �bergeben werden
    public Vector3[] site2;
    public GameObject player;
    public GameObject ballSpawner;



    // Start is called before the first frame update
    void Start()
    {
       // SpawnTargetSimple();
    }

    // Update is called once per frame
    void Update()
    {
        if(!target && OVRInput.GetDown(OVRInput.Button.Two))
        {
           // SpawnTargetSimple();
        }
    }

    public void HitTarget()
    {
        Destroy(target);
        score++;
    }

    public void SpawnTargetSimple()
    {
        target = Instantiate(targetPrefab, new Vector3(-1,1,2), Quaternion.identity);
    }
    public void SetSite(Vector3 pointA,Vector3 pointB)
    {
        site1[0] = pointA;
        site1[1] = pointB;

    }
    public void SpawnTarget(Vector3[] Near, Vector3[] far, Vector3 parallel)
    {

        if (target)
        {
            Destroy(target);
            //Debug.Log("Target destroyed. Cause: New Target");
        }

        //target = Instantiate(targetPrefab, (farthestPoint + range), Quaternion.identity); // TODO rotation anpassen
        Vector3 targetPositionX = Near[0] + ((Near[1] - Near[0]) * Random.Range(0f, 1f)); //0: linke Seite | 1: rechte Seite
        Vector3 targetPositionY = new Vector3(0, 0, 0); //0: genau auf der Platte | >0: irgendwo �ber der Platte
        Vector3 targetPositionZ = (far[0] - Near[0]) * Random.Range(0f, 0.45f); //0: Ende wo der Ball spawnt | 1: Ende wo der Ball nicht spawnt
        Vector3 targetPosition = targetPositionX + targetPositionY + targetPositionZ;

        target = Instantiate(targetPrefab, targetPosition, Quaternion.FromToRotation(Vector3.right, parallel)); // TODO rotation anpassen
    }


}
