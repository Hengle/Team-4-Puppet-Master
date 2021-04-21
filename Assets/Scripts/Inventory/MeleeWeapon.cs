using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public float swingSpeed = 1;
    public float swingAngle = 45;
    float swingCheck = 0;
    public GameObject hitArea;
    public Transform hitPosition;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnDisable()
    {
        swingCheck = 0;
        transform.rotation = Quaternion.Euler(Mathf.Lerp(0, swingAngle, swingCheck / (swingSpeed / 2f)), transform.eulerAngles.y, transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) && swingCheck == 0) || swingCheck > 0)
            swingWeapon();
    }


    void swingWeapon()
    {
        if (swingCheck == 0)
            Instantiate(hitArea, hitPosition);

        swingCheck += Time.deltaTime;
        if (swingCheck < swingSpeed / 2f)
            transform.rotation = Quaternion.Euler(Mathf.Lerp(0, swingAngle, swingCheck / (swingSpeed / 2f)), transform.eulerAngles.y, transform.eulerAngles.z);
        else
            transform.rotation = Quaternion.Euler(Mathf.Lerp(swingAngle, 0, (swingCheck - swingSpeed / 2f) / (swingSpeed / 2f)), transform.eulerAngles.y, transform.eulerAngles.z);

        if (swingCheck >= swingSpeed)
            swingCheck = 0;
    }




}
