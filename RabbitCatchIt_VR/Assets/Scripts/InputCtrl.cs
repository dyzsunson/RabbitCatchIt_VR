using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCtrl : MonoBehaviour {
    public static InputCtrl context;
    public CannonAI cannonAI;

    public bool Is_AI_Ctrl = false;

    public static bool IsLeftButton {
        get {
            if (context.Is_AI_Ctrl)
                return context.cannonAI.IsLeftHold;
            else
                return Input.GetKey(KeyCode.LeftArrow); 
        }
    }

    public static bool IsRightButton {
        get {
            if (context.Is_AI_Ctrl)
                return context.cannonAI.IsRightHold;
            else
                return Input.GetKey(KeyCode.RightArrow);
        }
    }

    public static bool IsPowerButton {
        get {
            if (context.Is_AI_Ctrl)
                return context.cannonAI.IsPowerButtonHold;
            else
                return Input.GetKey(KeyCode.Space);
        }
    }

    public static bool IsPowerButtonUp {
        get {
            if (context.Is_AI_Ctrl)
                return context.cannonAI.IsPowerButtonUp;
            else
                return Input.GetKeyUp(KeyCode.Space);
        }
    }

    private void Awake() {
        context = this;
    }

    public static bool IsHotKeyDown(KeyCode _key) {
        if (context.Is_AI_Ctrl)
            return context.cannonAI.IsHotKeyDown(_key);
        else
            return Input.GetKeyDown(_key);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
