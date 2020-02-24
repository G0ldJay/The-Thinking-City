using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolotableObjectViewer : MonoBehaviour {
    private GameObject currentObj;
    public Material hologram;

    public void LoadHoloObject(GameObject newObj) {
        // do not load object in if the currently displayed obj is the same
        if(currentObj != null && newObj.name == currentObj.name) {
            return;
        }

        // kill current obj
        DestroyChildren();

        //create obj with hologram shader & rotation anim
        GameObject obj = Instantiate(newObj, transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        currentObj = obj;

        //apply shader
        RemoveAllComponents(obj);

        obj.AddComponent<UnityEngine.MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = hologram;

        obj.AddComponent<HolotableObject>();
    }

    void DestroyChildren() {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    void RemoveAllComponents(GameObject obj) {
        foreach (var comp in obj.GetComponents<Component>()) {
            if (!(comp is Transform) && !(comp is MeshFilter) && !(comp is MeshRenderer)) {
                Destroy(comp);
            }
        }
    }
}
