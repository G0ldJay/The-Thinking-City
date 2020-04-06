using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TrishBusHandlerCollider : MonoBehaviour
{

    FMOD.Studio.Bus TrishBus;
    public FMODUnity.StudioEventEmitter StudioEventEmitter;

    // Start is called before the first frame update
    void Start()
    {
        TrishBus = FMODUnity.RuntimeManager.GetBus("Bus:/MasterGroup/Trish");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (StudioEventEmitter.TriggerOnce == false)
            {
                TrishBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }

            StudioEventEmitter.Play();
            StudioEventEmitter.TriggerOnce = true;
        }
    }

}
