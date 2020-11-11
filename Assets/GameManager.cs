using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public Transform[] spawnLocations = new Transform[5];
    public static GameManager instance;
    public GameObject StartButton;
    public Text playerCount;

    [SyncVar]
    private int numPlayers;
    private int maxPlayers;
    private Action<NetworkConnection, ConnectMessage> PlayerConnectAction;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (!isServer)
        {
            StartButton.SetActive(false);
        }
        else
        {
            numPlayers = NetworkServer.connections.Count;
        }

        maxPlayers = NetworkManager.singleton.maxConnections;
        playerCount.text = $"Players: {numPlayers}/{maxPlayers}";
    }

    // Update is called once per frame
    void Update()
    {
        if(isServer && numPlayers != NetworkServer.connections.Count)
        {
            numPlayers = NetworkServer.connections.Count;
        }

        playerCount.text = $"Players: {numPlayers}/{maxPlayers}";
    }

    public void StartGame()
    {
        Camera.main.enabled = false;

        NetworkServer.SendToAll(new StartGameMessage());

        playerCount.gameObject.SetActive(false);
        StartButton.SetActive(false);
    }

    public int GetNumPlayers()
    {
        return numPlayers;
    }
}
