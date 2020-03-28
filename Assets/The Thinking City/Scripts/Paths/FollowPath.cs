using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

    public EditorPathScript _Path;
    public int    _WaypointID = 0;
    public float  _Speed;
    public float  _RotSpeed   = 5.0f;
    public string _PathName;

    private float _ReachDistance = 1.0f;

    Vector3 _LastPos;
    Vector3 _CurrPos;

    // Start is called before the first frame update
    void Start() {
        // _Path = GameObject.Find(_PathName).GetComponent<EditorPathScript>();
        _LastPos = transform.position;
    }

    // Update is called once per frame
    public void MoveAlongPath(float joystickMagnitude) {
        int direction = joystickMagnitude > 0 ? 1 : -1;
        float dist         = Vector3.Distance(_Path._pathObjs[_WaypointID].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, _Path._pathObjs[_WaypointID].position, _Speed * Time.deltaTime);

        // rotate toards position of next waypoint
        var newRotation    = Quaternion.LookRotation(_Path._pathObjs[_WaypointID].position - transform.position);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, _RotSpeed * Time.deltaTime);

        if(dist <= _ReachDistance) {
            // change target waypoint
            _WaypointID += direction;
            // turn on / off light on rails
        }

        
        if(_WaypointID >= _Path._pathObjs.Count) {
            // activate light at end and allow dropping
        }
    }
}
