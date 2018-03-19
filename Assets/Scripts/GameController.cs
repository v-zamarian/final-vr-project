using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public bool levelOver;
    public static GameController instance;

	// Use this for initialization
	void Start () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        levelOver = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.K)) { //testing only
            levelOver = true;
        }
	}
}
