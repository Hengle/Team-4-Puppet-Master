using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// reference to this game manager instance
    /// </summary>
    public static GameManager instance;

    #region Public attributes
    [Header("Server Settings")]
    /// <summary>
    /// Maximum number of players in a room
    /// </summary>
    [Range(2,10)]
    public int maxPlayers;
    /// <summary>
    /// prefab to sppawn player agents from
    /// </summary>
    public GameObject agentPrefab;

    [Header("Spawn Locations")]
    /// <summary>
    /// Where in the level player's spawn from
    /// </summary>
    public Transform[] spawnLocations;
    /// <summary>
    /// Where in the level player's spawn from
    /// </summary>
    public Transform[] lobbyLocations;

    [Header("Object References")]
    /// <summary>
    /// reference to the start game button
    /// </summary>
    public GameObject StartButton;
    /// <summary>
    /// reference to the player count text
    /// </summary>
    public Text playerCount;
    #endregion

    //reference to the current Photon room
    private Photon.Realtime.Room currentRoom;
    private GameObject localAgent;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // in case we started this demo with the wrong scene being active, simply load the menu scene
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("TitleScreen");

            return;
        }

        currentRoom = PhotonNetwork.CurrentRoom;

        if (!PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(false);
        }
        else
        {
            currentRoom.MaxPlayers = (byte)maxPlayers;
        }

        localAgent = PhotonNetwork.Instantiate(agentPrefab.name, lobbyLocations[currentRoom.PlayerCount - 1].position, Quaternion.identity);
        Debug.Log(currentRoom.PlayerCount);

        UpdateLobbyText();
    }

    /// <summary>
    /// Called when the host clicks the start game button
    /// </summary>
    public void StartGame()
    {
        currentRoom.IsOpen = false;
        playerCount.gameObject.SetActive(false);
        StartButton.SetActive(false);
        //Tell all player objects to start the game
        photonView.RPC("OnStartGame", RpcTarget.All);
    }

    [PunRPC]
    private void OnStartGame()
    {
        Camera.main.enabled = false;

        localAgent.GetComponent<NetworkedAgent>().OnStartGame();
    }

    /// <summary>
    /// returns the number of players in the room
    /// </summary>
    /// <returns></returns>
    public int GetNumPlayers()
    {
        return currentRoom.PlayerCount;
    }

    /// <summary>
    /// Updates the text in the lobby screen
    /// </summary>
    public void UpdateLobbyText()
    {
        playerCount.text = $"Players: {currentRoom.PlayerCount}/{maxPlayers}";
    }

    #region Photon Callbacks
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateLobbyText();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyText();
    }
    #endregion
}
