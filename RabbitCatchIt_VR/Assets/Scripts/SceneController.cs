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

    // Scene Controll
    private bool is_waiting = false;
    private bool is_running = false;
    private float m_waitTime = 5.0f;

    private bool is_vr_ready = false;
    private bool is_pc_ready = false;

    // Scene Object
    public Level[] levelList;
    static int s_currentLevel = 0;
    public Text LevelText;


    public GameObject RoamingCameraObj;

    // UI Menu
    public UI_Controller ui_controller;

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
    }



    // Use this for initialization
    void Start () {
        s_currentLevel = NotChanged.context.Level_Current;
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

#region public scene controll function
    public void ResetCamera() {
        UnityEngine.VR.InputTracking.Recenter();
    }

    public void PC_Ready() {
        is_pc_ready = true;
        if (is_vr_ready) {
            this.Start_MultiPlayer();
        }
    }

    public void VR_Ready() {
        is_vr_ready = true;
        if (is_pc_ready) {
            this.Start_MultiPlayer();
        }
    }

    public void Start_SinglePlayer() {
        InputCtrl.context.Is_AI_Ctrl = true;
        GameReady();
    }

    public void Start_MultiPlayer() {
        InputCtrl.context.Is_AI_Ctrl = false;
        GameReady();
    }

    public void GameQuit() {
        Application.Quit();
    }

    public void ReStartScene() {
        SceneManager.LoadScene(0);
    }

    public void ShowTutorial() {
        this.ui_controller.ShowTutorial();
    }

    public void HideTutorial() {
        this.ui_controller.HideTutorial();
    }

    public void NextLevel() {
        this.LevelChange(1);
    }

    public void LastLevel() {
        this.LevelChange(-1);
    }
    #endregion


#region private support functions
    void GameReady() {
        m_time = gameTime;
        is_waiting = true;
        // is_running = true;

        RoamingCameraObj.SetActive(false);

        this.ui_controller.GameReady();

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

    void GameEnd() {
        is_running = false;
        this.levelList[s_currentLevel].GameEnd();

        if (InputCtrl.context.Is_AI_Ctrl)
            SceneController.AI_Current.GameEnd();

        this.ui_controller.GameEnd();
        
        // score
        ScoreCalculate();
        this.GetComponent<AudioSource>().Play();
    }

    void ScoreCalculate() {
        SceneController.Level_Current.GetComponent<ScoreCalculation>().Calculate();
    }

    private void LevelChange(int _offset) {
        levelList[s_currentLevel].gameObject.SetActive(false);
        s_currentLevel = (s_currentLevel + _offset + levelList.Length) % levelList.Length;
        levelList[s_currentLevel].gameObject.SetActive(true);

        NotChanged.context.Level_Current = s_currentLevel;

        LevelText.text = levelList[s_currentLevel].LevelName;
    }
}
#endregion 
