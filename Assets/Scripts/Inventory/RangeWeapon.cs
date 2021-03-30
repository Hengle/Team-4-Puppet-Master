using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    public GameObject projectile, shootPos;


    // Start is called before the first frame update
    void shootProj()
    {
        Instantiate(projectile, shootPos.transform.position, shootPos.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            shootProj();
    }
}
