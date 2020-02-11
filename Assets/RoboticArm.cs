using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ref video https://youtu.be/OJFS4Z0tT9A

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
        foreach (Rigidbody rb in rigRigidbodies){
            rb.isKinematic = !isRagdoll;
        }
        GetComponent<Rigidbody>().useGravity = !isRagdoll;
        GetComponent<Animator>().enabled = !isRagdoll;
    }


}
