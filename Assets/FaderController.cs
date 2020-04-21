using UnityEngine;
using UnityEditor;
using System.Collections;

public class FaderController : MonoBehaviour {
    public Animator animator;
    public CharacterManager cm;

    private void Start() {
    }

    public void FadeToBlack() {
        animator.SetTrigger("FadeOut");
    }

    public void FadeToClear() {
        animator.SetTrigger("FadeIn");
    }

    public void OnFadeComplete() {

        // teleport player to end if player is dead
        // fade in
        if(cm._Dead) {
            cm.KillPlayer();
            FadeToClear();
            return;
        }

        // bring to main menu if player gets to eye
        StartCoroutine(QuitGameTemp(3));
    }

    IEnumerator QuitGameTemp(float t) {
        yield return new WaitForSeconds(t);
        // bring to main menu if player gets to eye
        Application.Quit();
        EditorApplication.isPlaying = false;
    }
}
