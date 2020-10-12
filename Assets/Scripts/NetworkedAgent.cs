using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkedAgent : MonoBehaviour
{
    //how fast the agent can move
    public float speed;

    protected Vector3 moveDir;

    protected Camera agentCamera;
    // Start is called before the first frame update
    void Awake()
    {
        //check if this agent is controlled locally
        if(IsLocalAgent())
        {
            AwakeOverride();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOverride();
    }

    private void FixedUpdate()
    {
        FixedUpdateOverride();
    }

    /// <summary>
    /// Determines if this agent is controlled by the local player
    /// </summary>
    /// <returns></returns>
    private bool IsLocalAgent()
    {
        return true;
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

    protected virtual void AwakeOverride()
    {
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
