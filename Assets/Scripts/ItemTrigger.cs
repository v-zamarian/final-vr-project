// Victor Zamarian
// 2/20/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Object")) {
            Destroy(other.gameObject);
        }
    }
}
