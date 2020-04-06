using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Oisin_DistanceGrabber : MonoBehaviour {

    public float _DistanceGrabberLength = 3;
    [SerializeField]
    private Hand hand;

    void Update() {
        //send raycast from hand finger
        Vector3 pos = hand.mainRenderModel.GetBonePosition((int)hand.fingerJointHover);

        RaycastHit hit;
        Ray ray = new Ray(pos, Quaternion.Euler(44, 0, 0) * Vector3.forward);
        Physics.Raycast(ray, out hit, _DistanceGrabberLength);

        Debug.Log(hit.collider.gameObject);

        //detect if object found is grabbable - has throwable component


        // if player grabs, check distance from player - if over a certain distance, allow distance grab


        // attach object

    }
}
