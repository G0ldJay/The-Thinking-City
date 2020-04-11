using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour {

    private Material NormalMaterial;
    [SerializeField]
    private Material HighlightedMaterial;

    private void Start() {
        NormalMaterial = GetComponent<MeshRenderer>().material;
    }

    public void HighlightObject(bool on) {
        if (on) {
            gameObject.GetComponent<MeshRenderer>().material = HighlightedMaterial;
        }
        else {
            gameObject.GetComponent<MeshRenderer>().material = NormalMaterial;
        }
    }
}
