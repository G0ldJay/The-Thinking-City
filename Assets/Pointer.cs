
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour {

    public float _DefaultLength = 5.0f;
    public GameObject _dot;
    public VRInputModule _InputModule;

    private LineRenderer _LineRenderer = null;

    private void Awake() {
        _LineRenderer = GetComponent<LineRenderer>();
    }

    private void Update() {
        UpdateLine();
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

        // set position of dot
        _dot.transform.position = endPos;

        // set linerenderer
        _LineRenderer.SetPosition(0, transform.position);
        _LineRenderer.SetPosition(1, endPos);
    }

    private RaycastHit CreateRaycast(float length) {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, _DefaultLength);

        return hit;
    }
}
