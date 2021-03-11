using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winBoundary : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Inventory>().inLocation=true; //referece a component connected to the same thing. 
        }
       
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Inventory>().inLocation = false; //referece a component connected to the same thing. 
        }

    }
}
