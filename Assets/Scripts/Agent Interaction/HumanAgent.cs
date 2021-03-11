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

    private Vector2 cameraDelta;

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
    /// 
    /// </summary>
    protected override void GetInput()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        cameraDelta = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
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
}
