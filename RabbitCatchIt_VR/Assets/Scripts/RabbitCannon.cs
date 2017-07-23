using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitCannon : MonoBehaviour {
    // float m_speed = 0.1f;
    float m_rotateSpeed = 0.0f;
    float m_max_rotateSpeed = 50.0f;
    float m_max_degree = 0.15f;
    float m_rotate_a = 250.0f;
    float m_degree = 0.0f;

    public float Max_RotateSpeed {
        get {
            return this.m_max_rotateSpeed;
        }
    }

    public float Rotate_A {
        get {
            return this.m_rotate_a;
        }
    }

    private Animator animator;

    bool is_stable_speed = false;

    public bool Is_running = false;
    Gun m_gun;

    private void Awake() {
        m_gun = this.transform.Find("cannon").GetComponent<Gun>();
    }

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        if (is_stable_speed)
            m_max_degree = 30.0f;
    }

    // Update is called once per frame
    void Update() {
        if (!is_stable_speed) {
            if (InputCtrl.IsLeftButton) {
                if (m_rotateSpeed > -m_max_rotateSpeed) {
                    m_rotateSpeed -= m_rotate_a * Time.deltaTime;
                }
            }
            else if (InputCtrl.IsRightButton) {
                if (m_rotateSpeed < m_max_rotateSpeed) {
                    m_rotateSpeed += m_rotate_a * Time.deltaTime;
                }
            }
            else {
                m_rotateSpeed = 0.0f;
            }

            if (this.transform.rotation.y < m_max_degree && m_rotateSpeed > 0.0f)
                this.transform.Rotate(this.transform.up, m_rotateSpeed * Time.deltaTime);
            else if (this.transform.rotation.y > -m_max_degree && m_rotateSpeed < 0.0f)
                this.transform.Rotate(this.transform.up, m_rotateSpeed * Time.deltaTime);
        }
        /*
        else {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                if (m_degree > -m_max_degree) {
                    this.transform.Rotate(this.transform.up, -m_max_rotateSpeed);
                    m_degree -= m_max_rotateSpeed * Time.deltaTime;
                }
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                if (m_degree < m_max_degree) {
                    this.transform.Rotate(this.transform.up, m_max_rotateSpeed);
                    m_degree += m_max_rotateSpeed * Time.deltaTime;
                }
            }
        }
        */
    }

    public void GameReady() {
        Is_running = false;
        m_gun.Able_Fire = false;
    }

    public void GameStart() {
        Is_running = true;
        m_gun.Able_Fire = true;
    }

    public void GameEnd() {
        Is_running = false;
        m_gun.Able_Fire = false;
    }

    public void Fire() {
        animator.Play("Base Layer.Hit");
    }
}
