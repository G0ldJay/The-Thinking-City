using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDamageTrigger : MonoBehaviour
{
    [SerializeField] string _parameter = "";
    [SerializeField] int _bloodParticlesBurstAmount = 10;

    private AIStateMachine _stateMachine = null;
    private Animator _animator = null;
    private int _parameterHash = -1;
    // private bool _attacked = false;

    private void Start()
    {
        _stateMachine = transform.root.GetComponentInChildren<AIStateMachine>();

        if (_stateMachine != null) _animator = _stateMachine.animator;

        _parameterHash = Animator.StringToHash(_parameter);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_animator) return;

        if (other.gameObject.CompareTag("Player") && _animator.GetFloat(_parameterHash) > 0.9f)
        {
            Debug.Log("Player hit by " + _parameter + "!");

            if (GameManager.instance && GameManager.instance.bloodParticles)
            {
                ParticleSystem system = GameManager.instance.bloodParticles;
                system.transform.position = transform.position;
                system.transform.rotation = Camera.main.transform.rotation;

                var main = system.main;
                main.simulationSpace = ParticleSystemSimulationSpace.World;

                system.Emit(_bloodParticlesBurstAmount);

            }
            //_attacked = !_attacked;
        }

    }

    //private void OnTriggerExit(Collider other)
    //{
    //    _attacked = !_attacked;
    //}
}

