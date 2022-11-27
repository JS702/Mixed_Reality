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
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); // 
        inputScript = inputGuy.GetComponent<InputScript>();
        racketScript = FindObjectOfType<RacketScript>();
    }

    void Update()
    {
        if (isEnabled && OVRInput.GetDown(OVRInput.Button.One))//TODO gucken welcher Knopf und ob der immer auslöst oder unter bestimmten Bedingungen
        {
            racketScript.hitBall = false;
            gameManager.SpawnTarget(inputScript.getNear(), inputScript.getFar(), inputScript.getParallel());
            if (ball)
            {
                Destroy(ball);
            }
            ball = Instantiate(ballPrefab, transform);
            ball.GetComponent<Rigidbody>().AddRelativeForce(shootDirection * shootSpeed);
        }
    }
    public void Relocate(Vector3 newPos,Vector3 dir)
    {
        transform.position = newPos;
        shootDirection = dir;
    }
}
