using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {

    public EditorPathScript _Path;
    public int    _WaypointID = 0;
    public float  _Speed;
    public float  _RotSpeed   = 5.0f;
    public string _PathName;

    private float _ReachDistance = 0.5f;
    private int _lastDirection = 0;

    Vector3 _LastPos;
    Vector3 _CurrPos;

    // Start is called before the first frame update
    void Start() {
        // _Path = GameObject.Find(_PathName).GetComponent<EditorPathScript>();
        _LastPos = transform.position;
    }

    // Update is called once per frame
    public void MoveAlongPath(float joystickMagnitude) {
        int direction = -1;
        if(joystickMagnitude > 0) {
            direction = 1;
        }

        // store last direction -> if new direction is different, force change target waypoint.
        // ensure that out of switching when at ends of path is accounted for
        if(direction != _lastDirection) { 
            // if new direction is -1, decrement waypoint, 
            // if new direction is +1, increment waypoint
            _WaypointID += direction;
            ValidateWaypointEnds();
        }

        float dist         = Vector3.Distance(_Path._pathObjs[_WaypointID].position, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, _Path._pathObjs[_WaypointID].position, _Speed * joystickMagnitude * Time.deltaTime);

        // rotate toards position of next waypoint
        var newRotation = Quaternion.LookRotation(_Path._pathObjs[_WaypointID].position - transform.position);
        if(direction == -1) {
            newRotation *= new Quaternion(0, 0, 0, 1);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, _RotSpeed * Time.deltaTime);

        if(dist <= _ReachDistance) {
            // change target waypoint
            _WaypointID += direction;
            // turn on / off light on rails
        }

        ValidateWaypointEnds();

        _lastDirection = direction;
    }

    private void ValidateWaypointEnds() {
        if (_WaypointID > _Path._pathObjs.Count) {
            _WaypointID = _Path._pathObjs.Count - 1;
            // activate light at end and allow dropping
        }
        else if (_WaypointID < 0) {
            _WaypointID = 0;
        }
    }
}


