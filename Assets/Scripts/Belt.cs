// Victor Zamarian
// 4/2/18

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour {
    RigidbodyConstraints constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
    float speed;

	// Use this for initialization
	void Start () {
        speed = transform.parent.parent.GetComponent<BeltController>().speed;
	}
	
	// Update is called once per frame
	void Update () {
        speed = transform.parent.parent.GetComponent<BeltController>().speed;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Object")) {
            return;
        }
        
        collision.transform.GetComponent<Rigidbody>().constraints = constraints;
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Object")) {
            return;
        }

        if (speed == 0.0f) {
            collision.transform.GetComponent<Rigidbody>().constraints = constraints | RigidbodyConstraints.FreezePositionX;
        } else {
            collision.transform.GetComponent<Rigidbody>().constraints = constraints;
            collision.transform.GetComponent<Rigidbody>().velocity = speed * transform.right;
        }
    }

    private void OnCollisionExit(Collision collision){
        if (collision.gameObject.layer != LayerMask.NameToLayer("Object")){
            return;
        }

        collision.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
