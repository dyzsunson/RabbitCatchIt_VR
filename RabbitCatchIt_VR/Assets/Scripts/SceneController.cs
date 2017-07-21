﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    float m_time;
    public Text TimeText;
    public Text WaitTimeText;
    public Text ScoreText;
    public Text RecordText;

    public Text TimeTextVR;
    public Text WaitTimeTextVR;
    public Text ScoreTextVR;
    public Text RecordTextVR;

    public Gun RabbitGun;

    public GameObject Rabbit;
    public GameObject Wall;
    public GameObject RoamingCameraObj;

    public GameObject StartMenu;
    public GameObject GameMenu;
    public GameObject EndMenu;

    public GameObject StartMenuVR;
    public GameObject GameMenuVR;
    public GameObject EndMenuVR;

    public GameObject Firework;

    public float gameTime;
    private bool is_waiting = false;
    private bool is_running = false;
    private float m_waitTime = 5.0f;

    private int m_totalEgg;
    private int m_hitEgg = 0;

    private int m_bulletFired_num = 0;
    private int m_bulletBlocked_num = 0;

    public static SceneController context;

    private void Awake() {
        context = this;
        StartMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
        StartMenuVR.GetComponent<FadeInOut>().FadeIn(1.0f);
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

    public static void BulletFired() {
        context.m_bulletFired_num++;
    }

    public static void BulletBlocked() {
        context.m_bulletBlocked_num++;
    }

    // Use this for initialization
    void Start () {
        GameMenu.SetActive(false);
        GameMenuVR.SetActive(false);
        // Rabbit.SetActive(false);
        // Wall.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
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
                TimeText.gameObject.SetActive(true);
                TimeText.text = ((int)m_time).ToString();

                TimeTextVR.transform.parent.gameObject.SetActive(true);
                TimeTextVR.text = ((int)m_time).ToString();

                is_running = true;
                is_waiting = false;

                WaitTimeText.transform.parent.gameObject.SetActive(false);
                WaitTimeTextVR.transform.parent.gameObject.SetActive(false);

                RabbitGun.Able_Fire = true;
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

    public void GameStart() {
        Rabbit.SetActive(true);
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

        RabbitGun.Able_Fire = false;

        ResetCamera();
    }

    public void GameQuit() {
        Application.Quit();
    }

    void GameEnd() {
        is_running = false;
        RabbitGun.Able_Fire = false;

        GameMenu.SetActive(false);
        GameMenuVR.SetActive(false);

        EndMenu.SetActive(true);
        Firework.SetActive(true);
        EndMenuVR.SetActive(true);

        EndMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
        EndMenuVR.GetComponent<FadeInOut>().FadeIn(1.0f);

        // score
        ScoreEnd();
        this.GetComponent<AudioSource>().Play();
    }

    void ScoreEnd() {
        string filePath = "Record/Record.txt";
        if (!Directory.Exists("Record")) {
            Directory.CreateDirectory("Record");
        }

        bool isNew = false;
        string fileTxt = "";

        if (!File.Exists(filePath)) {
            File.Create(filePath).Dispose(); ;
            isNew = true;
        }

        string[] lines = File.ReadAllLines(filePath);
        String todayStr = DateTime.Today.ToString("yyyyMMdd");

        
        if (lines.Length == 0)
            isNew = true;

        if (!isNew) {
            if (lines[0] != todayStr)
                isNew = true;
        }
        

        print(isNew);
        foreach (string str in lines)
            print(str);

        string recordStr = "";

        if (isNew)
            fileTxt += todayStr;
        else
            fileTxt += lines[0];
        fileTxt += "\r\n";

        if (isNew || ((m_totalEgg - m_hitEgg) > int.Parse(lines[1]))) {
            fileTxt += (m_totalEgg - m_hitEgg).ToString();
            recordStr += "NEW!";
            RecordText.transform.Find("Star1").gameObject.SetActive(true);
            RecordTextVR.transform.Find("Star1").gameObject.SetActive(true);
        }
        else {
            fileTxt += lines[1];
            recordStr += lines[1];
        }
        fileTxt += "\r\n";
        recordStr += "\r\n";

        if (isNew || (m_bulletFired_num > int.Parse(lines[2]))) {
            fileTxt += m_bulletFired_num.ToString();
            recordStr += "NEW!";
            RecordText.transform.Find("Star2").gameObject.SetActive(true);
            RecordTextVR.transform.Find("Star2").gameObject.SetActive(true);
        }
        else {
            fileTxt += lines[2];
            recordStr += lines[2];
        }
        fileTxt += "\r\n";
        recordStr += "\r\n";

        if (isNew || (m_bulletBlocked_num > int.Parse(lines[3]))) {
            fileTxt += m_bulletBlocked_num.ToString();
            recordStr += "NEW!";
            RecordText.transform.Find("Star3").gameObject.SetActive(true);
            RecordTextVR.transform.Find("Star3").gameObject.SetActive(true);
        }
        else {
            fileTxt += lines[3];
            recordStr += lines[3];
        }


        File.WriteAllText(filePath, fileTxt);
        

        ScoreText.text = ScoreTextVR.text = "Egg Remain: " + (m_totalEgg - m_hitEgg) + " / " + m_totalEgg + "\r\n" +
            "Bombs Fired: " + m_bulletFired_num + "\r\n" +
            "Bombs Blocked: " + m_bulletBlocked_num;

        RecordText.text = RecordTextVR.text = recordStr;
    }

    public void ReStartScene() {
        SceneManager.LoadScene(0);
    }
}
