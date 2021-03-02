using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : PickupableItem
{
    public float swingSpeed = 1;
    public float swingAngle = 45;
    float swingCheck = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       

    }


    void swingWeapon()
    {
        if (!onGround && selected)
        {

            if ((Input.GetMouseButtonDown(0) && swingCheck == 0) || swingCheck > 0)
                swingWeapon();
        }
        swingCheck += Time.deltaTime;
        if (swingCheck < swingSpeed / 2f)
            transform.rotation = Quaternion.Euler(Mathf.Lerp(0, swingAngle, swingCheck / (swingSpeed / 2f)), transform.eulerAngles.y, transform.eulerAngles.z);
        else
            transform.rotation = Quaternion.Euler(Mathf.Lerp(swingAngle, 0, (swingCheck - swingSpeed / 2f) / (swingSpeed / 2f)), transform.eulerAngles.y, transform.eulerAngles.z);

        if (swingCheck >= swingSpeed)
            swingCheck = 0;
       
    }




}
