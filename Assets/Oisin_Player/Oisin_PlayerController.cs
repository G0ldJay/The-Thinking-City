using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Oisin_PlayerController : MonoBehaviour {

    public SteamVR_Action_Vector2 walkInput;
    public SteamVR_Action_Vector2 rotInput;
    //public SteamVR_Action_Boolean running;
    public float walkSpeed   = 1.8f;
    public float rotateSpeed = 2.0f;
    //public float runningSpeed = 2;

    private CharacterController characterController;

    private void Start() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        if (walkInput.axis.magnitude > 0.1f) {
            ///float speed = running.state ? runningSpeed : walkSpeed;
            MovePlayer(walkSpeed);
        }
        if(rotInput.axis.magnitude > 0.1f) {
            RotatePlayer(rotateSpeed);
        }
    }

    void MovePlayer(float speed) {
        Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(walkInput.axis.x, 0, walkInput.axis.y));
        characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0, 9.81f, 0) * Time.deltaTime);
    }

    void RotatePlayer(float speed) {
        transform.Rotate(0, rotInput.axis.x * rotateSpeed * Time.deltaTime, 0);
    }
}