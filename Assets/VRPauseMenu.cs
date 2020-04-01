using UnityEngine;
using UnityEditor;
using Valve.VR;
using UnityEngine.SceneManagement;

public class VRPauseMenu : MonoBehaviour {

    public SteamVR_Action_Boolean PauseToggle;
    public SteamVR_Input_Sources handType;
    public static bool paused = false;
    public GameObject PauseMenuUi;
    public GameObject pointer;

    private void Start() {
        PauseToggle.AddOnStateDownListener(Pause, handType);
    }

    public void Resume() {
        paused = false;
        PauseMenuUi.SetActive(false);
        pointer.SetActive(false);
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
        pointer.SetActive(true);
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
