using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Network Manager")]
    public NetworkManager manager;
    [Header("Button Layouts")]
    public GameObject phoneButtons;
    public GameObject pcButtons;
    public GameObject devButtons;
    [Header("Sub Menus")]
    public GameObject joinScreen;
    public TMP_InputField joinAddress;
    

    private bool pcPlayer = true;
    private bool dev = false;
    private GameObject mainScreen;
    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_IOS || UNITY_ANDROID
        pcPlayer = false;
#elif UNITY_EDITOR
        dev = true;
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

    public void HostGame()
    {
        manager.StartHost();
    }

    public void JoinGameScreen()
    {
        mainScreen.SetActive(!mainScreen.activeSelf);
        joinScreen.SetActive(!joinScreen.activeSelf);
        joinAddress.text = manager.networkAddress;
    }

    public void JoinGame()
    {
        manager.networkAddress = joinAddress.text;
        manager.StartClient();
    }

    public void OpenSettings()
    {
        
    }

    public void PlayTutorial()
    {

    }


}
