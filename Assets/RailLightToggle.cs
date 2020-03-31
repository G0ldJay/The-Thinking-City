using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailLightToggle : MonoBehaviour {
    [SerializeField]
    private bool UseMaterials = false;
    [SerializeField]
    private bool StartActive  = false;
    [SerializeField]
    private Material offMaterial;
    [SerializeField]
    private Material onMaterial;

    private void Start() {
        ToggleEmissive(StartActive);
    }

    // if true, turn emissive on, else turn off
    public void ToggleEmissive(bool On) {
        if(UseMaterials) {
            if(On) {
                GetComponent<MeshRenderer>().material = onMaterial;
            }
            else {
                GetComponent<MeshRenderer>().material = offMaterial;
            }
        }
        else {
            if (On) {
                GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            }
            else {
                GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
                
            }
        }

    }
}
