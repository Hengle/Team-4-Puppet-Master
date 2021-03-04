using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : PickupableItem
{
    public int ammo;
    public GameObject projectile;




    // Start is called before the first frame update
    void shootProj()
    {
        if (!onGround && selected)
        {
            if (Input.GetMouseButtonDown(0) && ammo > 0)
                shootProj();
        }
        Instantiate(projectile, transform.position, transform.rotation);
        ammo--;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
