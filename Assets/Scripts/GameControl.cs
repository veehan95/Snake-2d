using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

    // reference to the various UI text elements
    public Text Score, HighScore;

    // number of Score
    int value = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Increase the score of the player
    // called from another script
    public void AddScore(int num)
    {
        value += num;
        Score.text = value.ToString();
    }

    public void PlayerDie()
    {
        value = 0;
        Score.text = value.ToString();

        /*
        if (score == 0)
        {
            gameOverText.enabled = true;
            Time.timeScale = 0;
        }
        */
    }
}
