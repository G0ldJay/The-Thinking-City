
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour {

    public float _DefaultLength = 5.0f;
    public GameObject _dot;
    public VRInputModule _InputModule;
    public LineRenderer _LineRenderer = null;
    public Valve.VR.InteractionSystem.Hand hand;
    public bool renderLine = false;

    private void Start() {
        //mainRenderModel.GetBonePosition((int)fingerJointHover)
        //transform.position = hand.mainRenderModel.GetBonePosition((int)hand.fingerJointHover);
        transform.Rotate(44, 0, 0);
    }

    private void Update() {
        SetTransformToFinger();
        UpdateLine();
    }

    public void SetTransformToFinger() {
        transform.position = hand.mainRenderModel.GetBonePosition((int)hand.fingerJointHover);
        //transform.forward = hand.mainRenderModel.transform.forward; 
    }

    private void UpdateLine() {
        // use default val for length or distance
        PointerEventData data = _InputModule.GetData();
        float targetLength = data.pointerCurrentRaycast.distance == 0 ? _DefaultLength : data.pointerCurrentRaycast.distance;

        // raycast
        RaycastHit hit = CreateRaycast(targetLength);

        // default
        Vector3 endPos = transform.position + (transform.forward * targetLength);

        // or based on hit
        if(hit.collider != null) {
            endPos = hit.point;
        }

        if (renderLine) {
            _dot.SetActive(true);
            // set position of dot
            _dot.transform.position = endPos;

            // set linerenderer
            _LineRenderer.SetPosition(0, transform.position);
            _LineRenderer.SetPosition(1, endPos);
        }
        else {
            _dot.SetActive(false);
        }
    }

    private RaycastHit CreateRaycast(float length) {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, _DefaultLength);

        return hit;
    }
}
