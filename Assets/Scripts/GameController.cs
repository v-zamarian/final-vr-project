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
    public Text strikesText;
    public Text timerText;
    public GameObject[] itemList;

    public static GameController instance;

    int keepItem;
    int strikes;

    //will be used later
    int points;
    int pointsRequired = 50;

	// Use this for initialization
	void Start () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        levelOver = false;
        strikes = 0;
        points = 0;

        //a random item from the list will be chosen
        keepItem = itemList[Random.Range(0, itemList.Length)].GetComponent<Item>().itemNum;
        print("KEEP ITEM: " + keepItem);
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

        //update countdown on screen
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
