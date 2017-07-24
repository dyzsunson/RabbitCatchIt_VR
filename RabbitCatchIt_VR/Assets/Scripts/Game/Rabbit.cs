using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour {
    public bool Is_running = false;
    ShootController m_gun;

    public ShootController ShootCtrl {
        get {
            return m_gun;
        }
    }

    // float m_speed = 0.1f;
    protected float m_rotateSpeed = 0.0f;
    protected float m_max_rotateSpeed = 50.0f;
    protected float m_max_degree = 0.15f;
    protected float m_rotate_a = 250.0f;
    protected float m_degree = 0.0f;

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


    void Awake() {
        m_gun = this.transform.Find("cannon").GetComponent<ShootController>();
    }

    // Use this for initialization
    protected virtual void Start() {
        
    }

    // Update is called once per frame
    protected virtual void Update() {
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

    public void GameReady() {
        Is_running = false;
        m_gun.Able_Fire = false;
        foreach (Skill skill in m_gun.skill_array)
            skill.UIObj.SetActive(true);
    }

    public void GameStart() {
        Is_running = true;
        m_gun.Able_Fire = true;
    }

    public void GameEnd() {
        Is_running = false;
        m_gun.Able_Fire = false;
    }

    public virtual void Fire() {

    }
}
