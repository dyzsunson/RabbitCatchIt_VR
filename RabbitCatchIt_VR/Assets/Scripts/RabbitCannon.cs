using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitCannon : MonoBehaviour {
    float m_speed = 0.1f;
    float m_rotateSpeed = 1.0f;
    float m_max_degree = 30.0f;
    float m_degree = 0.0f;

    private Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        /*if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            this.transform.Translate(m_speed * -this.transform.right);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            this.transform.Translate(m_speed * this.transform.right);
        }*/

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            if (m_degree > -m_max_degree) {
                this.transform.Rotate(this.transform.up, -m_rotateSpeed);
                m_degree -= m_rotateSpeed;
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            if (m_degree < m_max_degree) {
                this.transform.Rotate(this.transform.up, m_rotateSpeed);
                m_degree += m_rotateSpeed;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            Fire();
        }
    }

    public void Fire() {
        animator.Play("Base Layer.Hit");
    }
}
