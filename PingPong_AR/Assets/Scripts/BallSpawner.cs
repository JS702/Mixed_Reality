using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;

    [SerializeField] GameObject ballPrefab;

    public Vector3 shootDirection;

    public bool isEnabled;

    public float shootSpeed;
    void Start()
    {

    }

    void Update()
    {
        if (isEnabled && OVRInput.GetDown(OVRInput.Button.One))//TODO gucken welcher Knopf und ob der immer auslöst oder unter bestimmten Bedingungen
        {
            if (ball)
            {
                Destroy(ball);
            }
            ball = Instantiate(ballPrefab, transform);
            ball.GetComponent<Rigidbody>().AddRelativeForce(shootDirection * shootSpeed);
        }
    }
}
