// Victor Zamarian
// 3/23/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder (1)]
public class GameController : MonoBehaviour {

    public bool levelOver;
    public int maxStrikes;
    public GameObject[] itemList;

    public static GameController instance;

    int keepItem;
    int strikes;

	// Use this for initialization
	void Start () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        levelOver = false;
        strikes = 0;

        //a random item from the list will be chosen
        keepItem = itemList[Random.Range(0, itemList.Length)].GetComponent<Item>().itemNum;
        print("KEEP ITEM: " + keepItem);
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K)) { //testing only
            levelOver = true;
        }

        //the level is lost and the belt stops moving
        if (strikes >= maxStrikes) {
            levelOver = true;
            print("LEVEL OVER");
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
        }
    }
}
