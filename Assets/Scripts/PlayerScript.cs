using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerScript : MonoBehaviour
{
    public int health = 100;
    public static int monsterCount = 0;
    public string playerFullWinScreen = "TitleScreen";    //All monsters die
    public string playerPartialWinScreen = "TitleScreen"; //Some monsters die
    public string playerLoseScreen = "TitleScreen";       //No Monsters die
    public bool justHit = false;
    public float timeHit = 0;
    public float secondsBetweenHits = 3;
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        monsterCount = 0;
    }

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

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "MonsterAttack" & !justHit)
        {
            justHit = true;
            health -= 50 ;
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
