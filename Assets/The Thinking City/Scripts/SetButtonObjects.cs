using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SetButtonObjects : MonoBehaviour {

    //exclude last objholder because it holds the eye
    public HoloObjHolder[] objs;

    public void SetObjs(GameObject replacement) {
        foreach (HoloObjHolder hoh in objs) {
            hoh.obj = replacement;
        }
    }
}
