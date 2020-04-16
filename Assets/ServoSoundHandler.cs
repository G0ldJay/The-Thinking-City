using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ServoSoundHandler : MonoBehaviour
{
    public bool isMoving;
    public bool isRotating;
    public FMODUnity.StudioEventEmitter servoEventEmitter;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving || isRotating)
        {
            if (!servoEventEmitter.IsPlaying())
            {
                servoEventEmitter.Play();
            }
        }

        else
        {
            servoEventEmitter.Stop();
        }
    }
}
