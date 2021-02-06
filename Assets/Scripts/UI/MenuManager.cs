using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Button Layouts")]
    public GameObject phoneButtons;
    public GameObject pcButtons;
    public GameObject devButtons;
    [Header("Sub Menus")]
    public TMP_InputField roomAddress;

    private NetworkManagerPhoton photonManager;
    private bool pcPlayer = true;
    private bool dev = false;
    private GameObject mainScreen;
    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_EDITOR
        dev = true;
#elif UNITY_IOS || UNITY_ANDROID
        pcPlayer = false;
#endif
        if (pcPlayer)
        {
            if (dev)
            {
                mainScreen = devButtons;
            }
            else
            {
                mainScreen = pcButtons;
            }
        }
        else
        {
            mainScreen = phoneButtons;
        }

        mainScreen.SetActive(true);
    }

    private void Start()
    {
        photonManager = NetworkManagerPhoton.instance;
    }

    public void HostGame()
    {
        photonManager.CreateRoom(roomAddress.text);
    }

    public void JoinGame()
    {
        photonManager.JoinRoom(roomAddress.text);
    }

    public void OpenSettings()
    {
        
    }

    public void PlayTutorial()
    {

    }


}
