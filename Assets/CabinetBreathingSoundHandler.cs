using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CabinetBreathingSoundHandler : MonoBehaviour
{

    // Start is called before the first frame update
    bool cabinetDoorsClosed;
    public StudioEventEmitter studioEventEmitter;

    void Start()
    {
        cabinetDoorsClosed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (cabinetDoorsClosed == false)
            {
                studioEventEmitter.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            studioEventEmitter.Stop();
            cabinetDoorsClosed = true;
        }
    }

    void Update()
    {
        if (cabinetDoorsClosed == true)
        {
            studioEventEmitter.Stop();
        }
    }
}
