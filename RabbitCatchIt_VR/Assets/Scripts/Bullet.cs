using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    float m_lifeTime = 10.0f;

	// Use this for initialization
	void Start () {
        Invoke("End", m_lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
        if (this.transform.position.y < -1.1f)
            End();
	}

    void End() {
        Destroy(this.gameObject);
    }
}
