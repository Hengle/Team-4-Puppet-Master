using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public abstract class NetworkedAgent : NetworkBehaviour
{
    //how fast the agent can move
    public float speed;
    

    protected Vector3 moveDir;

    protected Camera agentCamera;
    protected int networkID;
    protected bool inputLocked;
    protected Action<NetworkConnection, StartGameMessage> StartGameAction;
    // Start is called before the first frame update
    void Start()
    {
        networkID = GameManager.instance.GetNumPlayers() - 1;
        Debug.Log($"Is Local Player: {isLocalPlayer}");
        //check if this agent is controlled locally
        if(isLocalPlayer)
        {
            OnJoinLobby();
            StartGameAction += OnStartGame;
            NetworkClient.RegisterHandler<StartGameMessage>(StartGameAction);
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
        if(!inputLocked)
        {
            GetInput();
        }
    }

    /// <summary>
    /// Handles all code related to movement and physics
    /// </summary>
    protected virtual void FixedUpdateOverride()
    {
        if(moveDir.magnitude != 0)
        {
            MoveCharacter();
        }
    }

    /// <summary>
    /// Called when the player joins the lobby
    /// </summary>
    protected virtual void OnJoinLobby()
    {
        inputLocked = true;
        Debug.Log($"Network ID: {getIdNumber()}");
    }

    /// <summary>
    /// Called when the host starts the game
    /// </summary>
    public virtual void OnStartGame(NetworkConnection conn, StartGameMessage message)
    {
        //if so, activate its camera
        agentCamera = GetComponentInChildren<Camera>();
        agentCamera.enabled = true;

        inputLocked = false;

        transform.position = GameManager.instance.spawnLocations[getIdNumber()].position;
    }

    /// <summary>
    /// Moves the agent based on the moveDir variable
    /// </summary>
    protected virtual void MoveCharacter()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    public int getIdNumber()
    {
        return networkID;
    }

    /// <summary>
    /// Recieves Input from the player
    /// </summary>
    protected abstract void GetInput();
}
