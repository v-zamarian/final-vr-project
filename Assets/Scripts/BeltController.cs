// Victor Zamarian
// 4/6/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltController : MonoBehaviour {
    [HideInInspector]
    public float speed;

    float startingSpeed = 0.75f;
    float maxSpeed = 3.0f;

    public float speedIncrement; //public for now
    bool singleCall;
    bool singleCall2;

    float start;
    public float waitTime;

    // Use this for initialization
    void Start () {
        start = Time.time;
        speed = 0.0f;
        singleCall = true;
        singleCall2 = true;
    }

    // Update is called once per frame
    void Update () {
        if (LeverController.instance.start && singleCall) {
            singleCall = false;
            speed = startingSpeed;
            //start the belt sound effect
            GameController.instance.audioSources[5].Play();
            Debug.Log(Time.time + " belt sound started");

        }

        //increase the speed over time
        float current = Time.time;

        if (current - start > waitTime && speed < maxSpeed) {
            speed += speedIncrement;
            start = current;
            GameController.instance.extraSpawnTime += 0.25f;
        }

        if (GameController.instance.levelOver) {      
            speed = 0.0f;

            if (singleCall2) {
                singleCall2 = false;
                //play a powering down sound effect and stop the normal belt sound effect
                GameController.instance.audioSources[5].Stop();
                Debug.Log(Time.time + " belt sound stopped");
                GameController.instance.audioSources[6].Play();
                Debug.Log(Time.time + " power down sound played");
            }
        }
    }
}
