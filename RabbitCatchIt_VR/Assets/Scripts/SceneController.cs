using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    float m_time;
    public Text TimeText;
    public Text WaitTimeText;
    public Text ScoreText;

    public Gun RabbitGun;

    public GameObject Rabbit;
    public GameObject Wall;
    public GameObject RoamingCameraObj;

    public GameObject StartMenu;
    public GameObject GameMenu;
    public GameObject EndMenu;

    public float gameTime;
    private bool is_waiting = false;
    private bool is_running = false;
    private float m_waitTime = 5.0f;

    private int m_totalEgg;
    private int m_hitEgg = 0;

    public static SceneController context;

    private void Awake() {
        context = this;
        StartMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
    }

    public static int TotalEgg {
        set {
            context.m_totalEgg = value;
            context.m_hitEgg = 0;
        }
    }

    public static void EggBreak() {
        context.m_hitEgg++;
    }

    // Use this for initialization
    void Start () {
        GameMenu.SetActive(false);
        // Rabbit.SetActive(false);
        // Wall.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (is_running) {
            m_time -= Time.deltaTime;
            if (m_time < 0.0f)
                GameEnd();
            else
                TimeText.text = ((int)m_time).ToString();          
        }

        if (is_waiting) {
            m_waitTime -= Time.deltaTime;
            if (m_waitTime < 0.0f) {
                TimeText.gameObject.SetActive(true);
                TimeText.text = ((int)m_time).ToString();
                is_running = true;
                is_waiting = false;
                WaitTimeText.gameObject.SetActive(false);
                RabbitGun.Able_Fire = true;
            }
            else if(m_waitTime < 1.0f) {
                WaitTimeText.text = "GO !";
            }
            else if (m_waitTime < 4.0f) {
                WaitTimeText.text = ((int)m_waitTime).ToString();
            }
        }
	}

    public void GameStart() {
        Rabbit.SetActive(true);
        m_time = gameTime;
        is_waiting = true;
        // is_running = true;
        RoamingCameraObj.SetActive(false);
        GameMenu.SetActive(true);
        GameMenu.GetComponent<FadeInOut>().FadeIn(1.0f);

        StartMenu.SetActive(false);
        RabbitGun.Able_Fire = false;
    }

    void GameEnd() {
        is_running = false;
        RabbitGun.Able_Fire = false;
        GameMenu.SetActive(false);
        EndMenu.SetActive(true);

        EndMenu.GetComponent<FadeInOut>().FadeIn(1.0f);

        ScoreText.text = "Egg Remain: " + (m_totalEgg - m_hitEgg) + " / " + m_totalEgg;
        this.GetComponent<AudioSource>().Play();
    }

    public void ReStartScene() {
        SceneManager.LoadScene(0);
    }
}
