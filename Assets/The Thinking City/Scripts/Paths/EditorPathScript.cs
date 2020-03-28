using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorPathScript : MonoBehaviour {
    public Color _rayColor = Color.white;
    public List<Transform> _pathObjs = new List<Transform>();
    Transform[] _arr;

    private void OnDrawGizmos() {
        Gizmos.color = _rayColor;
        _arr = GetComponentsInChildren<Transform>();
        _pathObjs.Clear();

        foreach(Transform t in _arr) {
            if(t != transform) {
                _pathObjs.Add(t);
            }
        }

        for (int i = 0; i < _pathObjs.Count; ++i) {
            Vector3 pos = _pathObjs[i].position;
            if(i > 0) {
                Vector3 prev = _pathObjs[i - 1].position;
                Gizmos.DrawLine(prev, pos);
                Gizmos.DrawWireSphere(pos, 0.15f);
            }
        }
    }
}
