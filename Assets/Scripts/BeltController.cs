// Victor Zamarian
// 2/20/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltController : MonoBehaviour {
    //[HideInInspector]
    public float speed;

    public float startingSpeed; //testing only, set to 0.0 later

    public float speedIncrement; //public for now
    bool singleCall;

    float start;
    float waitTime = 5.0f;

    // Use this for initialization
    void Start () {
        start = Time.time;
        speed = startingSpeed;
        singleCall = true;
    }

    // Update is called once per frame
    void Update () {
        if (LeverController.instance.start && singleCall) {
            singleCall = false;
            speed = speedIncrement;
        }

        //increase the speed over time, implemented later
        /*float current = Time.time;

        if (current - start > waitTime) {
            speed += speedIncrement;
            start = current;
        }*/
    }
}
