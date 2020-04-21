using System.Collections;
using UnityEngine;

public class EyeEndingEvent : MonoBehaviour
{
    [SerializeField] private GameObject _trishEyeAudio      = null;
    [SerializeField] private Animator   _eyeAnimator        = null;
    [SerializeField] private Animator   _exitDoorAnimator   = null;

    // Start is called before the first frame update
    void Start()
    {
        if (_trishEyeAudio != null)
        {
            _trishEyeAudio.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_trishEyeAudio != null)
            {
                _trishEyeAudio.SetActive(true);
            }
            if (_eyeAnimator != null)
            {
                _eyeAnimator.SetTrigger("OpenEye");
            }

            // stop player from moving
            FindObjectOfType<Oisin_PlayerController>().canMove = false;

            // start fadeout after a few secs
            StartCoroutine(FadeToMainMenu(4));
        }
    }

    IEnumerator FadeToMainMenu(float time) {
        yield return new WaitForSeconds(time);
        FindObjectOfType<FaderController>().FadeToBlack();
    }
}
