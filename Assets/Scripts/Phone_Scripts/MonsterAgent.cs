using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAgent : NetworkedAgent
{
    public MobileJoystick_UI joystick;
    /// <summary>
    /// Moves the agent using phone joystick input
    /// </summary>
    protected override void MoveCharacter()
    {
        Vector3 pos = transform.position;

        Vector3 movement = new Vector3(joystick.moveDirection.x, 0, joystick.moveDirection.y);

        transform.position = pos + movement * Time.deltaTime * speed;
    }
}
