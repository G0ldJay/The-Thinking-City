using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolotableObjectViewer : MonoBehaviour {
    private string currentObj;
    public Material hologram;

    public void LoadHoloObject(HoloObjHolder HOH) {
        GameObject newObj = HOH.GetObj();

        // do not load object in if the currently displayed obj is the same
        if(currentObj == "" || newObj.name == currentObj) {
            return;
        }

        // kill current obj
        DestroyChildren();

        //create obj with hologram shader & rotation anim
        GameObject parentObj = Instantiate(newObj, transform.position, Quaternion.identity);
        currentObj = newObj.name;
        //since we're using prefabs we need to get the child object & set new parent


        GameObject obj = parentObj.transform.GetChild(0).gameObject;
        obj.transform.parent = transform;

        int children = obj.transform.childCount;
        Debug.Log("children: " + children);
        if (newObj.name != "EYE") {
            if (children == 0) {
                // place object in an empty
                GameObject newParent = Instantiate(new GameObject("ParentEmpty"), transform);
                obj.transform.parent = newParent.transform;

                RemoveAllComponents(1, newParent);
                AddHologramShader(1, newParent);
                newParent.AddComponent<HolotableObject>();
            }
            else {
                RemoveAllComponents(children, obj);
                AddHologramShader(children, obj);

                for (int i = 0; i < transform.childCount; i++) {
                    transform.GetChild(i).gameObject.AddComponent<HolotableObject>();
                }
            }
        }

        if(newObj.name == "ERROR" || newObj.name == "EYE") {
            //set all objholder objects to error OR EYE
            GetComponent<SetButtonObjects>().SetObjs(newObj);
        }
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
            if (!childObj.name.Contains("noshader")) {
                Material[] mats = childObj.GetComponent<MeshRenderer>().materials;
                mats[0] = hologram;
                childObj.GetComponent<MeshRenderer>().materials = mats;
            }
        }
    }
}
