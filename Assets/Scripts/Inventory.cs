using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<PickupableItem> list;
    bool nearItem = false;

    void Start()
    {
        list = new List<PickupableItem>();
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
                    item.onGround = false;
                    Destroy(other.gameObject);
                }
            }
        }
        else
            nearItem = false;
    }

}
