
using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Oisin_GrabDistance : MonoBehaviour {

    [HideInInspector]
    public GameObject _CurrentObject;
    [HideInInspector]
    public GameObject _PrevObject;
    public float _DistanceGrabberLength = 2;
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

        if (hit.collider != null) {
            
            _CurrentObject = hit.collider.transform.root.gameObject;
            //Debug.Log(hand.gameObject.name + " : " + _CurrentObject.name);

            if (_PrevObject != null) {
                if(_PrevObject.name != _CurrentObject.name && _PrevObject.GetComponent<Throwable>() != null) {
                    // unhighlight object
                    HighlightObject(_PrevObject, false);
                }
            }

            //detect if object found is grabbable - has throwable component
            if (_CurrentObject.GetComponent<Throwable>() != null && _CurrentObject.GetComponent<Throwable>().distanceGrabbable) {
                // swap object material to highlighted variant
                HighlightObject(_CurrentObject, true);
                //_CurrentObject.GetComponent<Highlighter>().HighlightObject();

                // if player grabs, check distance from player - if over a certain distance, allow distance grab
                if (_ClickAction.GetStateDown(_TargetSource)) {
                    // attach object
                    hand.AttachObject(_CurrentObject, GrabTypes.Grip);
                }
            }
        }
        else {
            _CurrentObject = null;
            HighlightObject(_PrevObject, false);
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

    void HighlightObject(GameObject obj, bool toggle) {
        // loop through obj & children and activate / deactivate highlight
        if (obj.GetComponent<Highlighter>() != null) {
            obj.GetComponent<Highlighter>().HighlightObject(toggle);
        }

        // loop through all children and toggle
        GameObject tempObj;
        foreach (Transform t in obj.transform) {
            tempObj = t.gameObject;
            if (tempObj.GetComponent<Highlighter>() != null) {
                tempObj.GetComponent<Highlighter>().HighlightObject(toggle);
            }

            // if child obj itself has children, toggle those
            if (t.childCount > 0) {
                GameObject child_tempObj;
                foreach (Transform child_t in tempObj.transform) {
                    child_tempObj = child_t.gameObject;
                    if (child_tempObj.GetComponent<Highlighter>() != null) {
                        child_tempObj.GetComponent<Highlighter>().HighlightObject(toggle);
                    }
                }
            }
        }
    }
}
