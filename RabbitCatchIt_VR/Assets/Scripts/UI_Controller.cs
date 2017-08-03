﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour {
    #region public value from outside
    public GameObject StartMenu;
    public GameObject GameMenu;
    public GameObject EndMenu;

    public GameObject StartMenuVR;
    public GameObject GameMenuVR;
    public GameObject EndMenuVR;

    public GameObject TutorialMenu;

    public GameObject Firework;
    #endregion

    #region private value
    GameObject PC_Waiting_obj;
    GameObject VR_Waiting_obj;
    GameObject PC_Ready_obj;
    GameObject VR_Ready_obj;

    GameObject PC_Ready_Button;
    GameObject PC_Cancel_Button;

    Text LevelText;
    Image LevelIcon;
    #endregion

    private void Awake() {
        PC_Waiting_obj = StartMenu.transform.Find("Prepare/PC_Waiting").gameObject;

        VR_Waiting_obj = StartMenu.transform.Find("Prepare/VR_Waiting").gameObject;
        PC_Ready_obj = StartMenu.transform.Find("Prepare/PC_Ready").gameObject;
        VR_Ready_obj = StartMenu.transform.Find("Prepare/VR_Ready").gameObject;

        PC_Ready_Button = StartMenu.transform.Find("Battle").gameObject;
        PC_Cancel_Button = StartMenu.transform.Find("Battle_Cancel").gameObject;

        LevelText = StartMenu.transform.Find("Level/LevelText").GetComponent<Text>();
        LevelIcon = StartMenu.transform.Find("Level/Icon").GetComponent<Image>();
    }

    // Use this for initialization
    void Start() {
        StartMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
        StartMenuVR.GetComponent<FadeInOut>().FadeIn(1.0f);

        GameMenu.SetActive(false);
        GameMenuVR.SetActive(false);

        EndMenu.SetActive(false);
        EndMenuVR.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    public void PC_Ready() {
        PC_Waiting_obj.SetActive(false);
        PC_Ready_obj.SetActive(true);

        PC_Ready_Button.SetActive(false);
        PC_Cancel_Button.SetActive(true);
    }

    public void VR_Ready() {
        VR_Waiting_obj.SetActive(false);
        VR_Ready_obj.SetActive(true);
    }

    public void PC_Cancel() {
        PC_Waiting_obj.SetActive(true);
        PC_Ready_obj.SetActive(false);

        PC_Ready_Button.SetActive(true);
        PC_Cancel_Button.SetActive(false);
    }

    public void VR_Cancel() {
        VR_Waiting_obj.SetActive(true);
        VR_Ready_obj.SetActive(false);
    }

    public void GameReady() {
        GameMenu.SetActive(true);
        GameMenu.GetComponent<FadeInOut>().FadeIn(1.0f);

        GameMenuVR.SetActive(true);
        GameMenuVR.GetComponent<FadeInOut>().FadeIn(1.0f);

        StartMenu.SetActive(false);
        StartMenuVR.SetActive(false);
    }

    public void GameStart() {

    }

    public void GameEnd() {
        GameMenu.SetActive(false);
        GameMenuVR.SetActive(false);

        EndMenu.SetActive(true);
        EndMenuVR.SetActive(true);

        Firework.SetActive(true);

        EndMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
        EndMenuVR.GetComponent<FadeInOut>().FadeIn(1.0f);
    }

    public void ShowTutorial() {
        TutorialMenu.SetActive(true);
        TutorialMenu.GetComponent<FadeInOut>().FadeIn(1.0f);
    }

    public void HideTutorial() {
        TutorialMenu.SetActive(false);
    }

    public void LevelChange(string _name, Sprite _sprite) {
        LevelText.text = _name;
        LevelIcon.sprite = _sprite;
    }
}
