using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLookAt : MonoBehaviour
{
    [SerializeField] private GameObject _lookAtTarget;
    // Update is called once per frame
    void Update()
    {
        _lookAtTarget = GameManager.instance.GetCurrentObjective();

        if(_lookAtTarget!= null)
        {
            gameObject.transform.LookAt(_lookAtTarget.transform);
        }
        
    }
}
