﻿// Victor Zamarian
// 4/6/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder (90)]
public class ScreenController : MonoBehaviour {

    public GameObject[] itemCameras;
    public RenderTexture[] cameraTextures;

    public Text keepText;

    //still need to add text and buttons to these groups
    public GameObject levelOverObjs;
    public GameObject levelWonObjs;
    public GameObject levelLoss1Text; //strikes
    public GameObject levelLoss2Text; //points

    int cameraNum;
    bool singleCall;

	// Use this for initialization
	void Start () {
        singleCall = true;

        levelOverObjs.SetActive(false);
        levelWonObjs.SetActive(false);
        levelLoss1Text.SetActive(false);
        levelLoss2Text.SetActive(false);

        int keepItem = GameController.instance.GetKeepItem();

        GameObject[] itemList = (GameObject[]) GameController.instance.itemList.Clone();

        for (int i = 0; i < itemList.Length; i++) {
            if (itemList[i].GetComponent<Item>().itemNum == keepItem) {
                cameraNum = i;
                break;
            }
        }

        itemCameras[cameraNum].SetActive(true);

        transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = cameraTextures[cameraNum];
	}
	
	// Update is called once per frame
	void Update () {
		if (GameController.instance.levelOver && singleCall) {
            singleCall = false;
            SwapScreens(GameController.instance.LevelLost());
        }
	}

    void SwapScreens(int outcome) {
        keepText.enabled = false;
        levelOverObjs.SetActive(true);

        if (outcome == 0) { //level won
            levelWonObjs.SetActive(true);
        }else if (outcome == 1) { //too many strikes
            levelLoss1Text.SetActive(true);
        }else if (outcome == 2) { //not enough points
            levelLoss2Text.SetActive(true);
        }
    }
}
