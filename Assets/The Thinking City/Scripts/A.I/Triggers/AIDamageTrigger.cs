using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDamageTrigger : MonoBehaviour
{
    // Inspector Variables
    [SerializeField] string     _parameter                  = "";
    [SerializeField] int        _bloodParticlesBurstAmount  = 10;
    [SerializeField] float      _damageAmount               = 25.0f;

    // Private Variables
    private AIStateMachine  _stateMachine   = null;
    private Animator        _animator       = null;
    private int             _parameterHash  = -1;
    GameManager             _gameManager    = null;
    // private bool _attacked = false;

    // ------------------------------------------------------------
    // Name	:	Start
    // Desc	:	Called on object start-up to initialize the script.
    // ------------------------------------------------------------
    private void Start()
    {
        // Cache state machine and animator references
        _stateMachine = transform.root.GetComponentInChildren<AIStateMachine>();

        if (_stateMachine != null) _animator = _stateMachine.animator;

        // Generate parameter hash for more efficient parameter lookups from the animator
        _parameterHash = Animator.StringToHash(_parameter);
    }

    // -------------------------------------------------------------
    // Name	:	OnTriggerStay
    // Desc	:	Called by Unity each fixed update that THIS trigger
    //			is in contact with another.
    // -------------------------------------------------------------
    private void OnTriggerStay(Collider other)
    {
        // If we don't have an animator return
        if (!_animator) return;

        // If this is the player object and our parameter is set for damage
        if (other.gameObject.CompareTag("Player") && _animator.GetFloat(_parameterHash) > 0.9f)
        {
            Debug.Log("Player hit by " + _parameter + "!");

            if (GameManager.instance && GameManager.instance.bloodParticles)
            {
                ParticleSystem system = GameManager.instance.bloodParticles; //TODO : Recode particle system as effect system. Particle system is defunct 

                // Temporary Code
                system.transform.position = transform.position;
                system.transform.rotation = Camera.main.transform.rotation;

                var main = system.main;
                main.simulationSpace = ParticleSystemSimulationSpace.World;

                system.Emit(_bloodParticlesBurstAmount);

            }
            //_attacked = !_attacked;

            if(_gameManager != null)
            {
                PlayerInfo info = _gameManager.GetPlayerInfo(other.GetInstanceID());

                if (info != null && info.characterManager != null) info.characterManager.TakeDamage(_damageAmount);
            }
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    _attacked = !_attacked;
    //}
}

