using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeEndingEvent : MonoBehaviour
{
    [SerializeField] private GameObject _trishEyeAudio      = null;
    [SerializeField] private Animator   _eyeAnimator        = null;
    [SerializeField] private Animator   _exitDoorAnimator   = null;

    // Start is called before the first frame update
    void Start()
    {
        _trishEyeAudio.SetActive(false);
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
        }
    }
}
