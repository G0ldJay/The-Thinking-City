using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboticArm : MonoBehaviour {
    private Collider    mainCollider;
    private Collider[]  allColliders;
    private Rigidbody[] rigRigidbodies;

    private void Awake() {
        mainCollider = GetComponent<Collider>();
        allColliders = gameObject.GetComponentsInChildren<Collider>(true);
        rigRigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
        TurnOnRagdoll(false);
    }

    public void TurnOnRagdoll(bool isRagdoll) {
        foreach (var c in allColliders) {
            c.enabled = isRagdoll;
        }
        mainCollider.enabled = !isRagdoll;
        foreach (Rigidbody rb in rigRigidbodies) {
            rb.isKinematic = !isRagdoll;
        }
        GetComponent<Animator>().enabled = !isRagdoll;
    }

    public void StopAllMovement() {
        JoystickController[] jsList = FindObjectsOfType<JoystickController>();
        foreach (var js in jsList) {
            js.dropped = true;
        }
    }

    public void CrashRoboticArm() {
        StartCoroutine(Crash());
    }

    IEnumerator Crash() {
        //call FMOD sound
        gameObject.GetComponent<SoundHandlerRoboticArm>().PlayArmCrash();
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        //turn on ragdoll
        TurnOnRagdoll(true);
        //stop all movement
        StopAllMovement();
    }

}
