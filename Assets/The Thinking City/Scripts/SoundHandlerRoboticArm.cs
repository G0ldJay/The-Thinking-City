using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandlerRoboticArm : MonoBehaviour {
    bool isPlayingSound = false;
    
    FMOD.Studio.EventInstance soundEvent;

    public void PlayServos() {
        if (isPlayingSound) return;

        soundEvent = FMODUnity.RuntimeManager.CreateInstance("event:/RobotArmServo");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
        soundEvent.start();
        isPlayingSound = true;
        // FMODUnity.RuntimeManager.PlayOneShot("event:/RobotArmServo", GetComponent<Transform>().position);

    }

    public void StopServos() {
        if (!isPlayingSound) return;

        soundEvent.release();
        FMOD.Studio.STOP_MODE stop = FMOD.Studio.STOP_MODE.IMMEDIATE;
        soundEvent.stop(stop);
        isPlayingSound = false;
    }

}