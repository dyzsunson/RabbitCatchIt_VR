﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitAI : MonoBehaviour {
    // button
    public bool IsLeftHold;
    public bool IsRightHold;
    public bool IsUpHold;
    public bool IsDownHold;
    public bool IsPowerButtonHold;
    public bool IsPowerButtonUp;

    private float[] array = { -10.0f, 10.0f, -10.0f, 10.0f, -10.0f, 10.0f, -10.0f, 10.0f, -10.0f, 10.0f, -10.0f, 10.0f, -10.0f, 10.0f, -10.0f, 10.0f };
    private float[] power = { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };

    private float m_rotate_a = 0.1f;
    private float m_max_speed = 1.0f;

    private float m_y_rotate_a = 0.1f;
    private float m_y_max_speed = 1.0f;

    bool isWorking = false;
    public bool is_from_file = false;

    int m_current = 0;

    bool is_last_powerButtonHold = false;

    public KeyCode[] keyList;
    private Dictionary<KeyCode, bool> keyDownDictionary;
    private int m_current_skill = -1;
    private KeyCode m_lastKey = KeyCode.F1;
    private float m_min_skill_time = 10.0f;
    private float m_max_skill_time = 15.0f;
    float m_skill_reloading_time;

    // Use this for initialization
    void Start() {
        keyDownDictionary = new Dictionary<KeyCode, bool>();
        foreach (KeyCode key in keyList) {
            keyDownDictionary.Add(key, false);
        }

        m_skill_reloading_time = Random.Range(m_min_skill_time, m_max_skill_time);
    }

    // Update is called once per frame
    void Update() {
        if (!is_last_powerButtonHold && IsPowerButtonHold) {
            is_last_powerButtonHold = true;
        }

        if (is_last_powerButtonHold && !IsPowerButtonHold) {
            is_last_powerButtonHold = false;
            IsPowerButtonUp = true;
        }
        else {
            IsPowerButtonUp = false;
        }

        if (isWorking && keyList.Length > 0) {
            if (m_lastKey != KeyCode.F1) {
                keyDownDictionary[m_lastKey] = false;
                m_lastKey = KeyCode.F1;
            }

            m_skill_reloading_time -= Time.deltaTime;
            if (m_skill_reloading_time < 0.0f) {
                UseSkill();
            }
        }
    }

    void UseSkill() {
        m_current_skill++;
        m_current_skill %= keyList.Length;

        keyDownDictionary[keyList[m_current_skill]] = true;
        m_lastKey = keyList[m_current_skill];

        m_skill_reloading_time = Random.Range(m_min_skill_time, m_max_skill_time);
    }

    public bool IsHotKeyDown(KeyCode key) {
        if (!keyDownDictionary.ContainsKey(key))
            return false;
        else
            return keyDownDictionary[key];
    }

#region rotation
    void RotateNext() {
        if (!isWorking)
            return;

        if (is_from_file) {
            m_current++;
            if (m_current >= array.Length)
                return;
        }

        float ty = RotateAroundY();
        float tx = RotateAroundX();

        Invoke("WaitAfterRotate", Mathf.Max(tx, ty));
    }

    float RotateAroundY() {
        float startDegree = SceneController.Rabbit_Current.transform.rotation.eulerAngles.y;
        if (startDegree > 180.0f)
            startDegree -= 360.0f;

        float endDegree = 0.0f;
        if (is_from_file) {
            endDegree = array[m_current];
        }
        else {
            endDegree = Random.Range(-10.0f, 10.0f);
        }
        float t = TimeRotateFromTo(startDegree, endDegree, m_max_speed, m_rotate_a);
        if (endDegree > startDegree)
            IsRightHold = true;
        else
            IsLeftHold = true;

        Invoke("RotateXEnd", t);

        return t;
    }

    float RotateAroundX() {
        float startDegree = SceneController.Rabbit_Current.ShootCtrl.GunBodyTransform.rotation.eulerAngles.x;
        if (startDegree > 180.0f)
            startDegree -= 360.0f;

        float endDegree = 0.0f;
        if (is_from_file) {
            endDegree = array[m_current];
        }
        else {
            endDegree = Random.Range(-5.0f, 5.0f);
        }
        float t = TimeRotateFromTo(startDegree, endDegree, m_max_speed, m_rotate_a);

        if (endDegree < startDegree)
            IsUpHold = true;
        else
            IsDownHold = true;

        print("StartDegree" + startDegree + " " + "EndDegree" + endDegree);

        Invoke("RotateYEnd", t);

        return t;
    }

    float TimeRotateFromTo(float _start, float _end, float _speed, float _a) {
        float t = 0.0f;

        float dis = Mathf.Abs(_end - _start);
        if (dis < 5.0f) {
            t = Mathf.Sqrt(dis * 2.0f / 250.0f);
        }
        else {
            t = (dis - 5.0f) / 50.0f + 0.2f;
        }

        return t;
    }

    void RotateXEnd() {
        IsLeftHold = IsRightHold = false;
    }

    void RotateYEnd() {
        IsUpHold = IsDownHold = false;
    }

    void WaitAfterRotate() {
        if (is_from_file) {
            SceneController.Rabbit_Current.transform.rotation = Quaternion.Euler(new Vector3(0.0f, array[m_current], 0.0f));
        }

        IsUpHold = IsDownHold = IsLeftHold = IsRightHold = false;

        float t = 0.1f;
        Invoke("Fire", t);
    }
#endregion

#region FIRE
    void Fire() {
        IsPowerButtonHold = true;

        float t;
        if (is_from_file) {
            t = power[m_current];
        }
        else {
            float low = 0.4f;
            float high = 0.75f;

            float degree = SceneController.Rabbit_Current.ShootCtrl.GunBodyTransform.rotation.eulerAngles.x;
            if (degree > 180.0f)
                degree -= 360.0f;

            if (degree > 5.0f)
                degree = 5.0f;
            if (degree < -5.0f)
                degree = -5.0f;

            if (degree < 0.0f)
                high -= 0.25f * (-degree) / 5.0f;
            else
                low += 0.2f * (degree) / 5.0f;

            t = Random.Range(low, high);
        }

        Invoke("WaitAfterFire", t);
    }

    void WaitAfterFire() {
        IsPowerButtonHold = false;

        float t = Random.Range(0.1f, 0.3f);
        Invoke("RotateNext", t);
    }
#endregion

    public void GameStart() {
        isWorking = true;
        m_current = -1;
        RotateNext();
    }

    public void GameEnd() {
        isWorking = false;
    }
}