// Victor Zamarian
// 3/23/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public float minItemTime;
    public float maxItemTime;

    GameObject[] itemList;

    bool singleCall;

	// Use this for initialization
	void Start () {
        singleCall = true;  
	}
	
	// Update is called once per frame
	void Update () {
		if (LeverController.instance.start && singleCall) {
            singleCall = false;
            StartCoroutine(SpawnItems());
        }
	}

    private IEnumerator SpawnItems() {
        //get the list of items from GameController
        itemList = (GameObject[])GameController.instance.itemList.Clone();

        //spawn items while the level is not over
        while (!GameController.instance.levelOver) {
            //randomly choose the item to spawn
            GameObject item = itemList[Random.Range(0, itemList.Length)];

            //randomly spawn the item in an area
            float xPos = Random.Range(-1.0f, 1.0f);
            float yPos = Random.Range(-1.0f, 1.0f);
            float zPos = Random.Range(-1.0f, 1.0f);
            Vector3 positionUpdate = new Vector3(xPos, yPos, zPos);

            Vector3 spawnPosition = positionUpdate + gameObject.transform.position;

            //spawn the item
            Instantiate(item, spawnPosition, Quaternion.identity);

            //wait a random amount of time before spawning another item
            float waitTime = Random.Range(minItemTime, maxItemTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
