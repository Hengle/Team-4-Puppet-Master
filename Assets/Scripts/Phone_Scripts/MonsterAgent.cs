using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAgent : NetworkedAgent
{
    public GameObject mobileCanvasPrefab;

    private MobileJoystick_UI joystick;
    public override void OnStartGame(NetworkConnection conn, StartGameMessage message)
    {
        base.OnStartGame(conn, message);

        Debug.Log("Phone Start");

        Instantiate(mobileCanvasPrefab, transform);

        joystick = GetComponentInChildren<MobileJoystick_UI>();
    }
    
    protected override void MoveCharacter()
    {
        transform.position += moveDir * Time.deltaTime * speed;
    }

    protected override void GetInput()
    {
        moveDir = new Vector3(joystick.moveDirection.x, 0, joystick.moveDirection.y);
    }
}
