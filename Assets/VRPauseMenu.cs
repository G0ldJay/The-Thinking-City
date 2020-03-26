
using UnityEngine;
using UnityEditor;
using Valve.VR;
using UnityEngine.SceneManagement;

public class VRPauseMenu : MonoBehaviour {

    public SteamVR_Action_Boolean PauseToggle;
    public static bool paused = false;
    public GameObject PauseMenuUi;
    public GameObject pointer;

    void Update() {
        if(PauseToggle.state) {
            if(paused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        paused = false;
        PauseMenuUi.SetActive(false);
        pointer.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause() {
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
