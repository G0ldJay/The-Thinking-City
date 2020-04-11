
using UnityEngine;
using Valve.VR.InteractionSystem;

public class KeycardAttachRoboticArm : MonoBehaviour {
    public Transform targetTrans;
    public RoboticArm rb;

    private void Update() {
        if (!rb.crashed) { 
            transform.position = targetTrans.position;
            transform.rotation = targetTrans.rotation;
        }
        else {
            GetComponent<Throwable>().distanceGrabbable = true;
            enabled = false;
        }
    }
}
