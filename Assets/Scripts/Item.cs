// Victor Zamarian
// 4/4/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    public int itemNum;

    void Start() {
        Destroy(gameObject, 30.0f);
    }
}
