using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : MonoBehaviour {

    public Transform MeatFootTransform;
    public Transform MetalFootTransform;

    //Play Meaty foot sound from foot audio source
    public void MeatFoot() {
        //play sound at left foot transform
        Debug.Log("PLAY MEATY FOOT SOUND");
    }

    //Play Steel foot sound from foot audio source
    public void MetalFoot() {
        //play sound at right foot transform
        Debug.Log("PLAY METAL FOOT SOUND");
    }

}
