using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// List of items the player has collected
    /// </summary>
    public List<PickupableItem> itemList;
    /// <summary>
    /// number of win items needed to trigger the win condition
    /// </summary>
    public int winTarget;
    /// <summary>
    /// UI for if an item is nearby
    /// </summary>
    public GameObject PickupUI;

    //how many win items have been collected so far
    private int winCount = 0;
    //list of pickupable items near the player
    private List<PickupableItem> nearItems;


    public bool inLocation = false;

    void Awake()
    {
        //initialize item lists
        itemList = new List<PickupableItem>();
        nearItems = new List<PickupableItem>();
    }

    //called once per frame
    private void Update()
    {
        //Get input from the player
        HandleInput();

        //check if the player has reached the win condition
        if (winCount == winTarget && inLocation == true)
        {
            Win();
        }

    }

    /// <summary>
    /// Handles recieving input from the player
    /// </summary>
    private void HandleInput()
    {
        //Equip a melee item
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectItem(PickupableItem.Type.meleeItem);
        }
        //equip a ranged item
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectItem(PickupableItem.Type.rangeItem);
        }
        //equip a win item
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectItem(PickupableItem.Type.winItem);
        }
        //pickup all nearby items
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupItems();
        }
    }

    private void PickupItems()
    {
        foreach (PickupableItem item in nearItems)
        {
            //add the item to the item list
            itemList.Add(item);

            //if it is a win item
            if (item.thisType == PickupableItem.Type.winItem)
            {
                //increment the win counter
                winCount++;
            }
            //destroy the gameobject for this item (may cause issues?)
            Destroy(item.gameObject);
        }
        //clear the nearItems list
        nearItems.Clear();
        PickupUI.SetActive(false);
    }

    /// <summary>
    /// selects the items of a given type in the list
    /// </summary>
    /// <param name="type"></param>
    private void SelectItem(PickupableItem.Type type)
    {
        foreach (PickupableItem item in itemList)
        {
            if (item.thisType == type)
            {
                item.selected = true;
            }
            else
            {
                item.selected = false;
            }
        }
    }

    /// <summary>
    /// Called when the player collects enough win items
    /// </summary>
    private void Win()
    {
        Debug.Log("You have collected all keys and dropped them at a safe location. Now face your fears");
        GameObject.Find("Main Light").GetComponent<Light>().intensity = 1;
        winCount = 0;
    }

    //Called when a trigger enters this collider
    void OnTriggerEnter(Collider other)
    {
        //if the collider was from a pickupable
        if (other.gameObject.CompareTag("Pickuppable"))
        {
            //get the item script
            PickupableItem item = other.GetComponent<PickupableItem>();
            //if the script is not null
            if (item != null)
            {
                //add the item to the near items list
                nearItems.Add(item);

                if(PickupUI.activeSelf == false)
                {
                    PickupUI.SetActive(true);
                }
            }
        }
    }
    //Called when a trigger exits this collider
    private void OnTriggerExit(Collider other)
    {
        //if the collider was from a pickupable
        if (other.gameObject.CompareTag("Pickuppable"))
        {
            //get the item script
            PickupableItem item = other.GetComponent<PickupableItem>();
            //if the script is not null and this item is in the near items list
            if (item != null && nearItems.Contains(item))
            {
                //remove the item from the near items list
                nearItems.Remove(item);

                if(nearItems.Count == 0 && PickupUI.activeSelf == true)
                {
                    PickupUI.SetActive(false);
                }
            }
        }
    }

}
