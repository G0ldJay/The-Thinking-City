using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;



public class SoundHandler : MonoBehaviour {

    public Transform MeatFootTransform;
    public Transform MetalFootTransform;

    //Play Meaty foot sound from foot audio source
    public void MeatFoot() {
        //play sound at left foot transform

        FMODUnity.RuntimeManager.PlayOneShot("event:/RobotMeatStep", MeatFootTransform.transform.position);
    }

    //Play Steel foot sound from foot audio source
    public void MetalFoot() {
        //play sound at right foot transform

        FMODUnity.RuntimeManager.PlayOneShot("event:/RobotMetalStep", MetalFootTransform.transform.position);;
    }

    public void MeatFootTurn()
    {
        //play sound at left foot transform

        FMODUnity.RuntimeManager.PlayOneShot("event:/RobotMeatStepTurn", MeatFootTransform.transform.position);
    }

    //Play Steel foot sound from foot audio source
    public void MetalFootTurn()
    {
        //play sound at right foot transform

        FMODUnity.RuntimeManager.PlayOneShot("event:/RobotMetalStepTurn", MetalFootTransform.transform.position); ;
    }

}
