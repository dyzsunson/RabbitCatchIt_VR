using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour {
    float m_time = 60.0f;
    public Text TimeText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        m_time -= Time.deltaTime;
        TimeText.text = ((int)m_time).ToString();
	}
}
