using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DisplayScore : MonoBehaviour
{
    public GameObject gameManagerObject;
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] int textSize = 100;
    // Start is called before the first frame update
    void Start()
    {
        txt.fontSize=textSize;
    }

    // Update is called once per frame
    void Update()
    {
        int scoreNumber=gameManagerObject.GetComponent<GameManager>().score;
        txt.text=(scoreNumber).ToString();
    }
}
