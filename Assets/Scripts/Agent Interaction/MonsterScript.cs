using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterScript : MonoBehaviour
{
    public static bool playerDead = false;
    public bool monsterSafe = false;
    public int monsterHealth = 1000;
    public string fullWinScreen = "TitleScreen";    //No monsters die
    public string partialWinScreen = "TitleScreen"; //Some monsters die
    public string loseScreen = "TitleScreen";       //All Monsters die
    public string waitingScreen = "TitleScreen"; //Some monsters die

    // Start is called before the first frame update
    void Start()
    {
        PlayerScript.monsterCounter();
        monsterHealth = 1000;

    }

    // Update is called once per frame
    void Update()
    {
        playerDeadYet();
        checkIfDead();
    }

    public void GetHit()
    {
        monsterHealth = 0;
    }

    void checkIfDead()
    {
        if (monsterHealth <= 0)
        {
            //Temporary way of showing monster death until further menus are developed
            Destroy(gameObject);
            //TODO -- Die :)
            //PlayerScript.playerWins();
            //SceneManager.LoadScene(waitingScreen);
        }
    }
    void playerDeadYet()
    {
        if (playerDead)
        {
            //TODO Actual end of game
            SceneManager.LoadScene(fullWinScreen);
        }
    }
    public static void playerDeathReport()
    {
        playerDead = true;
    }

}
