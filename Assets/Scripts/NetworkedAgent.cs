using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class NetworkedAgent : NetworkBehaviour
{
    //how fast the agent can move
    public float speed;

    protected Vector3 moveDir;

    protected Camera agentCamera;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Is Local Player: {isLocalPlayer}");
        //check if this agent is controlled locally
        if(isLocalPlayer)
        {
            StartOverride();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer)
        {
            UpdateOverride();
        }
    }

    private void FixedUpdate()
    {
        if(isLocalPlayer)
        {
            FixedUpdateOverride();
        }
    }

    /// <summary>
    /// Handles all code related to input
    /// </summary>
    protected virtual void UpdateOverride()
    {
        GetInput();
    }

    /// <summary>
    /// Handles all code related to movement and physics
    /// </summary>
    protected virtual void FixedUpdateOverride()
    {
        MoveCharacter();
    }

    protected virtual void StartOverride()
    {
        Debug.Log("Start Override");
        //if so, activate its camera
        agentCamera = GetComponentInChildren<Camera>();
        agentCamera.enabled = true;
    }

    /// <summary>
    /// Moves the agent based on the moveDir variable
    /// </summary>
    protected virtual void MoveCharacter()
    {
        gameObject.transform.Translate(moveDir * speed * Time.deltaTime);
    }

    /// <summary>
    /// Recieves Input from the player
    /// </summary>
    protected abstract void GetInput();
}
