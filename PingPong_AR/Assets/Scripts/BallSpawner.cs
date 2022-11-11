using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;

    [SerializeField] GameObject ballPrefab;

    public Vector3 shootDirection;

    public float shootSpeed;
    void Start()
    {
        shootDirection = new Vector3(0,1,0);
        shootSpeed = 0.5f;
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))//TODO gucken welcher Knopf und ob der immer auslöst oder unter bestimmten Bedingungen
        {
            if (ball)
            {
                Destroy(ball);
            }
            ball = Instantiate(ballPrefab);
            ball.GetComponent<Rigidbody>().AddRelativeForce(shootDirection * shootSpeed);
        }
    }
}
