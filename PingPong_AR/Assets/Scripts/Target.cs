using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
       // gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager = FindObjectOfType<GameManager>(); // Es gibt nur einen GameManger also liber diesen Befehl müssen und nicht String Refernece basirend 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Ball")) 
        {
            gameManager.HitTarget();
        }
    }
}
