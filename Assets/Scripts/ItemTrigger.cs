// Victor Zamarian
// 3/23/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object")) {
            GameController.instance.ItemDestroyed(other.gameObject.GetComponent<Item>().itemNum);
            Destroy(other.gameObject);
        }
    }
}
