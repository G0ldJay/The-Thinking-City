using System.Collections;
using UnityEngine;
using TMPro;

public class DoorButtonPanel : MonoBehaviour {
    public int[] keycode = new int[4];
    public Color[] colours = new Color[3]; // blue, red, green
    public DoorFunctionality df;
    public Light light;
    public TextMeshProUGUI codeText;
    private int count = 0;

    public void SetKey(int key) {
        keycode[count] = key;
        SetText();
        count++;
    }

    public void SetText() {
        codeText.text = keycode[0] + "" + keycode[1] + "" + keycode[2] + "" + keycode[3];
    }

    public void ResetKeycode() {
        for(int i = 0; i < 4; ++i) {
            keycode[i] = 0;
        }
        count = 0;
        SetText();
    }

    public void CheckKeycode() {
        if(keycode[0] == 2 && keycode[1] == 3 && keycode[2] == 1 && keycode[3] == 4) {
            df.ActivateDoor();
            //set light to green
            StartCoroutine(ChangeColor(colours[2]));
            // TODO - Add success sound here
        }
        else {
            // set light to red & play error sound
            StartCoroutine(ChangeColor(colours[1]));
            // TODO - Add error sound here
        }
        ResetKeycode();
    }


    IEnumerator ChangeColor(Color colour) {
        light.color = colour;
        yield return new WaitForSeconds(1);
        light.color = colours[0];
    }
}
