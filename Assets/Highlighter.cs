using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour {

    private Material NormalMaterial;
    [SerializeField]
    private Material HighlightedMaterial;

    private void Start() {
        NormalMaterial = gameObject.GetComponent<MeshRenderer>().material;
    }

    public void HighlightObject() {
        gameObject.GetComponent<MeshRenderer>().material = HighlightedMaterial;
    }

    public void UnhighlightObject() {
        gameObject.GetComponent<MeshRenderer>().material = NormalMaterial;
    }
}
