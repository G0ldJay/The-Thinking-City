using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HologramEye : MonoBehaviour { 
    ConstraintSource constraintSource;

    private void Start() {
        LookAtConstraint cons = gameObject.AddComponent<LookAtConstraint>();
        cons.weight = 1;
        cons.rotationOffset = new Vector3(-90, 0, 0);

        constraintSource.sourceTransform = FindObjectOfType<Valve.VR.InteractionSystem.Player>().gameObject.transform;
        constraintSource.weight = 1;

        cons.AddSource(constraintSource);
        cons.constraintActive = true;
    }
}
