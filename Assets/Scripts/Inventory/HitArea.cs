using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArea : MonoBehaviour
{
    float timeToLive = 0.5f;
    float timeAlive = 0;
    public bool arrow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!arrow)
            timeAlive += Time.deltaTime;
        if (timeAlive >= timeToLive && !arrow)
            Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (GameManager.instance.playerHunting)
        {
            if (other.gameObject.CompareTag("Monster"))
                other.GetComponent<MonsterScript>().GetHit();
        }
    }
}
