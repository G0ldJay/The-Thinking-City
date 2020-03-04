using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolotableObject : MonoBehaviour {
    void Update() {
        transform.Rotate(0, 20f * Time.deltaTime, 0, Space.World);
    }
}
