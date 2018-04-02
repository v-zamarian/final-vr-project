// Victor Zamarian
// 3/26/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    void Awake() {
        //fade from black
    }

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
            print(Time.time + " " + itemNum);
            strikes++;
            strikesText.text = " X" + strikesText.text;
            //play buzzer sound
        } else { //the correct item was kept
            points += pointsAmount;
            pointsText.text = points + " / " + pointsRequired;
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
        //have headset fade to black
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RetryLevel() {
        //have headset fade to black
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
