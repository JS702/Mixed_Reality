using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int score = 0;
    public GameObject target;
    public GameObject targetPrefab;
    public Vector3[] site1; // Die zwei Ecken von einer Seite; müssen im Input übergeben werden
    public Vector3[] site2;
    public GameObject player;
    public GameObject ballSpawner;
    // Start is called before the first frame update
    void Start()
    {
        SpawnTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if(!target && OVRInput.GetDown(OVRInput.Button.Two))
        {
            SpawnTarget();
        }
    }

    public void HitTarget()
    {
        Destroy(target);
        score++;
    }

    public void SpawnTarget()
    {
        /*
        Vector3[] site = Vector3.Distance(player.transform.position, site1[0]) > Vector3.Distance(player.transform.position, site2[0]) ? site1 : site2;
        Vector3 farthestPoint = site[0].magnitude > site[1].magnitude ? site[0] : site[1];
        Vector3 nearestPoint = site[0].magnitude > site[1].magnitude ? site[1] : site[0];

        Vector3 range = (farthestPoint - nearestPoint) * Random.value;
        range.y = 0.5f; // Target können auch höher spawnen
        target = Instantiate(targetPrefab, (farthestPoint + range), Quaternion.identity); // TODO rotation anpassen
        */
        target = Instantiate(targetPrefab, new Vector3(-1,1,2), Quaternion.identity);
    }


}
