using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Oisin_PlayerController : MonoBehaviour {

    public SteamVR_Action_Vector2 input;
    public SteamVR_Action_Boolean running;
    public float walkSpeed = 1;
    public float runningSpeed = 2;
    private CharacterController characterController;

    private void Start() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        if (input.axis.magnitude > 0.1f) {
            float speed = running.state ? runningSpeed : walkSpeed;
            MovePlayer(speed);
        }
    }

    void MovePlayer(float speed) {
        Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
        characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0, 9.81f, 0) * Time.deltaTime);
    }
}
