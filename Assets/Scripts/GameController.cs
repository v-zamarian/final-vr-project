// Victor Zamarian
// 3/26/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VRTK;

[DefaultExecutionOrder (1)]
public class GameController : MonoBehaviour {

    public bool levelOver;
    public int maxStrikes;
    public float totalTime;
    public Text strikesText;
    public Text timerText;
    public Text pointsText;
    public int pointsRequired;
    public int pointsAmount; //amount to increase points by
    public GameObject[] itemList;

    public static GameController instance;

    float timeLeft = 0.0f;
    int keepItem;
    int strikes;
    int points;
    bool singleCall;

    VRTK_HeadsetFade headsetFade = new VRTK_HeadsetFade();

    // Use this for initialization
    void Start () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        singleCall = true;
        levelOver = false;
        strikes = 0;
        points = 0;

        //a random item from the list will be chosen
        keepItem = itemList[Random.Range(0, itemList.Length)].GetComponent<Item>().itemNum;
        print("KEEP ITEM: " + keepItem);

        pointsText.text = points + " / " + pointsRequired;

        //headsetFade.Unfade(1.0f);
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K)) { //testing only
            levelOver = true;
            NextLevel();
        }

        if (Input.GetKeyDown(KeyCode.O)) {
            headsetFade.Fade(Color.black, 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            headsetFade.Unfade(0.5f);
        }

        //the level is lost and the belt stops moving
        if (strikes >= maxStrikes) {
            levelOver = true;
            print("LEVEL OVER");
        }

        //start the timer
        if (LeverController.instance.start && singleCall) {
            singleCall = false;
            timeLeft = totalTime;
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
        if (itemNum != keepItem) {
            strikes++;
            strikesText.text = " X" + strikesText.text;
            //play buzzer sound
        } else { //the correct item was kept
            points += pointsAmount;
            pointsText.text = points + " / " + pointsRequired;
        //play sound effect any time points are gained
        //when the point goal is achieved, play a sound

        //when point goal is achieved, then decrease remaining time by some amount when keeping the correct item
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
        //have headset fade to black?
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	    //maybe play a transition sound
    }

    public void RetryLevel() {
        //have headset fade to black?
        //headsetFade.Fade(Color.black, 1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
