using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HumanAgent : NetworkedAgent
{
    [Header("Camera Properties")]
    //How fast the camera moves
    public float lookSpeed;
    public float minCamXRot;
    public float maxCamXRot;

    /// <summary>
    /// Moves the character and handles Camera rotation
    /// </summary>
    protected override void UpdateOverride()
    {
        base.UpdateOverride();
        MoveCamera();
    }

    /// <summary>
    /// Moves the agent using WASD input
    /// </summary>
    protected override void MoveCharacter()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        gameObject.transform.Translate(input * speed * Time.deltaTime);
    }

    /// <summary>
    /// Recieves mouse input and rotates the player object/camera accordingly
    /// </summary>
    private void MoveCamera()
    {
        //store a reference of the camera's transform
        Transform cameraTransform = agentCamera.gameObject.transform;

        //rotate the camera around the x axis to loox up/down
        cameraTransform.Rotate(-Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime, 0, 0);

        //TODO: Clamp First Person Camera Rotation

        //rotate the agent around the y axis to look left/right
        gameObject.transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime, 0);
    }
}
