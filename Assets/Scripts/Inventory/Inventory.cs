using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviourPunCallbacks
{
    public Image[] guiList;
    public GameObject[] modelList;
    public Text winNumText;

    public GameObject heldItem;

    /// <summary>
    /// number of win items needed to trigger the win condition
    /// </summary>
    public int winTarget;

    //list of pickupable items near the player
    private List<PickupableItem> nearItems;

    private bool _melee, _range;
    private int _win;
    public bool hasMelee 
    { 
        get { return _melee; } 
        set 
        { 
            _melee = value;
            guiList[0].gameObject.SetActive(value); 
        } 
    }

    public bool hasRange
    {
        get { return _range; }
        set
        {
            _range = value;
            guiList[1].gameObject.SetActive(value);
        }
    }
    public int numWinItems
    {
        get { return _win; }
        set
        {
            _win = value;
            guiList[2].gameObject.SetActive(value > 0);
            winNumText.text = value + "";
        }
    }

    public PickupableItem.Type currentlySelectedItem;


    public bool inLocation = false;

    void Awake()
    {
        if(!photonView.IsMine)
        {
            Destroy(this);
        }

        //initialize item lists
        nearItems = new List<PickupableItem>();

        currentlySelectedItem = PickupableItem.Type.none;

        numWinItems = 0;
        hasMelee = false;
        hasRange = false;
    }

    //called once per frame
    private void Update()
    {
        //check if the player has reached the win condition
        
            if (numWinItems == winTarget && inLocation)
                    Win();
    }

    /// <summary>
    /// Picks up all items in the near items list
    /// </summary>
    public void PickupItems()
    {
        foreach (PickupableItem item in nearItems)
        {
            switch (item.thisType)
            {
                case PickupableItem.Type.winItem:
                    numWinItems++;
                    break;
                case PickupableItem.Type.meleeItem:
                    hasMelee = true;
                    break;
                case PickupableItem.Type.rangeItem:
                    hasRange = true;
                    break;
            }
            if (currentlySelectedItem == PickupableItem.Type.none)
                SelectItem(item.thisType);

            //destroy the gameobject for this item (may cause issues?)
            Destroy(item.gameObject);
        }
        //clear the nearItems list
        nearItems.Clear();
    }

    private bool HasItem(PickupableItem.Type t)
    {
        switch (t)
        {
            case PickupableItem.Type.winItem:
                return (numWinItems > 0);
            case PickupableItem.Type.meleeItem:
                return hasMelee;
            case PickupableItem.Type.rangeItem:
                return hasRange;
            default:
                return false;
        }
    }


    /// <summary>
    /// selects the items of a given type in the list
    /// </summary>
    /// <param name="type"></param>
    public void SelectItem(PickupableItem.Type type)
    {
        foreach (Image i in guiList)
            i.GetComponent<Outline>().enabled = false;
        heldItem.GetComponent<MeleeWeapon>().enabled = (type == PickupableItem.Type.meleeItem);
        heldItem.GetComponent<RangeWeapon>().enabled = (type == PickupableItem.Type.rangeItem);

        if (currentlySelectedItem != PickupableItem.Type.none)
            Destroy(heldItem.transform.Find("CurrentlyHeld").gameObject);
        if (HasItem(type))
            currentlySelectedItem = type;
        guiList[(int)currentlySelectedItem].GetComponent<Outline>().enabled = true;
        GameObject currHeld = Instantiate(modelList[(int)currentlySelectedItem], heldItem.transform);
        currHeld.name = "CurrentlyHeld";
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
        numWinItems = 0;
        GameManager.instance.playerHunting = true;
    }


}
