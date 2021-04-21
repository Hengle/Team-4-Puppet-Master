using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSafe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Lower()
    {
        transform.localPosition -= Vector3.up * 100;
    }
    public void Raise()
    {
        transform.localPosition += Vector3.up * 100;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            other.GetComponent<MonsterScript>().monsterSafe = true;
        }

    }
}
