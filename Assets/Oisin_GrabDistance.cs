
using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Oisin_GrabDistance : MonoBehaviour {

    [HideInInspector]
    public GameObject _CurrentObject;
    [HideInInspector]
    public GameObject _PrevObject;
    public float _DistanceGrabberLength = 3;
    public SteamVR_Input_Sources _TargetSource;
    public SteamVR_Action_Boolean _ClickAction;

    [SerializeField]
    private Hand hand;
    private int forw = 1;
    private Vector3 pos = Vector3.zero;
    private Transform handTrans;
    private GameObject castingPoint;

    private void Start() {
        string castingpt = "GrabPointerRight";
        if (hand.handType == SteamVR_Input_Sources.LeftHand) {
            forw = -1;
            castingpt = "GrabPointerLeft";
        }
        handTrans = transform;
        StartCoroutine(SetPointerPos(2, castingpt));
    }

    void Update() {
        //send raycast from hand finger
        if (castingPoint == null) return;
        pos = castingPoint.transform.position;

        RaycastHit hit;
        Ray ray = new Ray(pos, handTrans.forward);
        Physics.Raycast(ray, out hit, _DistanceGrabberLength);
        Debug.DrawRay(pos, handTrans.forward, Color.green);

        //Debug.Log(hand.gameObject.name + " : " + hit.collider.gameObject);
        _CurrentObject = hit.collider.gameObject;

        if(_CurrentObject != _PrevObject && _PrevObject != null) {
            // set prev object material to normal material
            _PrevObject.GetComponent<Highlighter>().UnhighlightObject();
        }
        else {
            //detect if object found is grabbable - has throwable component
            if (_CurrentObject.GetComponent<Throwable>() != null) {
                // swap object material to highlighted variant
                _CurrentObject.GetComponent<Highlighter>().HighlightObject();

                // if player grabs, check distance from player - if over a certain distance, allow distance grab
                if (_ClickAction.GetStateDown(_TargetSource)) {
                    // attach object
                    hand.AttachObject(_CurrentObject, GrabTypes.Grip);
                }
            }
        }
        _PrevObject = _CurrentObject;
    }

    IEnumerator SetPointerPos(float time, string objName) {
        yield return new WaitForSeconds(time);
        castingPoint = GameObject.Find(objName);
        if(castingPoint == null) {
            //redo search
            StartCoroutine(SetPointerPos(1, objName));
        }
    }
}
