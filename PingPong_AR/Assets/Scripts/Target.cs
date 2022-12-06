using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    GameManager gameManager;
    RacketScript racketScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Es gibt nur einen GameManger also liber diesen Befehl müssen und nicht String Refernece basirend
        racketScript = FindObjectOfType<RacketScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Ball") && racketScript.hitBall) //hitBall verhindert, dass der Ball das Target trifft, ohne vorher den Schläger berührt zu haben
        {
            gameManager.HitTarget();
            //Debug.Log("Target destroyed. Cause: Hit Target");
            Destroy(gameObject);
        }
    }
}
