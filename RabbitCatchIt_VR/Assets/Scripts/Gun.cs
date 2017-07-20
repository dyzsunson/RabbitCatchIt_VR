using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public GameObject BulletPrefab;

    public Transform GunBodyTransform;
    public Transform FireTransform;
    public Transform StartTransform;

    float m_power = 0.0f;
    float m_max_power = 2.0f;
    float m_min_power = 0.4f;
    float m_scaler = 1200.0f;
    float m_max_reloadTime = 0.5f;
    float m_reloadTime = 0.0f;

    bool is_reloading = false;
    float m_reload_gun_speed = 1.0f;
    Vector3 m_gun_prePosition;

	// Use this for initialization
	void Start () {
        m_power = m_min_power;
        m_gun_prePosition = GunBodyTransform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (!is_reloading && Input.GetKey(KeyCode.Space)) {
            if (m_power < m_max_power)
                m_power += Time.deltaTime;
        }

        if (!is_reloading && Input.GetKeyUp(KeyCode.Space)) {
            Fire();           
            Reload();
        }

        if (is_reloading) {
            if (m_reloadTime > m_max_reloadTime) {
                ReloadEnd();
            }
            else {
                if (m_reloadTime < m_max_reloadTime / 2.0f) {
                    GunBodyTransform.Translate(-m_reload_gun_speed * Time.deltaTime * new Vector3(0.0f, 0.0f, 1.0f));
                }
                else {
                    GunBodyTransform.Translate(m_reload_gun_speed * Time.deltaTime * new Vector3(0.0f, 0.0f, 1.0f));
                }
                m_reloadTime += Time.deltaTime;
            }
            // this.transform.Translate()
        }
	}

    void Reload() {
        m_power = m_min_power;
        is_reloading = true;
        m_reloadTime = 0.0f;
    }

    void ReloadEnd() {
        GunBodyTransform.localPosition = m_gun_prePosition;
        is_reloading = false;
    }

    void Fire() {
        GameObject bullet = Instantiate(BulletPrefab) as GameObject;
        bullet.transform.position = FireTransform.position;
        bullet.GetComponent<Rigidbody>().AddForce(m_scaler * m_power * (FireTransform.position - StartTransform.position).normalized);
        this.GetComponent<AudioSource>().Play();
    }
}
