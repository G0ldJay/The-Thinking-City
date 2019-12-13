using UnityEngine;

public class VRWaypointSystem : MonoBehaviour {

    public GameObject[] waypointMarkers = new GameObject[11];
    public GameObject waypointPrefab;
    private int waypointIndex = 1;

    public void LoadNextWaypoint() {
        Vector3 position = this.waypointMarkers[this.waypointIndex].transform.position + new Vector3(0, 1.91f, 0);
        GameObject newWaypoint = Instantiate(this.waypointPrefab, position, Quaternion.identity);
        this.waypointIndex++;

        if (waypointIndex == 11) {
            this.waypointIndex = 0;
            //LoadNextWaypoint();
        }
    }
}
