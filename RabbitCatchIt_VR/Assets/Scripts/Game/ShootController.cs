using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour {
    protected bool able_fire = false;

    public bool Able_Fire {
        set {
            able_fire = value;
        }
        get {
            return able_fire;
        }
    }

    // fire obj
    public GameObject BulletPrefab;
    public Transform GunBodyTransform;
    public Transform FireTransform;
    public Transform StartTransform;

    public PowerUI powerUI;

    public Skill[] skill_array;

    // power variable
    protected float m_power = 0.0f;
    protected float m_max_power = 1.25f;
    protected float m_min_power = 0.5f;
    protected float m_scaler = 1200.0f;

    // reload
    protected float m_max_reloadTime = 0.5f;
    protected float m_reloadTime = 0.0f;

    protected bool is_reloading = false;


    // Use this for initialization
    protected virtual void Start () {
        m_power = m_min_power;
    }

    // Update is called once per frame
    protected virtual void Update () {
        if (able_fire) {
            if (InputCtrl.IsPowerButton) { // !is_reloading && 
                if (m_power < m_max_power)
                    m_power += Time.deltaTime;
            }

            powerUI.SetPower((int)(((m_power - m_min_power) / (m_max_power - m_min_power)) * 100));

            if (!is_reloading && InputCtrl.IsPowerButtonUp) {
                Fire();
                StartReload();
            }
        }

        if (is_reloading) {
            if (m_reloadTime > m_max_reloadTime) {
                ReloadEnd();
            }
            else {
                Reloading();
            }
            // this.transform.Translate()
        }

    }

    void StartReload() {
        m_power = m_min_power;
        is_reloading = true;
        m_reloadTime = 0.0f;
    }

    protected virtual void ReloadEnd() {
        is_reloading = false;
    }

    protected virtual void Reloading() {
        m_reloadTime += Time.deltaTime;
    }

    protected void FireOneBullet(GameObject _obj, float _power) {
        _obj.transform.position = FireTransform.position;
        _obj.GetComponent<Rigidbody>().AddForce(m_scaler * _power * (FireTransform.position - StartTransform.position).normalized);

        this.GetComponent<AudioSource>().Play();
        SceneController.BulletFired();
    }

    protected virtual void Fire() {
        GameObject obj = Instantiate(BulletPrefab);
        FireOneBullet(obj, m_power);
        this.transform.parent.GetComponent<Rabbit>().Fire();
    }
}
