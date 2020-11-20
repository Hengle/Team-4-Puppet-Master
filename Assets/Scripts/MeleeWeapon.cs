using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : PickupableItem
{
    public float swingSpeed = 1;
    public float swingAngle = 45;
    float swing = 0;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!onGround && selected)
        {

            if ((Input.GetMouseButtonDown(0) && swing == 0) || swing > 0)
                swingWeapon();
        }

    }


    void swingWeapon()
    {
        swing += Time.deltaTime;
        if (swing < swingSpeed / 2f)
            transform.rotation = Quaternion.Euler(Mathf.Lerp(0, swingAngle, swing / (swingSpeed / 2f)), transform.eulerAngles.y, transform.eulerAngles.z);
        else
            transform.rotation = Quaternion.Euler(Mathf.Lerp(swingAngle, 0, (swing - swingSpeed / 2f) / (swingSpeed / 2f)), transform.eulerAngles.y, transform.eulerAngles.z);

        if (swing >= swingSpeed)
            swing = 0;
    }




}
