using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
    public GameObject BulletPrefab;
    public GameObject BigBulletPrefab;
    public GameObject NaughtyBulletPrefab;
    public GameObject ThreeBulletPrefab;

    public Transform GunBodyTransform;
    public Transform FireTransform;
    public Transform StartTransform;

    public PowerUI powerUI;

    float m_power = 0.0f;
    float m_max_power = 1.25f;
    float m_min_power = 0.5f;
    float m_scaler = 1200.0f;
    float m_max_reloadTime = 0.5f;
    float m_reloadTime = 0.0f;

    bool is_reloading = false;
    float m_reload_gun_speed = 1.0f;
    Vector3 m_gun_prePosition;

    bool able_fire = false;

    public Skill[] skill_array;
    enum SkillID {
        BigBullet = 0, 
        ThreeBullet = 1,
        NaughtyBullet = 2
    }

    float m_firePower = 0.0f;


    public bool Able_Fire {
        set {
            able_fire = value;
        }
        get {
            return able_fire;
        }
    }

    public static Gun context;

    private void Awake() {
        context = this;
    }

    // Use this for initialization
    void Start () {
        m_power = m_min_power;
        m_gun_prePosition = GunBodyTransform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        if (able_fire) {
            if (!is_reloading && Input.GetKey(KeyCode.Space)) {
                if (m_power < m_max_power)
                    m_power += Time.deltaTime;
            }


            powerUI.SetPower((int)(((m_power - m_min_power) / (m_max_power - m_min_power)) * 100));

            if (!is_reloading && Input.GetKeyUp(KeyCode.Space)) {
                Fire();
                Reload();
            }
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

    void FireABullet(bool _isThreeBullet) {
        GameObject bullet = null;
        if (_isThreeBullet)
            bullet = Instantiate(ThreeBulletPrefab) as GameObject;
        else if (skill_array[(int) SkillID.BigBullet].is_working)
            bullet = Instantiate(BigBulletPrefab) as GameObject;
        else if (skill_array[(int)SkillID.NaughtyBullet].is_working)
            bullet = Instantiate(NaughtyBulletPrefab) as GameObject;
        else
            bullet = Instantiate(BulletPrefab) as GameObject;

        bullet.transform.position = FireTransform.position;

        if (skill_array[(int)SkillID.BigBullet].is_working)
            bullet.GetComponent<Rigidbody>().AddForce(100.0f * m_scaler * m_firePower * (FireTransform.position - StartTransform.position).normalized);
        else
            bullet.GetComponent<Rigidbody>().AddForce(m_scaler * m_firePower * (FireTransform.position - StartTransform.position).normalized);

        this.GetComponent<AudioSource>().Play();
    }

    void FireAThreeBullet() {
        FireABullet(true);
    }

    void Fire() {
        m_firePower = m_power;
        
        if (skill_array[(int)SkillID.ThreeBullet].is_working) {
            FireAThreeBullet();
            Invoke("FireAThreeBullet", 0.2f);
            Invoke("FireAThreeBullet", 0.4f);
        }
        else
            FireABullet(false);

        this.transform.parent.GetComponent<RabbitCannon>().Fire();
        SceneController.BulletFired();
    }
}
