using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPodDoorTrigger : MonoBehaviour
{
    private RobotPod _robotPodScript = null;

    public RobotPod robotPodScript { set { _robotPodScript = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (_robotPodScript != null)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            _robotPodScript.OpenRobotPodDoor();
            
        }
    }
}
