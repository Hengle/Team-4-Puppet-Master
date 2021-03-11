using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManagerPhoton : MonoBehaviourPunCallbacks
{
    private enum GameplayScene
    {
        TwoBlocks,
        ScottScene
    }


    public static NetworkManagerPhoton instance;

    [SerializeField]
    private GameplayScene gameplayScene;

    private string GameplaySceneName;
    private string[] SceneNames = { "TwoBlocks", "ScottScene" };

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            GameplaySceneName = SceneNames[(int)gameplayScene];
        }
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void CreateRoom(string roomName)
    {
        if(roomName == null)
        {
            PhotonNetwork.CreateRoom($"Room {PhotonNetwork.CountOfRooms}");
        }
        else
        {
            PhotonNetwork.CreateRoom(roomName);
        }
    }

    public void JoinRoom(string roomName)
    {
        if(roomName == null)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.JoinRoom(roomName);
        }
    }

    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
    }
    public override void OnCreatedRoom()
    {
        Debug.Log($"Created Room: {PhotonNetwork.CurrentRoom.Name}");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
        ChangeScene(GameplaySceneName);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Disconnected: {cause}");
    }
    #endregion
}
