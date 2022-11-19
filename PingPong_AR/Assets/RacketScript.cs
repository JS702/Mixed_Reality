using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketScript : MonoBehaviour
{
    [SerializeField] Transform handtoTrack;
    Rigidbody racketRigi;
    // Start is called before the first frame update
    void Start()
    {
        racketRigi = GetComponent<Rigidbody>();
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        racketRigi.velocity = (handtoTrack.position - transform.position) / Time.fixedDeltaTime;

        /*
        Quaternion roationdiff = handtoTrack.rotation * Quaternion.Inverse(transform.rotation);
        roationdiff.ToAngleAxis(out float angleInDeg, out Vector3 rotationAxis);

        Vector3 rotationDifferneceInDegree = angleInDeg * rotationAxis;

        racketRigi.angularVelocity = (rotationDifferneceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
        */
        
        transform.rotation = handtoTrack.rotation; // Nur zum Test -1
        
        racketRigi.MoveRotation( handtoTrack.rotation); // Nur zum Test -1
    }
}
