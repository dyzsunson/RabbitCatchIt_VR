﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerShootController : ShootController {
    // float m_speed = 0.1f;
    float m_rotateSpeed = 0.0f;
    float m_max_rotateSpeed = 50.0f;
    float m_max_degree = 8.0f;
    float m_rotate_a = 250.0f;
    float m_pre_degree = 0.0f;

    float m_curve_power = 0.0f;

    public GameObject Curve_UI;
    Transform m_curve_ui_cursor;

    public Transform Build_Soccer_Position;
    private GameObject m_current_soccer;

    private Transform m_gun_body;
    public bool Is_powerHolding {
        get {
            return Able_Fire && InputCtrl.IsPowerButton;
        }
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();
        m_pre_degree = (this.transform.rotation.eulerAngles.x + 360) % 360;
        // m_scaler *= 1.2f;
        m_max_reloadTime *= 3.0f;
        m_allow_power_reloading = false;

        BuildSoccer();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        if (InputCtrl.IsUpButton) {
            if (m_rotateSpeed > -m_max_rotateSpeed) {
                m_rotateSpeed -= m_rotate_a * Time.deltaTime;
            }
        }
        else if (InputCtrl.IsDownButton) {
            if (m_rotateSpeed < m_max_rotateSpeed) {
                m_rotateSpeed += m_rotate_a * Time.deltaTime;
            }
        }
        else {
            m_rotateSpeed = 0.0f;
        }

        float rotationX = this.transform.rotation.eulerAngles.x;

        if (rotationX > 180.0f)
            rotationX -= 360.0f;

        if (rotationX < (m_pre_degree + m_max_degree) && m_rotateSpeed > 0.0f)
            this.transform.Rotate(this.transform.right, m_rotateSpeed * Time.deltaTime);
        else if (rotationX > (m_pre_degree - m_max_degree) && m_rotateSpeed < 0.0f)
            this.transform.Rotate(this.transform.right, m_rotateSpeed * Time.deltaTime);

        if (Able_Fire && !is_reloading) {
            if (Is_powerHolding) {
                if (InputCtrl.IsLeftButton)
                    m_curve_power -= 3.0f * Time.deltaTime;
                if (InputCtrl.IsRightButton)
                    m_curve_power += 3.0f * Time.deltaTime;

                if (m_curve_power > 1.0f)
                    m_curve_power = 1.0f;
                if (m_curve_power < -1.0f)
                    m_curve_power = -1.0f;
            }
            m_curve_ui_cursor.localPosition = new Vector3(m_curve_power * 25.0f, 0.0f, 0.0f);
        }

    }

    protected override void Reloading() {
        base.Reloading();
    }

    protected override void ReloadEnd() {
        base.ReloadEnd();
        BuildSoccer();
    }

    protected override void ResetFire() {
        base.ResetFire();
        m_curve_power = 0.0f;
    }

    protected override void Fire() {
        if (m_current_soccer == null)
            return;

        m_current_soccer.GetComponent<SoccerBall>().Fire(new Vector3(m_curve_power, 0.0f, 0.0f));
        FireOneBullet(m_current_soccer, m_power, new Vector3(m_curve_power * -0.5f, 0.0f, -0.3f));

        this.transform.parent.GetComponent<Rabbit>().Fire();

        m_curve_power = 0.0f;
        m_current_soccer = null;
    }

    public override void GameReady() {
        base.GameReady();
        Curve_UI.SetActive(true);
        m_curve_ui_cursor = Curve_UI.transform.Find("curve_cursor");
    }

    void BuildSoccer() {
        m_current_soccer = Instantiate(BulletPrefab);
        m_current_soccer.transform.position = Build_Soccer_Position.position;
    }
}