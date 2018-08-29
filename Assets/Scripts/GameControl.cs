using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    // reference to the various UI text elements
    public Text Score, LifePoint, Rune, GameOver;
    GameObject MainMenu;

    public AudioSource Dead;
    public AudioSource Bgm;

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    // number of Score
    public int value = 0;
    public int lifePoint = 3;

    public float timeTemp_potion = 0;
    public float timeLeft_potion = 40f;
    public GameObject potionPrefab;

    // Use this for initialization
    void Start ()
    {
        //Start BGM
        Bgm.Play();

        MainMenu = GameObject.Find("Button");
        MainMenu.SetActive(false);
        GameOver.enabled = false;
        LifePoint.text = lifePoint.ToString();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GameObject.FindGameObjectWithTag("Potion"))
            if (timeTemp_potion > 0)
                timeTemp_potion -= Time.deltaTime;
            else if (GameObject.FindGameObjectWithTag("Potion"))
                Destroy(GameObject.FindGameObjectWithTag("Potion"));
    }

    // Spawn one piece of potion
    void SpawnPotion()
    {
        timeTemp_potion = timeLeft_potion;

        int x, y;
        do
        {
            // x position between left & right border
            x = (int)Random.Range(borderLeft.position.x,
                                      borderRight.position.x);

            // y position between top & bottom border
            y = (int)Random.Range(borderBottom.position.y,
                                      borderTop.position.y);
        } while (Physics.CheckSphere(new Vector2(x, y), 0.2f));

        if (GameObject.FindGameObjectWithTag("Potion"))
            Destroy(GameObject.FindGameObjectWithTag("Potion"));

        // Instantiate the wall at (loc[0], loc[1])
        Instantiate(potionPrefab,
                    new Vector2(x, y),
                    Quaternion.identity); // default rotation
    }

    // Increase the score of the player
    // called from another script
    public void AddScore(int num)
    {
        if ((int)(value / 10) != (int)((value += num) / 10) && !GameObject.FindGameObjectWithTag("Potion") && value != 0)
            SpawnPotion();
        value += num;
        Score.text = value.ToString();
    }

    public void ShowRuneEffect(string runeEffect)
    {
        Rune.text = runeEffect;
    }

    public bool PlayerDie()
    {
        lifePoint--;
        LifePoint.text = lifePoint.ToString();
        if (lifePoint <= 0)
        {
            Time.timeScale = 0;
            GameOver.enabled = true;

            //Change Sound Effect
            Bgm.Stop();
            Dead.Play();

            MainMenu.SetActive(true);
                GameOver.text = "Game Over!\nYour Score is " + value;
            return true;
        }
        return false;
    }

    public void AddLife()
    {
        lifePoint++;
        LifePoint.text = lifePoint.ToString();
    }
}
