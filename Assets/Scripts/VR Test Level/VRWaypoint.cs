using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRWaypoint : MonoBehaviour {
    public VRWaypointSystem VRWPS;

    private void Awake() {
        this.VRWPS = FindObjectOfType<VRWaypointSystem>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag != "Player") return;
        Debug.Log("Destroy waypoint");
        // laod next waypoint then destroy parent
        VRWPS.LoadNextWaypoint();
        Destroy(transform.parent.gameObject);
        
    }
}
