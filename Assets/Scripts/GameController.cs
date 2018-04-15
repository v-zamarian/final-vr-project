// Victor Zamarian
// 4/6/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VRTK;
using VOA = VRTK.VRTK_ObjectAppearance;

[DefaultExecutionOrder (1)]
public class GameController : MonoBehaviour {

    [HideInInspector]
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
    public GameObject faderObjSim;
    public GameObject faderObjRift;
    public GameObject[] itemList;

    //Sound Effects
    //GameController: whistle at 20/30 seconds[3], buzzer on incorrect item[1], points gained[0],
    //goal points reached[2], transition sound(?)[7]
    //LeverController: lever destroyed[4]
    //BeltController: constant belt sound[5], belt power down[6]
    public AudioSource[] audioSources;

    public static GameController instance;

    float timeLeft = 0.0f;
    int keepItem;
    int strikes;
    int points;
    bool singleCall;
    bool singleCall2;
    bool singleCall3;
    bool playOnce;
    bool playOnce2;

    // Use this for initialization
    void Start () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        singleCall = true;
        singleCall2 = true;
        singleCall3 = true;
        playOnce = true;
        playOnce2 = true;
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

        StartCoroutine(StartFadeIn());
    }
	
	// Update is called once per frame
	void Update () {
        if (faderObjRift.GetComponent<Renderer>().material.color.a <= 0.0f && singleCall2) {
            singleCall2 = false;
            faderObjRift.transform.Translate(-0.5f * transform.forward);
        }

        if (faderObjSim.GetComponent<Renderer>().material.color.a <= 0.0f && singleCall3) {
            singleCall3 = false;
            faderObjSim.transform.Translate(-1.0f * transform.forward);
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

            if (timeLeft <= 30.0f && playOnce) {
                playOnce = false;
                //when timer says 30, play a whitsle sound
                audioSources[3].Play();
                Debug.Log(Time.time + " whistle sound played");
            }
        }

        if (LeverController.instance.start && timeLeft <= 0.0f) {
            levelOver = true;
            timerText.text = "0:00";
        }
    }

    //fade screen from black at the start of the scene
    IEnumerator StartFadeIn() {
        yield return new WaitForSeconds(0.5f);
        FadeIn();
    }

    //fade screen from black
    void FadeIn() {
        VOA.SetOpacity(faderObjRift, 0.0f, 1.5f);
        VOA.SetOpacity(faderObjSim, 0.0f, 1.5f);
    }

    //fade screen to black
    void FadeOut() {
        faderObjRift.transform.Translate(1.0f * transform.forward);
        faderObjSim.transform.Translate(1.0f * transform.forward);
        VOA.SetOpacity(faderObjRift, 0.5f, 1.5f);
        VOA.SetOpacity(faderObjSim, 1.0f, 1.5f);
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
                audioSources[1].Play();
                Debug.Log(Time.time + " buzzer sound played");

            } else { //the correct item was kept
                points += pointsAmount;
                pointsText.text = points + " / " + pointsRequired;
                //play sound effect any time points are gained
                audioSources[0].Play();
                Debug.Log(Time.time + " point gain sound played");

                if (points >= pointsRequired) {    
                    if (playOnce2) {
                        playOnce2 = false;
                        //when the point goal is achieved, play a sound
                        audioSources[2].Play();
                        Debug.Log(Time.time + " goal sound played");
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

    //go to the next level
    public void NextLevel() {
        StartCoroutine(NextLevelCo());
    }

    IEnumerator NextLevelCo() {
        FadeOut();
        //maybe play transition sound effect
        audioSources[7].Play();
        Debug.Log(Time.time + " transition sound played");

        yield return new WaitForSeconds(1.6f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //retry the current level
    public void RetryLevel() {
        StartCoroutine(RetryLevelCo());
    }

    IEnumerator RetryLevelCo() {
        FadeOut();
        yield return new WaitForSeconds(1.6f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    //quit the game
    public void QuitGame() {
        Application.Quit();
    }
}
