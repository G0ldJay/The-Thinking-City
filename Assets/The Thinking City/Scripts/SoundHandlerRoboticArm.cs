using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandlerRoboticArm : MonoBehaviour {
    FMOD.Studio.EventInstance soundEvent;

    public void PlayServos() {
        soundEvent = FMODUnity.RuntimeManager.CreateInstance("event:/RobotArmServo");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
        soundEvent.start();
        // FMODUnity.RuntimeManager.PlayOneShot("event:/RobotArmServo", GetComponent<Transform>().position);

    }

    public void StopServos() {
        soundEvent.release();
        FMOD.Studio.STOP_MODE stop = FMOD.Studio.STOP_MODE.IMMEDIATE;
        soundEvent.stop(stop);
    }

}