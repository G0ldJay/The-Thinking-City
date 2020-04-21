﻿using UnityEngine;
using UnityEditor;
using Valve.VR;
using UnityEngine.SceneManagement;
using System.Collections;

public class VRPauseMenu : MonoBehaviour {

    public SteamVR_Action_Boolean PauseToggle;
    public SteamVR_Input_Sources _handSource;
    public static bool paused = false;
    public GameObject PauseMenuUi;
    public GameObject pointer;
    public SteamVR_LoadLevel levelLoader;

    private bool canPause = false;

    private void Awake() {
        PauseToggle.AddOnStateDownListener(Pause, _handSource);
        PauseMenuUi = GameObject.Find("PauseMenuOptions");

        PauseMenuUi.SetActive(false);
        StartCoroutine(AllowPause(3));
    }

    IEnumerator AllowPause(float time) {
        yield return new WaitForSeconds(time);
        canPause = true;
    }

    public void Resume() {
        paused = false;
        PauseMenuUi.SetActive(false);
        ActivatePointer(false);
        Time.timeScale = 1f;
        SteamVR_Fade.Start(Color.black, 0);
        SteamVR_Fade.Start(Color.clear, 1);
    }

    public void Pause(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        if (!canPause) return;

        // resume if already paused
        if(paused) {
            Resume();
            return;
        }
        // otherwise, pause
        paused = true;
        PauseMenuUi.SetActive(true);
        ActivatePointer(true);
        Time.timeScale = 0f;
    }

    public void Restart() {
        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resume();
        SteamVR_LoadLevel.Begin(levelLoader.levelName);
    }

    public void Quit() {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }

    private void ActivatePointer(bool onOff) {
        pointer.SetActive(onOff);
        foreach (Transform t in pointer.transform) {
            t.gameObject.SetActive(onOff);
        }
    }
}


