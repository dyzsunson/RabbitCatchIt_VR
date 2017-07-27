using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    // Timer
    public float gameTime;
    float m_time;
    public Text TimeText;
    public Text WaitTimeText;

    public Text TimeTextVR;
    public Text WaitTimeTextVR;

    private bool is_waiting = false;
    private bool is_running = false;
    private float m_waitTime = 5.0f;

    // Scene Object
    public Level[] levelList;
    static int s_currentLevel = 0;
    public Text LevelText;


    public GameObject RoamingCameraObj;
    public GameObject Firework;

    // UI Menu
    public GameObject StartMenu;
    public GameObject GameMenu;
    public GameObject EndMenu;

    public GameObject StartMenuVR;
    public GameObject GameMenuVR;
    public GameObject EndMenuVR;

    public GameObject TutorialMenu;

    public static SceneController context;

    public static Level Level_Current {
        get {
            return context.levelList[s_currentLevel];
        }
    }

    public static Rabbit Rabbit_Current {
        get {
            return Level_Current.rabbit;
        }
    }

    public static RabbitAI AI_Current {
        get {
            return Level_Current.AI;
        }
    }

    private void Awake() {
        context = this;
        s_currentLevel = NotChanged.context.Level_Current;
        print(s_currentLevel);
    }



    // Use this for initialization
    void Start () {
        StartMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
        StartMenuVR.GetComponent<FadeInOut>().FadeIn(1.0f);

        GameMenu.SetActive(false);
        GameMenuVR.SetActive(false);

        LevelChange(0);
     }
	
	// Update is called once per frame
	void Update () {
        EventSystem.current.SetSelectedGameObject(null);

        if (is_running) {
            m_time -= Time.deltaTime;
            if (m_time < 0.0f)
                GameEnd();
            else {
                TimeText.text = ((int)m_time).ToString();
                TimeTextVR.text = ((int)m_time).ToString();
            }
        }

        if (is_waiting) {
            m_waitTime -= Time.deltaTime;
            if (m_waitTime < 0.0f) {
                GameStart();
            }
            else if(m_waitTime < 1.0f) {
                if (WaitTimeText.text != "GO !") {
                    WaitTimeText.transform.Find("Sound2").GetComponent<AudioSource>().Play();
                }

                WaitTimeText.text = "GO !";
                WaitTimeTextVR.text = "GO !";
            }
            else if (m_waitTime < 4.0f) {
                if (WaitTimeText.text != ((int)m_waitTime).ToString()) {
                    WaitTimeText.transform.Find("Sound1").GetComponent<AudioSource>().Play();
                }

                WaitTimeText.text = ((int)m_waitTime).ToString();
                WaitTimeTextVR.text = ((int)m_waitTime).ToString();
            }
        }
	}

    public void ResetCamera() {
        UnityEngine.VR.InputTracking.Recenter();
    }

    public void Start_SinglePlayer() {
        InputCtrl.context.Is_AI_Ctrl = true;
        GameReady();
    }

    public void Start_MultiPlayer() {
        InputCtrl.context.Is_AI_Ctrl = false;
        GameReady();
    }

    void GameReady() {
        m_time = gameTime;
        is_waiting = true;
        // is_running = true;

        RoamingCameraObj.SetActive(false);
        GameMenu.SetActive(true);
        GameMenu.GetComponent<FadeInOut>().FadeIn(1.0f);

        GameMenuVR.SetActive(true);
        GameMenuVR.GetComponent<FadeInOut>().FadeIn(1.0f);

        StartMenu.SetActive(false);
        StartMenuVR.SetActive(false);

        this.levelList[s_currentLevel].GameReady();
        ResetCamera();
    }

    void GameStart() {
        TimeText.gameObject.SetActive(true);
        TimeText.text = ((int)m_time).ToString();

        TimeTextVR.transform.parent.gameObject.SetActive(true);
        TimeTextVR.text = ((int)m_time).ToString();

        is_running = true;
        is_waiting = false;

        WaitTimeText.transform.parent.gameObject.SetActive(false);
        WaitTimeTextVR.transform.parent.gameObject.SetActive(false);

        this.levelList[s_currentLevel].GameStart();

        if (InputCtrl.context.Is_AI_Ctrl)
            SceneController.AI_Current.GameStart();
    }

    public void GameQuit() {
        Application.Quit();
    }

    void GameEnd() {
        is_running = false;
        this.levelList[s_currentLevel].GameEnd();

        if (InputCtrl.context.Is_AI_Ctrl)
            SceneController.AI_Current.GameEnd();

        GameMenu.SetActive(false);
        GameMenuVR.SetActive(false);

        EndMenu.SetActive(true);
        Firework.SetActive(true);
        EndMenuVR.SetActive(true);

        EndMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
        EndMenuVR.GetComponent<FadeInOut>().FadeIn(1.0f);

        // score
        ScoreCalculate();
        this.GetComponent<AudioSource>().Play();
    }

    void ScoreCalculate() {
        SceneController.Level_Current.GetComponent<ScoreCalculation>().Calculate();
    }

    public void ReStartScene() {
        SceneManager.LoadScene(0);
    }

    public void ShowTutorial() {
        TutorialMenu.SetActive(true);
        TutorialMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
    }

    public void HideTutorial() {
        TutorialMenu.SetActive(false);
    }

    public void NextLevel() {
        this.LevelChange(1);
    }

    public void LastLevel() {
        this.LevelChange(-1);
    }

    private void LevelChange(int _offset) {
        levelList[s_currentLevel].gameObject.SetActive(false);
        s_currentLevel = (s_currentLevel + _offset + levelList.Length) % levelList.Length;
        levelList[s_currentLevel].gameObject.SetActive(true);

        NotChanged.context.Level_Current = s_currentLevel;

        LevelText.text = levelList[s_currentLevel].LevelName;
    }
}
