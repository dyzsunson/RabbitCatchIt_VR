using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    public Rabbit rabbit;
    public GameObject GoalObj;

    public string LevelName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameReady() {
        rabbit.GameReady();
    }

    public void GameStart() {
        rabbit.GameStart();
    }

    public void GameEnd() {
        rabbit.GameEnd();
    }
}
