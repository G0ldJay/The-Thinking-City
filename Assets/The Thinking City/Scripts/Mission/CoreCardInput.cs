using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreCardInput : MonoBehaviour
{
    private CorePowerUpQuest _corePowerUpScript = null;

    public CorePowerUpQuest corePowerUpScript { set { _corePowerUpScript = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (_corePowerUpScript != null)
        {
            _corePowerUpScript.OnTriggerEvent(gameObject, other);
        }
    }
}
