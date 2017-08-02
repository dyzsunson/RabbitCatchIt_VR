using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour {
    public GameObject StartMenu;
    public GameObject GameMenu;
    public GameObject EndMenu;

    public GameObject StartMenuVR;
    public GameObject GameMenuVR;
    public GameObject EndMenuVR;

    public GameObject TutorialMenu;

    public GameObject Firework;

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
}
