using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Inventory), typeof(PlayerScript))]
public class HumanAgent : NetworkedAgent
{
    [Header("Camera Properties")]
    //How fast the camera moves
    public float lookSpeed;
    public float minCamXRot;
    public float maxCamXRot;
    [Header("Inventory Attributes")]
    public GameObject PickupUIPrefab;

    private GameObject PickupUI;
    private Inventory inventory;
    private PlayerScript playerHealth;
    private Vector2 cameraDelta;

    protected override void AwakeOverride()
    {
        //initialize script references
        inventory = GetComponent<Inventory>();
        playerHealth = GetComponent<PlayerScript>();

        SpawnUI();
    }

    protected override void StartOverride()
    {
        PickupUI.SetActive(false);
    }

    /// <summary>
    /// Moves the character and handles Camera rotation
    /// </summary>
    protected override void FixedUpdateOverride()
    {
        base.FixedUpdateOverride();

        if(cameraDelta.magnitude != 0)
        {
            MoveCamera();
        }
    }

    /// <summary>
    /// Get input from the player
    /// </summary>
    protected override void GetInput()
    {
        //get the input for movement and camera direction
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        cameraDelta = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));

        //Equip a melee item
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventory.SelectItem(PickupableItem.Type.meleeItem);
        }
        //equip a ranged item
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventory.SelectItem(PickupableItem.Type.rangeItem);
        }
        //equip a win item
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventory.SelectItem(PickupableItem.Type.winItem);
        }
        //pickup all nearby items
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.PickupItems();
            PickupUI.SetActive(false);
        }
    }

    /// <summary>
    /// Recieves mouse input and rotates the player object/camera accordingly
    /// </summary>
    private void MoveCamera()
    {
        //store a reference of the camera's transform
        Transform cameraTransform = agentCamera.gameObject.transform;

        //rotate the camera around the x axis to loox up/down
        cameraTransform.Rotate(cameraDelta.y * 10 * lookSpeed * Time.deltaTime, 0, 0);

        //TODO: Clamp First Person Camera Rotation

        //rotate the agent around the y axis to look left/right
        gameObject.transform.Rotate(0, cameraDelta.x * 10 * lookSpeed * Time.deltaTime, 0);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (inputLocked)
        {
            return;
        }

        Debug.Log(collision.gameObject.name);

        //if the collider was a monster
        if (collision.gameObject.CompareTag("Monster"))
        {
            if(GameManager.instance.playerHunting)
            {
                collision.gameObject.GetComponent<MonsterScript>().GetHit();
            }
            else
            {
                //let the health script know we were hit
                playerHealth.GetHit();
            }
        }
    }
    //Called when a trigger enters this collider
    void OnTriggerEnter(Collider other)
    {
        if(inputLocked)
        {
            return;
        }

        Debug.Log(other.gameObject.name);

        //if the collider was from a pickupable
        if (other.gameObject.CompareTag("Pickuppable"))
        {
            //get the item script
            PickupableItem item = other.GetComponent<PickupableItem>();
            //if the script is not null
            if (item != null)
            {
                inventory.AddItem(item);

                if (PickupUI.activeSelf == false)
                {
                    PickupUI.SetActive(true);
                }
            }
        }
    }

    private void SpawnUI()
    {
        var canvas = GameObject.FindGameObjectWithTag("UI Canvas");

        PickupUI = Instantiate(PickupUIPrefab, canvas.transform);
    }

    //Called when a trigger exits this collider
    private void OnTriggerExit(Collider other)
    {
        if(inputLocked)
        {
            return;
        }
        //if the collider was from a pickupable
        if (other.gameObject.CompareTag("Pickuppable"))
        {
            //get the item script
            PickupableItem item = other.GetComponent<PickupableItem>();
            //if the script is not null and this item is in the near items list
            if (item != null)
            {
                if (inventory.RemoveItem(item) && PickupUI.activeSelf == true)
                {
                    PickupUI.SetActive(false);
                }
            }
        }
    }
}
