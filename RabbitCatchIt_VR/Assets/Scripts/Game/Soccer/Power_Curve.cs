﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class Power_Curve : MonoBehaviour {
    LineRenderer line;
    int max_point_num = 80;
    int current_point_num = 0;
    float m_line_length = 0.075f;
    Vector3[] position_array;

    private void Awake() {
        line = this.GetComponent<LineRenderer>();    
    }

    // Use this for initialization
    void Start () {
        line.positionCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    Vector3 CalculateOnePoint(Vector3 P1, Vector3 T1, Vector3 P2, Vector3 T2, float s) {
        Matrix4x4 H = new Matrix4x4(new Vector4(2, -2, 1, 1), new Vector4(-3, 3, -2, -1), new Vector4(0, 0, 1, 0), new Vector4(1, 0, 0, 0));
        Matrix4x4 C = new Matrix4x4(new Vector4(P1.x, P1.y, P1.z, 1), new Vector4(P2.x, P2.y, P2.z, 1), new Vector4(T1.x, T1.y, T1.z, 0), new Vector4(T2.x, T2.y, T2.z, 0));
        // Matrix4x4 H = new Matrix4x4(new Vector4(2, -3, 0, 1), new Vector4(-2, 3, 0, 0), new Vector4(1, -2, 1, 0), new Vector4(1, -1, 0, 0));
        // Matrix4x4 C = new Matrix4x4(new Vector4(P1.x, P2.x, T1.x, T2.x), new Vector4(P1.y, P2.y, T1.y, T2.y), new Vector4(P1.z, P2.z, T1.z, T2.z), new Vector4(1, 1, 0, 0));
        // Matrix4x4 HC = H * C;
        Vector3 S = new Vector3(s * s * s, s * s, s);
        Vector3 SH = H.MultiplyPoint(S);
        return C.MultiplyPoint(SH);
    }

    public void SetPowerAndCurve(float _power, float _maxPower, float _curve, float _maxCurve) {
        current_point_num = (int) (max_point_num * (_power / _maxPower));

        line.positionCount = max_point_num;
        position_array = new Vector3[max_point_num];

        int cut = 0;

        Vector3 P1 = new Vector3(0.0f, 0.0f, m_line_length * cut);
        Vector3 T1 = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 P2 = new Vector3(2.0f * _curve / _maxCurve, 0.0f, m_line_length * max_point_num);
        Vector3 T2 = new Vector3(5.0f * _curve / _maxCurve, 0.0f, 1.0f);

        for (int i = 0; i <= cut; i++)
            position_array[i] = new Vector3(0.0f, 0.0f, m_line_length * i);
        for (int i = cut + 1; i < max_point_num; i++)
            position_array[i] = CalculateOnePoint(P1, T1, P2, T2, (i - cut) * 1.0f / max_point_num);
        //for (int i = current_point_num; i < max_point_num; i++)
          //  position_array[i] = P2; // new Vector3(0.0f, 0.0f, m_line_length * current_point_num);

        line.SetPositions(position_array);
    }
}