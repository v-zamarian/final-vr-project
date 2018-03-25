// Victor Zamarian
// 3/25/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder (90)]
public class ScreenController : MonoBehaviour {

    public GameObject[] itemCameras;
    public RenderTexture[] cameraTextures;

    int cameraNum;

	// Use this for initialization
	void Start () {
        int keepItem = GameController.instance.GetKeepItem();

        GameObject[] itemList = (GameObject[]) GameController.instance.itemList.Clone();

        for (int i = 0; i < itemList.Length; i++) {
            if (itemList[i].GetComponent<Item>().itemNum == keepItem) {
                cameraNum = i;
                break;
            }
        }

        print("CAMERA: " + cameraNum);
        itemCameras[cameraNum].SetActive(true);

        transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = cameraTextures[cameraNum];

        //put "Keep this item" on display screen
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //display level over screen and canvas after the level is won/lost
}
