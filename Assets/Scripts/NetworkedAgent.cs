using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkedAgent : MonoBehaviour
{
    //how fast the agent can move
    public float speed;

    protected Camera agentCamera;
    // Start is called before the first frame update
    void Awake()
    {
        //check if this agent is controlled locally
        if(IsLocalAgent())
        {
            //if so, activate its camera
            agentCamera = GetComponentInChildren<Camera>();
            agentCamera.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOverride();
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
    /// Handles all code related to movement
    /// </summary>
    protected virtual void UpdateOverride()
    {
        MoveCharacter();
    }

    protected abstract void MoveCharacter();
}
