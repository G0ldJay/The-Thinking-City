using UnityEngine;

public class FaderController : MonoBehaviour {
    public Animator animator;


    public void FadeToBlack() {
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete() {

        // teleport player to end if player is dead
        // fade in



        // bring to main menu if player gets to eye
    }
}
