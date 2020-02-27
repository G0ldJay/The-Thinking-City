using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterTrigger : MonoBehaviour
{
    private CorePowerUpQuest _corePowerUpScript = null;

    public CorePowerUpQuest corePowerUpScript { set { _corePowerUpScript = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (_corePowerUpScript != null)
        {
            if (_corePowerUpScript.coreOnline)
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                _corePowerUpScript.ShutterRelease();
            }        
        }
    }
}
