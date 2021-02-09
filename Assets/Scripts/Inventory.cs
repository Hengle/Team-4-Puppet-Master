using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<PickupableItem> list;
    bool nearItem = false;
    int winCount = 0;

    void Start()
    {
        list = new List<PickupableItem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach(PickupableItem item in list)
            {
                if (item.thisType == PickupableItem.Type.meleeItem)
                {
                    item.selected = true;
                }
                else
                {
                    item.selected = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (PickupableItem item in list)
            {
                if (item.thisType == PickupableItem.Type.rangeItem)
                {
                    item.selected = true;
                }
                else
                {
                    item.selected = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (PickupableItem item in list)
            {
                if (item.thisType == PickupableItem.Type.winItem)
                {
                    item.selected = true;
                }
                else
                {
                    item.selected = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickupable"))
        {
            nearItem = true;
            PickupableItem item = other.GetComponent<PickupableItem>();
            if (item == null)
            {
                Debug.LogError("Item does not have PickupableItem component");
                nearItem = false;
            }
            else if (!item.onGround)
                nearItem = false;
        }
        else
            nearItem = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (nearItem && other.gameObject.CompareTag("Pickupable") && Input.GetKeyDown(KeyCode.E))
        {
            PickupableItem item = other.GetComponent<PickupableItem>();
            if (item == null)
            {
                Debug.LogError("Item does not have PickupableItem component");
                nearItem = false;
                return;
            }
            else
            {
                if (item.onGround)
                {
                    list.Add(item);
                    if (item.thisType == PickupableItem.Type.winItem)
                    {
                        winCount++;
                        if (winCount == 5)
                        {
                            //CALL A SAFE ZONE/WIN CONDITION LOCATION FOR KEYS HERE (PROBS IN GAME MANAGER)
                        }
                    }
                    if (item.thisType == PickupableItem.Type.weaponItem)
                    {
                        //EQUIP? ONCE YOU UNLOCK YOU CAN USE WITH BUTTON?
                    }
                    item.onGround = false;
                    Destroy(other.gameObject);
                }
            }
        }
        else
            nearItem = false;
    }

}
