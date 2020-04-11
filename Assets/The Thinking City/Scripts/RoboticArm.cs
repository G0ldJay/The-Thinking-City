using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboticArm : MonoBehaviour {
    [HideInInspector]
    public bool crashed = false;
    public GameObject playerInterestedObj;
    public GameObject followObj;

    [SerializeField]
    private GameObject railEndChecker;
    private Collider    mainCollider;
    private Collider[]  allColliders;
    private Rigidbody[] rigRigidbodies;
    private Vector3 originalPos;

    private void Awake() {
        mainCollider = GetComponent<Collider>();
        allColliders = gameObject.GetComponentsInChildren<Collider>(true);
        rigRigidbodies = gameObject.GetComponentsInChildren<Rigidbody>();
        TurnOnRagdoll(false);
        originalPos = transform.position;
    }

    private void Update() {
        transform.position = new Vector3(followObj.transform.position.x, originalPos.y, followObj.transform.position.z);
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
        if (!crashed && railEndChecker.GetComponent<RailLightToggle>().isCurrentlyOn) {
            StartCoroutine(Crash());
            crashed = true;
        }
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
        //drop obj of interest
        playerInterestedObj.GetComponent<Rigidbody>().useGravity = true;
    }

}
