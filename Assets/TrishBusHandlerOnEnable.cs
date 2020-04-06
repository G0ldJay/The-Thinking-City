using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrishBusHandlerOnEnable : MonoBehaviour
{
    FMOD.Studio.Bus TrishBus;
    public FMODUnity.StudioEventEmitter StudioEventEmitter;

    // Start is called before the first frame update
    void Start()
    {
        TrishBus = FMODUnity.RuntimeManager.GetBus("Bus:/MasterGroup/Trish");
    }

    private void OnEnable()
    {
        if (StudioEventEmitter.TriggerOnce == false)
        {
            TrishBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        StudioEventEmitter.Play();
        StudioEventEmitter.TriggerOnce = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
