using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWin : MonoBehaviour
{
    private bool triggered = false;
    [SerializeField] private GameObject fusion = null;
    [SerializeField] private Collider winCollider = null;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Keycard") && !triggered)
        {
            if (fusion != null)
                fusion.SetActive(true);

            if(winCollider!=null)
                winCollider.enabled = true;

            triggered = true;
        }
    }
}
