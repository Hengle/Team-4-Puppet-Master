using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerScript : MonoBehaviour
{
    public static int monsterCount = 0;
    [Header("Game Over Screens")]
    public string playerFullWinScreen = "TitleScreen";    //All monsters die
    public string playerPartialWinScreen = "TitleScreen"; //Some monsters die
    public string playerLoseScreen = "TitleScreen";       //No Monsters die
    [Header("Player Hit Attributes")]
    public int health = 100;
    public bool justHit = false;
    public float timeHit = 0;
    public float secondsBetweenHits = 3;

    // Update is called once per frame
    void Update()
    {
        hitDelay();
        checkDead();
        checkWon();
    }

    public static void monsterCounter()
    {
        monsterCount++;
    }  

    public void GetHit()
    {
        if(!justHit)
        {
            justHit = true;
            health -= 100;
            timeHit = Time.time;
            hitDelay();
        }
    }

    void hitDelay()
    {
        if(timeHit+secondsBetweenHits <= Time.time)
        {
            justHit = false;
        }
    }
    void checkDead()
    {
        if (health <= 0)
        {
            playerDeath();
        }
        if (health > 0)
        {
            //Do nothing
        }
    }
    void playerDeath()
    {
        //TODO
        //I'm not sure this will work. ALL monsters have to recieve that the player has died.
        //Debug.Log("Human Player Has died");
        MonsterScript.playerDeathReport();
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
        SceneManager.LoadScene(playerLoseScreen);
    }
    public static void playerWins()
    {
        monsterCount--;
    }
    void checkWon()
    {
        if(monsterCount==-1)
        {
            //TODO actual win
            SceneManager.LoadScene(playerFullWinScreen);
        }
    }
}
