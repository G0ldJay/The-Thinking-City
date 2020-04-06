
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Oisin_GrabDistance : MonoBehaviour {

    [HideInInspector]
    public GameObject _CurrentObject;
    public float _DistanceGrabberLength = 3;
    public SteamVR_Input_Sources _TargetSource;
    public SteamVR_Action_Boolean _ClickAction;

    [SerializeField]
    private Hand hand;
    private int forw = 1;
    private Vector3 pos = Vector3.zero;
    private Transform handTrans;

    private void Start() {
        if(hand.handType == SteamVR_Input_Sources.LeftHand) {
            forw = -1;
        }
        handTrans = transform;
    }

    void Update() {
        //send raycast from hand finger
        if (hand != null) {
            pos = hand.mainRenderModel.GetBonePosition((int)hand.fingerJointHover);
            handTrans = hand.mainRenderModel.GetBone((int)hand.fingerJointHover).transform;
        }

        RaycastHit hit;
        Ray ray = new Ray(pos, handTrans.right * forw);
        Physics.Raycast(ray, out hit, _DistanceGrabberLength);
        Debug.DrawRay(pos, handTrans.right * forw, Color.green);

        Debug.Log(hand.gameObject.name + " : " + hit.collider.gameObject);
        _CurrentObject = hit.collider.gameObject;

        //detect if object found is grabbable - has throwable component
        if (_CurrentObject.GetComponent<Throwable>() != null) {
            // if player grabs, check distance from player - if over a certain distance, allow distance grab
            if (_ClickAction.GetStateDown(_TargetSource)) {
                // attach object
                hand.AttachObject(_CurrentObject, GrabTypes.Grip);
            }
        }
        _CurrentObject = null;
    }
}
