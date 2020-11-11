using Mirror;
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

    private int numPlayers;
    private int maxPlayers;
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

        numPlayers = NetworkManager.singleton.numPlayers;
        maxPlayers = NetworkManager.singleton.maxConnections;
        playerCount.text = $"Players: {numPlayers}/{maxPlayers}";
    }

    // Update is called once per frame
    void Update()
    {
        if(numPlayers != NetworkManager.singleton.numPlayers)
        {
            numPlayers = NetworkManager.singleton.numPlayers;
            playerCount.text = $"Players: {numPlayers}/{maxPlayers}";
        }
    }

    public void StartGame()
    {
        Camera.main.enabled = false;

        foreach(NetworkedAgent agent in FindObjectsOfType<NetworkedAgent>())
        {
            agent.CmdOnStartGame();
        }
    }
}
