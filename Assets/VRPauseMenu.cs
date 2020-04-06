using UnityEngine;
using UnityEditor;
using Valve.VR;
using UnityEngine.SceneManagement;

public class VRPauseMenu : MonoBehaviour {

    public SteamVR_Action_Boolean PauseToggle;
    public SteamVR_Input_Sources _handSource;
    public SteamVR_Action_Boolean _ClickAction;
    public static bool paused = false;
    public GameObject PauseMenuUi;
    public GameObject pointer;

    private void Start() {
        PauseToggle.AddOnStateDownListener(Pause, _handSource);
    }

    public void Resume() {
        paused = false;
        PauseMenuUi.SetActive(false);
        pointer.GetComponent<Pointer>().renderLine = false;
        Time.timeScale = 1f;
    }

    public void Pause(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        // resume if already paused
        if(paused) {
            Resume();
            return;
        }
        // otherwise, pause
        paused = true;
        PauseMenuUi.SetActive(true);
        pointer.GetComponent<Pointer>().renderLine = true;
        Time.timeScale = 0f;
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit() {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }
}
