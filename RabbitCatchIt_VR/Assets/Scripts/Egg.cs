using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour {
    bool isBreak = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Bullet") {
            this.GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;

            if (!isBreak) {
                SceneController.EggBreak();
                isBreak = true;
            }
            // Destroy(this.gameObject);
        }
    }
}
