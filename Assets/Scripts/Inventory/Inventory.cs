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
        //check if the player has reached the win condition
        if (winCount == winTarget && inLocation == true)
        {
            Win();
        }
    }

    /// <summary>
    /// Picks up all items in the near items list
    /// </summary>
    public void PickupItems()
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
    }

    /// <summary>
    /// selects the items of a given type in the list
    /// </summary>
    /// <param name="type"></param>
    public void SelectItem(PickupableItem.Type type)
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
    /// adds a given item to the near items list
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(PickupableItem item)
    {
        //add the item to the near items list
        nearItems.Add(item);
    }

    /// <summary>
    /// removes a given item to the near items list
    /// </summary>
    /// <param name="item"></param>
    public bool RemoveItem(PickupableItem item)
    {
        bool emptyList = false;

        if (nearItems.Contains(item))
        {
            //remove the item from the near items list
            nearItems.Remove(item);

            if (nearItems.Count == 0)
            {
                emptyList = true;
            }
        }

        return emptyList;
    }

    /// <summary>
    /// Called when the player collects enough win items
    /// </summary>
    private void Win()
    {
        Debug.Log("You have collected all keys and dropped them at a safe location. Now face your fears");
        GameObject.Find("Main Light").GetComponent<Light>().intensity = 1;
        winCount = 0;
        GameManager.instance.playerHunting = true;
    }
}
