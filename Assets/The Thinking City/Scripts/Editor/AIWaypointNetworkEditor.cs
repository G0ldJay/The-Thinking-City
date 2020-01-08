using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;


[CustomEditor(typeof(AIWaypointNetwork))] //Creates a custom editor type of the AIWaypointNetwork class
// ----------------------------------------------------------------------
// Class	:	AIWaypointNetworkEditor
// Desc		:	Controls how the waypoints are displayed in the editor
// ----------------------------------------------------------------------
public class AIWaypointNetworkEditor : Editor
{
    // ---------------------------------------------------------
    // Name	:	OnInspectorGUI
    // Desc	:	Called by unitys editor
    // ---------------------------------------------------------
    public override void OnInspectorGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork)target; //Object being inspected in window. Type of AIWaypointNetwork

        network.DisplayMode = (PathDisplayMode)EditorGUILayout.EnumPopup("Display Mode",network.DisplayMode); //Creates a button in inspector window controlling how wapoints are displayed 

        if (network.DisplayMode == PathDisplayMode.Path) //Path display mode allows you to specify waypoint paths to display
        {
            network.UIStart = EditorGUILayout.IntSlider("Waypoint Start", network.UIStart, 0, network.Waypoints.Count - 1);//Start waypoint of your choice
            network.UIEnd = EditorGUILayout.IntSlider("Waypoint End", network.UIEnd, 0, network.Waypoints.Count - 1); //End waypoint of your choice
        }

        DrawDefaultInspector(); //Calls Unitys default inspector 
    }

    // ---------------------------------------------------------
    // Name	:	OnSceneGUI
    // Desc	:	Called by unitys editor. Updates scene window
    // ---------------------------------------------------------
    private void OnSceneGUI()
    {
        AIWaypointNetwork network = (AIWaypointNetwork)target; //Object being inspected in window. Type of AIWaypointNetwork

        LabelWaypoints(network); //Draws a label below each waypoint in the scene view making it easier to see list order

        if (network.DisplayMode == PathDisplayMode.Connections) //If connections is the selected option
            DisplayWaypointConnections(network); //Calls function to display waypoints as a full connected path 
        else if (network.DisplayMode == PathDisplayMode.Path) //If path is the selected option
            DisplayWaypointPath(network); //Calls function to display wapoints as a path
    }

    // ---------------------------------------------------------
    // Name	:	LabelWaypoints
    // Desc	:	Assigns individual name to each wapoint in network passed in
    // ---------------------------------------------------------
    private void LabelWaypoints(AIWaypointNetwork network)
    {
        for (int i = 0; i < network.Waypoints.Count; i++) //Iterates through waypoints list 
        {
            if (network.Waypoints[i] != null) //Checks for empty spot in list
                Handles.Label(network.Waypoints[i].position, "Waypoint " + (i+1).ToString()); //Gives label in position a name
        }
    }

    // ---------------------------------------------------------
    // Name	:	DisplayWaypointConnections
    // Desc	:	Draws a relationship link betweeen each waypoint in list 
    // ---------------------------------------------------------
    private void DisplayWaypointConnections(AIWaypointNetwork network)
    {
        Vector3[] linePoints = new Vector3[network.Waypoints.Count + 1]; //Vector array of points 

        for (int i = 0; i <= network.Waypoints.Count; i++) //Iterates through waypoints list 
        {
            int index = i != network.Waypoints.Count ? i : 0; //Wraps index value

            if (network.Waypoints[index] != null) //Checks for empty spot in list
                linePoints[i] = network.Waypoints[index].position; //Adds Vector3 position of waypoint in list to line points array
            else
                linePoints[i] = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity); //Error handle just adds ridiculous value to stand out
        }

        Handles.color = Color.cyan; //Sets color of line 
        Handles.DrawPolyLine(linePoints); //Draws the line between points
    }

    // ---------------------------------------------------------
    // Name	:	DisplayWaypointPath
    // Desc	:	Draws a relationship link betweeen specified values by user
    // ---------------------------------------------------------
    private void DisplayWaypointPath(AIWaypointNetwork network)
    {
        NavMeshPath path = new NavMeshPath(); //Creates new instance of NavMeshPath. Will allow us to draw path between points in scene view.
        Vector3 from = network.Waypoints[network.UIStart].position, to = network.Waypoints[network.UIEnd].position; //Holds the position of the two waypoints you're generating a path between 

        NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path); //Calculates the path between two points along the NavMesh 

        Handles.color = Color.yellow; //Sets the lines color to yellow coz it pops
        Handles.DrawPolyLine(path.corners); //Draws the path in the scene view
    }
}
