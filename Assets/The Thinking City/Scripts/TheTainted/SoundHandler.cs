using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;



public class SoundHandler : MonoBehaviour {

    public GameObject MeatFootTransform;
    public GameObject MetalFootTransform;
    public GameObject MeatArm;
    public GameObject Head;

    //Play Meaty foot sound from foot audio source
    public void MeatFoot() {
        //play sound at left foot transform

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/RobotMeatStep", MeatFootTransform);
    }

    //Play Steel foot sound from foot audio source
    public void MetalFoot() {
        //play sound at right foot transform

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/RobotMetalStep", MetalFootTransform);;
    }

    public void MeatFootTurn()
    {
        //play sound at left foot transform

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/RobotMeatStepTurn", MeatFootTransform);
    }

    //Play Steel foot sound from foot audio source
    public void MetalFootTurn()
    {
        //play sound at right foot transform

        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/RobotMetalStepTurn", MetalFootTransform); ;
    }

    public void MeatHit()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/WetHit", MeatArm);
    }

    public void Screech()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/ScreechVoiceLine", Head);
    }


}
