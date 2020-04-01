using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulkheadControl : MonoBehaviour
{
    [SerializeField] private Animator _bulkheadAnimator = null;
    [SerializeField] private KlaxonExit[] _exitKlaxons = null;
    [SerializeField] private GameObject _trishPrematureExit = null;

    private bool _doorsOpen = false;    

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void PullSecurityLever()
    {
        if (!_doorsOpen)
        {
            _doorsOpen = true;

            if (_exitKlaxons != null)
            {
                _trishPrematureExit.SetActive(false);

                for(int i = 0; i < _exitKlaxons.Length; i++)
                {
                    _exitKlaxons[i].ActivateKlaxons();
                }
            }

            
            _bulkheadAnimator.SetTrigger("UnlockExit");
        }
    }
}
