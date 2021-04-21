using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour
{
    public enum Type { meleeItem, rangeItem, winItem, otherItem, none}

    public Type thisType;

    void Update()
    {
        transform.eulerAngles += Vector3.up;
    }

}
