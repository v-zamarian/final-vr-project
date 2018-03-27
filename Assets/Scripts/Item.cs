// Victor Zamarian
// 3/26/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    public int itemNum;

    void Start() {
        if (itemNum != 0) //testing only
            Destroy(gameObject, 30.0f);
    }
}
