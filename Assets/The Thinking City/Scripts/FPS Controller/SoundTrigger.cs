using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    private bool _soundPlayed = false;
    [SerializeField] private AudioSource _soundToPlay = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_soundPlayed)
            {
                if (_soundToPlay != null)
                {
                    _soundToPlay.Play();
                    _soundPlayed = true;
                }
            }
        }
    }
}
