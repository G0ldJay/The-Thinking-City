using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Public Enums of the AIWaypointNetwork
public enum PathDisplayMode {None, Connections, Path}; //Display mode type in editor window

// ----------------------------------------------------------------------
// Class	:	AIWaypointNetwork
// Desc		:	Holds info about the waypoint network for other scripts to utilize
// ----------------------------------------------------------------------
public class AIWaypointNetwork : MonoBehaviour 
{
    [HideInInspector] //Hides this variable from the inspector window 
    public PathDisplayMode DisplayMode = PathDisplayMode.Connections; //Holds display mode. Set to connections by default
    [HideInInspector]
    public int UIStart = 0; //Holds start value of list size
    [HideInInspector]
    public int UIEnd = 0; //Holds end of list size
    public List<Transform> Waypoints = new List<Transform>(); //List of waypoints. Viewable in editor as we need to populate it.
}
