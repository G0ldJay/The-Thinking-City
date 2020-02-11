using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class JoystickController : MonoBehaviour {
    public GameObject targetObject;
    public GameObject soundSourceObject;
    public CircularDrive cd;
    public float objectMoveSpeed = 1;
    public enum RotAxis {
        XAxis,
        YAxis,
        ZAxis
    };
    public RotAxis axisOfRotation = RotAxis.ZAxis;
    public enum TransformAdj {
        translate,
        rotate
    };
    public TransformAdj transformAdjustment = TransformAdj.translate;
    private float maxRot, minRot, maxRange, minRange;
    private bool isPlayingSound = false;
    private SoundHandlerRoboticArm SHRA;
    public float X_Max, X_Min;

    private void Start() {
        maxRot = cd.maxAngle;
        minRot = cd.minAngle;
        minRange = -1;
        maxRange =  1;
        SHRA = soundSourceObject.GetComponent<SoundHandlerRoboticArm>();
    }

    void Update() {
        float rot = GetNormalisedRotMagnitude();
        Debug.Log(gameObject.name + ", " + rot);

        if(rot != 0) {
            if (transformAdjustment == TransformAdj.translate) {
                targetObject.transform.localPosition += new Vector3(objectMoveSpeed * rot, 0, 0);
                //if(!CheckValidMovement()) {
                //    targetObject.transform.localPosition -= new Vector3(objectMoveSpeed * rot, 0, 0);
                //}
            }
            else if (transformAdjustment == TransformAdj.rotate) {
                targetObject.transform.localEulerAngles += new Vector3(0, 0, objectMoveSpeed * rot);
            }

            //SHRA.PlayServos();

            // play sound effect if moving
            if (!isPlayingSound) {
                SHRA.PlayServos();
                isPlayingSound = true;
            }
        }
        else
        {
            isPlayingSound = false;
            SHRA.StopServos();
            Debug.Log("I am still");
        }
    }

    //check pos in bounds, if over a certain limit, return false to cancel movement.
    private bool CheckValidMovement() {
        Vector3 pos = targetObject.transform.position;
        //check x axis position

        //check y axis position

        return false;
    }

    private float GetNormalisedRotMagnitude() {
        float rot = 0;
        if (axisOfRotation == RotAxis.XAxis) {
            rot = gameObject.transform.localRotation.eulerAngles.x;
        }
        else if (axisOfRotation == RotAxis.YAxis) {
            rot = gameObject.transform.localRotation.eulerAngles.y;
        }
        else if (axisOfRotation == RotAxis.ZAxis) {
            rot = gameObject.transform.localRotation.eulerAngles.z;
        }

        if (rot == 0) return 0;

        float zRot = rot;
        if (zRot > 45.0f) {
            zRot -= 360;
        };
        float normalizedRot = minRange + (zRot - minRot) * (maxRange - minRange) / (maxRot - minRot);
        if (normalizedRot == -7) normalizedRot = 1;
        return normalizedRot;
    }
}
