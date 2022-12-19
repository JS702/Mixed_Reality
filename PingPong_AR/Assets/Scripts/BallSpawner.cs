using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;

    [SerializeField] GameObject ballPrefab;

    public Vector3 shootDirection;

    public bool isEnabled;//Vllt removen wenn der nix anderes macht außer sachen spawnen

    public float shootSpeed;

    GameManager gameManager;
    [SerializeField] GameObject inputGuy;
    InputScript inputScript;
    RacketScript racketScript;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        inputScript = inputGuy.GetComponent<InputScript>();
        racketScript = FindObjectOfType<RacketScript>();
        shootDirection = Vector3.forward;
    }

    void Update()
    {
        if (isEnabled && OVRInput.GetDown(OVRInput.Button.One))//TODO gucken welcher Knopf und ob der immer auslöst oder unter bestimmten Bedingungen
        {
            SpawnBallProtocol(false);
        }
    }

    public void SpawnBallProtocol(bool waitCont)
    {
        racketScript.hitBall = false;
        Vector3[] a = inputScript.getFar();
        Vector3[] b = inputScript.vertices;
        gameManager.SpawnTarget(a, inputScript.getNear(), false);//TODO manchmal moving = true setzen, vllt bei jedem 5 ball oder so
        if (ball)
        {
            Destroy(ball);
        }
        ball = Instantiate(ballPrefab, transform);
        ball.GetComponent<Rigidbody>().AddRelativeForce(shootDirection * shootSpeed);
        if (waitCont)
        {
            ball.GetComponent<BallScript>().GameOnMode();
        }
    }
}
