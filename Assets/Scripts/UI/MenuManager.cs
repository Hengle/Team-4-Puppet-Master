using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject phoneButtons;
    public GameObject pcButtons;
    public GameObject devButtons;

    private bool pcPlayer = true;
    private bool dev = false;
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
                devButtons.SetActive(true);
            }
            else
            {
                pcButtons.SetActive(true);
            }
        }
        else
        {
            phoneButtons.SetActive(true);
        }
    }

    public void HostGame()
    {

    }

    public void JoinGame()
    {

    }

    public void OpenSettings()
    {

    }

    public void PlayTutorial()
    {

    }
}
