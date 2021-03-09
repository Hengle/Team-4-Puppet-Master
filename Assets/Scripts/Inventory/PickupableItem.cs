using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableItem : MonoBehaviour
{
    public Sprite guiImage;
    public string guiName;
    public bool onGround = true;
    public bool selected = false;

    public enum Type { winItem, rangeItem, meleeItem, otherItem}

    public Type thisType;

    //win item= usb/key
    //weapon item? (pipe, etc?)

    //potential for later trap item
    void Use() 
    {
        
    }


}
