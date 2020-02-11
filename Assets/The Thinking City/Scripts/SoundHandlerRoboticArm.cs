using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandlerRoboticArm : MonoBehaviour {
    bool isPlayingSound = false;
    
    FMOD.Studio.EventInstance servoInstance;

    public void PlayServos() {
        if (isPlayingSound) return;

        servoInstance = FMODUnity.RuntimeManager.CreateInstance("event:/RobotArmServo");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(servoInstance, GetComponent<Transform>(), GetComponent<Rigidbody>());
        servoInstance.start();
        isPlayingSound = true;
        // FMODUnity.RuntimeManager.PlayOneShot("event:/RobotArmServo", GetComponent<Transform>().position);

    }

    public void StopServos() {
        if (!isPlayingSound) return;

        servoInstance.release();
        FMOD.Studio.STOP_MODE stop = FMOD.Studio.STOP_MODE.IMMEDIATE;
        servoInstance.stop(stop);
        isPlayingSound = false;
    }

    public void PlayArmCrash()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/ArmCrash", GetComponent<Transform>().position);
    }

}