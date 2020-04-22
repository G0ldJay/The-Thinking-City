using UnityEngine;
using UnityEditor;
using Valve.VR;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathMenuOptions : MonoBehaviour {

    public SteamVR_LoadLevel levelLoader;

    public void Restart() {
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SteamVR_LoadLevel.Begin(levelLoader.levelName); 
    }

    public void Quit() {
        Application.Quit();
        //EditorApplication.isPlaying = false;
    }
}
