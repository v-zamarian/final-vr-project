// Victor Zamarian
// 3/19/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LeverController : MonoBehaviour {
    VRTK_SpringLever rotator;
    public ParticleSystem particles;

    public bool start;
    public static LeverController instance;

	// Use this for initialization
	void Start () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        start = false;
        rotator = GetComponentInChildren<VRTK_SpringLever>();
	}
	
	// Update is called once per frame
	void Update () {
        //start the belt movement after the lever is pulled a certain amount
        if (rotator.GetValue() > 20.0f) {
            start = true;
            Destroy(gameObject, 1.0f);
        }
	}

    void OnDestroy() {
        //play a sound effect as well
        GameController.instance.audioSources[4].Play();
        Debug.Log(Time.time + " poof sound played");

        particles.Play();
    }
}
