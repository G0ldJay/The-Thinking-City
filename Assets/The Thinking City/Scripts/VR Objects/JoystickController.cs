using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class JoystickController : MonoBehaviour {
    public GameObject targetObject;
    public CircularDrive cd;
    public float objectMoveSpeed = 1;

    private float maxRot, minRot, maxRange, minRange;

    private void Start() {
        maxRot = cd.maxAngle;
        minRot = cd.minAngle;
        minRange = -1;
        maxRange =  1;
    }

    void Update() {
        if (gameObject.transform.localRotation.eulerAngles.z == 0) return;

        float zRot = gameObject.transform.localRotation.eulerAngles.z;
        if (zRot > 45.0f) {
            zRot -= 360;
        };
        float normalizedRot = minRange + (zRot - minRot) * (maxRange - minRange) / (maxRot - minRot);

        //Debug.Log(normalizedRot);
    }
}
