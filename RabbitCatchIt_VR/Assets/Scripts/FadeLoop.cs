using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeLoop : MonoBehaviour {
    CanvasGroup group;
    float alphaNow = 0.0f;
    float totalTime;

    public float loopTime = 1.0f;
    bool isWorking = false;
    public bool startFloop = false;

    enum FadeType {
        fadeIn,
        fadeOut
    };

    FadeType fadeType;

    void Awake() {
        group = this.GetComponent<CanvasGroup>();
        group.alpha = 0f;
    }
    // Use this for initialization
    void Start() {
        if (startFloop)
            StartLoop(loopTime);
    }

    public void StartLoop(float _loopTime) {
        loopTime = _loopTime;
        if (!isWorking) {
            isWorking = true;
            FadeIn(loopTime);
        }
    }

    public void StopLoop() {
        isWorking = false;
        group.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update() {
        if (isWorking) {
            alphaNow += Time.deltaTime;
            if (alphaNow > totalTime) {
                if (fadeType == FadeType.fadeIn)
                    FadeOut(loopTime);
                else {
                    FadeIn(loopTime);
                }
            }


            switch (fadeType) {
                case FadeType.fadeIn:
                    group.alpha = getEnterAlpha(alphaNow / totalTime);
                    break;
                case FadeType.fadeOut:
                    group.alpha = getOutAlpha(alphaNow / totalTime);
                    break;
            }
        }
    }

    void FadeIn(float time) {
        totalTime = time;
        alphaNow = 0.0f;
        fadeType = FadeType.fadeIn;
    }

    void FadeOut(float time) {
        totalTime = time;
        alphaNow = 0.0f;
        fadeType = FadeType.fadeOut;
    }

    float getEnterAlpha(float percentage) {
        float x = (percentage * 2f - 1f) * 5.0f;
        float alpha = 1f / (1f + Mathf.Pow(Mathf.Exp(1f), -x));
        return alpha;
    }

    float getOutAlpha(float percentage) {
        return 1f - getEnterAlpha(percentage);
    }
}
