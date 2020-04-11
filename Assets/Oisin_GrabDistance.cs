
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
    string castingpt = "";

    [SerializeField]
    private Hand hand;
    private int forw = 1;
    private Vector3 pos = Vector3.zero;
    private GameObject castingPoint;

    private void Start() {
        if (_TargetSource == SteamVR_Input_Sources.LeftHand) {
            castingpt = "GrabPointerLeft";
        }
        else if(_TargetSource == SteamVR_Input_Sources.RightHand) {
            castingpt = "GrabPointerRight";
        }
        StartCoroutine(SetPointerPos(2, castingpt));
    }

    void Update() {
        //send raycast from hand finger
        if (castingPoint == null) return;
        pos = castingPoint.transform.position;

        RaycastHit hit;
        Ray ray = new Ray(pos, castingPoint.transform.forward);
        Physics.Raycast(ray, out hit, _DistanceGrabberLength);
        Debug.DrawRay(pos, castingPoint.transform.forward, Color.green);

        if (hit.collider.gameObject != null) {
            Debug.Log(hand.gameObject.name + " : " + hit.collider.gameObject);
            _CurrentObject = hit.collider.gameObject;

            //if(_PrevObject.GetComponent<Highlighter>() != null) {
            //    // set prev object material to normal material
            //    _PrevObject.GetComponent<Highlighter>().UnhighlightObject();
            //}

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
