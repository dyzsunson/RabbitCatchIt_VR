using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour {
    public bool Is_running = false;
    Gun m_gun;

    void Awake() {
        m_gun = this.transform.Find("cannon").GetComponent<Gun>();
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
}
