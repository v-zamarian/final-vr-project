// Victor Zamarian
// 2/20/18

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

    float start;
    public float waitTime;

    // Use this for initialization
    void Start () {
        start = Time.time;
        speed = 0.0f;
        singleCall = true;
    }

    // Update is called once per frame
    void Update () {
        if (LeverController.instance.start && singleCall) {
            singleCall = false;
            speed = startingSpeed;
        }

        //increase the speed over time
        float current = Time.time;

        if (current - start > waitTime && speed < maxSpeed) {
            speed += speedIncrement;
            start = current;
        }

        if (GameController.instance.levelOver) {
            //play a powering down sound effect
            speed = 0.0f;
        }
    }
}
