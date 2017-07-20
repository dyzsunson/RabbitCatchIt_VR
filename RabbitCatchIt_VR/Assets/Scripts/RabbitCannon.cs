using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitCannon : MonoBehaviour {
    float m_speed = 0.1f;
    float m_rotateSpeed = 0.0f;
    float m_max_rotateSpeed = 1.0f;
    float m_max_degree = 0.15f;
    float m_rotate_a = 0.1f;
    float m_degree = 0.0f;

    private Animator animator;

    bool is_stable_speed = false;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        if (is_stable_speed)
            m_max_degree = 30.0f;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!is_stable_speed) {
            if (this.transform.rotation.y < m_max_degree && m_rotateSpeed > 0.0f)
                this.transform.Rotate(this.transform.up, m_rotateSpeed);
            else if (this.transform.rotation.y > -m_max_degree && m_rotateSpeed < 0.0f)
                this.transform.Rotate(this.transform.up, m_rotateSpeed);

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                if (m_rotateSpeed > -m_max_rotateSpeed) {
                    m_rotateSpeed -= m_rotate_a;
                }
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                if (m_rotateSpeed < m_max_rotateSpeed) {
                    m_rotateSpeed += m_rotate_a;
                }
            }
            else {
                m_rotateSpeed = 0.0f;
            }
        }

        else {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                if (m_degree > -m_max_degree) {
                    this.transform.Rotate(this.transform.up, -m_max_rotateSpeed);
                    m_degree -= m_max_rotateSpeed;
                }
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                if (m_degree < m_max_degree) {
                    this.transform.Rotate(this.transform.up, m_max_rotateSpeed);
                    m_degree += m_max_rotateSpeed;
                }
            }
        }
        

        /*if (Input.GetKeyUp(KeyCode.Space)) {
            Fire();
        }*/
    }

    public void Fire() {
        animator.Play("Base Layer.Hit");
    }
}
