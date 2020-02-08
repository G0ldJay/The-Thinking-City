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

        //Debug.Log("PLAY MEATY FOOT SOUND");
        FMODUnity.RuntimeManager.PlayOneShot("event:/RobotMeatStep", MeatFootTransform.transform.position);
    }

    //Play Steel foot sound from foot audio source
    public void MetalFoot() {

        FMODUnity.RuntimeManager.PlayOneShot("event:/RobotMetalStep", MetalFootTransform.transform.position);

        //play sound at right foot transform
        //Debug.Log("PLAY METAL FOOT SOUND");
    }

}
