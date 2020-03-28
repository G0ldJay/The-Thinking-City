using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimController : MonoBehaviour {

    public Animator anim;
    public bool open;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor() {
        if (this.open) return;
        StartCoroutine(OpenDoor(0.5f));
    }

    IEnumerator OpenDoor(float time) {
        yield return new WaitForSeconds(time);
        anim.Play("OpenDoor");
        this.open = true;
        StartCoroutine(CloseDoor(2.5f));
    }

    IEnumerator CloseDoor(float time) {
        yield return new WaitForSeconds(time);
        anim.Play("CloseDoor");
        this.open = false;
    }
}