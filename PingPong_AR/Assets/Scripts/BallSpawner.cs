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
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); // 
    }

    void Update()
    {
        if (isEnabled && OVRInput.GetDown(OVRInput.Button.One))//TODO gucken welcher Knopf und ob der immer auslöst oder unter bestimmten Bedingungen
        {
            //gameManager.SpawnTarget(); wieder anmachen wenn Tisch bauen geht
            if (ball)
            {
                Destroy(ball);
            }
            ball = Instantiate(ballPrefab, transform);
            ball.GetComponent<Rigidbody>().AddRelativeForce(shootDirection * shootSpeed);
        }
    }
}
