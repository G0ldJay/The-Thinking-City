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
    private SoundHandlerRoboticArm SHRA;
    //public Vector3 targetOriginalPosition;
    public bool dropped;

    private void Start() {
        maxRot = cd.maxAngle;
        minRot = cd.minAngle;
        minRange = -1;
        maxRange =  1;
        SHRA = soundSourceObject.GetComponent<SoundHandlerRoboticArm>();
        //targetOriginalPosition = targetObject.transform.position;
    }

    void Update() {
        float mvmtMagnitude = GetNormalisedRotMagnitude();
        //Debug.Log(mvmtMagnitude);
        if(mvmtMagnitude != 0 && !this.dropped) {
            if (transformAdjustment == TransformAdj.translate) {
                //targetObject.transform.Translate(objectMoveSpeed * rot * Time.deltaTime, 0, 0);
                //if(!CheckValidMovement()) {
                //    targetObject.transform.Translate(-objectMoveSpeed * rot * Time.deltaTime, 0, 0);
                //}

                // move forward or backwards along path
                targetObject.GetComponent<FollowPath>().MoveAlongPath(mvmtMagnitude);

            }
            else if (transformAdjustment == TransformAdj.rotate) {
                targetObject.transform.localEulerAngles += new Vector3(0, 0, objectMoveSpeed * mvmtMagnitude * Time.deltaTime);
            }
            // play sound effect if moving
            SHRA.PlayServos();
        }
        else {
            SHRA.StopServos();
        }
    }

    ////check pos in bounds, if over a certain limit, return false to cancel movement.
    //private bool CheckValidMovement() {
    //    Vector3 pos = targetObject.transform.position;
    //    //check x axis position
    //    if (pos.x > targetOriginalPosition.x + 5 || pos.x < targetOriginalPosition.x - 5) {
    //        return false;
    //    }
    //    else if(pos.z > targetOriginalPosition.z + 5 || pos.z < targetOriginalPosition.z - 5) {
    //        return false;
    //    }
    //    return true;
    //}

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
