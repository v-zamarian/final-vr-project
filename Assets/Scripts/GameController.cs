// Victor Zamarian
// 4/6/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VRTK;

[DefaultExecutionOrder (1)]
public class GameController : MonoBehaviour {

    public bool levelOver;
    public float totalTime;

    [HideInInspector]
    public float extraSpawnTime; //items wait longer to spawn at higher speeds

    public int maxStrikes;
    public Text strikeText;
    public Text strikeTextMax;
    public Text[] strikesTextList;

    public Text timerText;
    public Text pointsText;
    public Text keepText;
    public Text leverText;
    public int pointsRequired;
    public int pointsAmount; //amount to increase points by
    public GameObject[] itemList;

    public static GameController instance;

    float timeLeft = 0.0f;
    int keepItem;
    int strikes;
    int points;
    bool singleCall;
    bool playOnce;

    // Use this for initialization
    void Start () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        singleCall = true;
        playOnce = true;
        levelOver = false;
        strikes = 0;
        points = 0;
        extraSpawnTime = 0.0f;

        //a random item from the list will be chosen
        keepItem = itemList[Random.Range(0, itemList.Length)].GetComponent<Item>().itemNum;

        pointsText.text = points + " / " + pointsRequired;
        strikeTextMax.text = " / " + maxStrikes;
        keepText.enabled = false;
        leverText.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K)) { //testing only
            levelOver = true;
            NextLevel();
        }

        //the level is lost and the belt stops moving
        if (strikes >= maxStrikes) {
            levelOver = true;
        }

        //start the timer
        if (LeverController.instance.start && singleCall) {
            singleCall = false;
            timeLeft = totalTime;
            keepText.enabled = true;
            leverText.enabled = false;
        }

        //update the timer text
        if (timeLeft > 0.0f && !levelOver) {
            timeLeft -= Time.deltaTime;
            int intTime = (int) Mathf.Ceil(timeLeft);

            string minutes = "";
            string seconds = "";

            if (intTime >= 60.0f) {
                int intMinutes = (int) Mathf.Floor(intTime / 60.0f);
                minutes = intMinutes.ToString();
                seconds = (intTime - (intMinutes * 60)).ToString("D2");
            } else {
                minutes = "0";
                seconds = intTime.ToString("D2");
            }

            timerText.text = minutes + ":" + seconds;

            //when timer says 20 or 30, play a whitsle sound
        }

        if (LeverController.instance.start && timeLeft <= 0.0f) {
            levelOver = true;
        }
    }

    //for ScreenController, tells it which item to display on the screen
    public int GetKeepItem() {
        return keepItem;
    }

    public void ItemDestroyed(int itemNum) {
        if (!levelOver) {
            if (itemNum != keepItem) {
                strikes++;
                strikesTextList[strikes - 1].color = Color.red;
                strikeText.text = "" + strikes;
                //play buzzer sound
            } else { //the correct item was kept
                points += pointsAmount;
                pointsText.text = points + " / " + pointsRequired;
                //play sound effect any time points are gained

                if (points >= pointsRequired) {    
                    if (playOnce) {
                        playOnce = false;
                        //when the point goal is achieved, play a sound
                    }

                    //decrease remaining time when keeping the correct item now that point goal is met
                    timeLeft -= 5;
                }
            }
        }
    }

    public int LevelLost() {
        if (strikes >= maxStrikes) {
            return 1; //too many strikes
        }else if (points < pointsRequired) {
            return 2; //not enough points
        }
        return 0; //level was not lost
    }

    public void NextLevel() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
	    //maybe play a transition sound
    }

    public void RetryLevel() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
