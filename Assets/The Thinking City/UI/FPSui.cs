using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSui : MonoBehaviour {
    public TextMeshProUGUI tmp;
    private float deltaTime = 0.0f;
    private float fps = 0.0f;
     
    void Update() {
        deltaTime += Time.deltaTime;
        deltaTime /= 2.0f;
        fps = 1.0f/deltaTime;
        float rounded = Mathf.Round(fps);
        tmp.text = "FPS: " + rounded;
    }

}
