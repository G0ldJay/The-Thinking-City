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
        GameObject parentObj = Instantiate(newObj, transform.position, Quaternion.identity);
        currentObj = parentObj;
        //since we're using prefabs we need to get the child object
        GameObject obj = parentObj.transform.GetChild(0).gameObject;
        obj.transform.parent = transform;


        int children = obj.transform.childCount;
        Debug.Log("children: " + children);
        if(children == 0) {
            obj = parentObj;
        }

        RemoveAllComponents(children, obj);

        AddHologramShader(children, obj);
    }

    void DestroyChildren() {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    void RemoveAllComponents(int children, GameObject obj) {
        for (int i = 0; i < children; i++) {
            GameObject childObj = obj.transform.GetChild(i).gameObject;
            foreach (var comp in childObj.GetComponents<Component>()) {
                if (!(comp is Transform) && !(comp is MeshFilter) && !(comp is MeshRenderer)) {
                    Destroy(comp);
                }
            }
        }
    }

    void AddHologramShader(int children, GameObject obj) {
        for (int i = 0; i < children; i++) {
            GameObject childObj = obj.transform.GetChild(i).gameObject;
            // add mesh renderer with blue hologram material
            //childObj.AddComponent<UnityEngine.MeshRenderer>();
            childObj.GetComponent<MeshRenderer>().material = hologram;
            childObj.AddComponent<HolotableObject>();
        }
    }
}
