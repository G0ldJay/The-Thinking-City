using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ref video https://youtu.be/OJFS4Z0tT9A

public class RoboticArm : MonoBehaviour {
    public Animator animator;
    public List<Collider> ragdollParts;

    private void Awake() {
        SetRagdollParts();
    }

    private void SetRagdollParts() {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>(); 

        foreach(Collider c in colliders) {
            if(c.gameObject != this.gameObject) {
                c.isTrigger = true;
                ragdollParts.Add(c);
            }
        }
    }

    public void TurnOnRagdoll() {
        animator.enabled = false;

        foreach (Collider c in ragdollParts) {
            c.isTrigger = false;
            c.attachedRigidbody.velocity = Vector3.zero;
        }
    }
}
