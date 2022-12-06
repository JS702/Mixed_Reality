using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketScript : MonoBehaviour
{
    [SerializeField] Transform handtoTrack;
    Rigidbody racketRigi;
    public bool hitBall;
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

    //vibrationTime = Zeit in Sekunden, die der Controller vibrieren soll
    //controller = entweder OVRInput.Controller.RHand oder OVRInput.Controller.LHand für rechts oder links
    public IEnumerator vibrate(float vibrationTime, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(0.1f, 1f, controller);

        yield return new WaitForSeconds(vibrationTime);
        OVRInput.SetControllerVibration(0f, 0f, controller);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Ball"))
        {
            hitBall = true;
            StartCoroutine(vibrate(0.05f, OVRInput.Controller.RHand));
        }
    }
}
