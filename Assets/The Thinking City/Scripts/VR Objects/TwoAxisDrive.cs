using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TwoAxisDrive : MonoBehaviour {

    // CODE TAKEN FROM https://forum.unity.com/threads/steamvr-2-axis-circular-drive.782717/

    private bool grabbed;
    private Hand hand;
    private GrabTypes grabbedWithType;

    private Quaternion _delta;

    private void Start() {
        grabbed = false;
    }

    private void HandHoverUpdate(Hand hand) {
        if (hand == this.hand || !grabbed) {
            GrabTypes startingGrabType = hand.GetGrabStarting();
            bool isGrabEnding = hand.IsGrabbingWithType(grabbedWithType) == false;

            if (grabbedWithType == GrabTypes.None && startingGrabType != GrabTypes.None) {
                grabbedWithType = startingGrabType;

                grabbed = true;
                this.hand = hand;

                var lookAt = Quaternion.LookRotation(hand.hoverSphereTransform.position - transform.position);

                _delta = Quaternion.Inverse(lookAt) * transform.rotation;
            }

            else if (grabbedWithType != GrabTypes.None && isGrabEnding) {
                grabbed = false;
                grabbedWithType = GrabTypes.None;
                this.hand = null;

            }
        }

    }

    private void Update() {
        if (grabbed) {
            transform.rotation = Quaternion.LookRotation(hand.hoverSphereTransform.position - transform.position) * _delta;
            //Quaternion temp = Quaternion.LookRotation(hand.hoverSphereTransform.position - transform.position) * _delta;
            //transform.rotation = new Quaternion(temp.x, 0, temp.z, temp.w);
        }
    }

    //////////////////
}
