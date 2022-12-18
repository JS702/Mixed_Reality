using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] AudioClip ballGround;
    private void OnCollisionEnter(Collision collision)
    {
        if (!(collision.gameObject.tag.Equals("Racket")))
        {
            
            AudioSource.PlayClipAtPoint(ballGround, transform.position, 1f);
           

        }
    }
}
