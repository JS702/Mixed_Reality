using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    [SerializeField] GameObject followObject;
    GameObject WorkingObject;
    [SerializeField] GameObject toTracker;
    // Start is called before the first frame update
    void Start()
    {
        WorkingObject = Instantiate(followObject, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        WorkingObject.transform.position = toTracker.transform.position;
    }
}
