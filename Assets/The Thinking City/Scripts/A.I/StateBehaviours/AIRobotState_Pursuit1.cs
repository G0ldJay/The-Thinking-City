using UnityEngine;
using System.Collections;

// -----------------------------------------------------------------
// Class	: AIRobotState_Pursuit1
// Desc		: A Robot state used for pursuing a target
// -----------------------------------------------------------------
public class AIRobotState_Pursuit1 : AIRobotState
{
    [SerializeField] [Range(0.0f, 1.0f)] float      _lookAtWeight               = 0.7f;
    [SerializeField] [Range(0.0f, 90.0f)] float     _lookAtAngleThreshold       = 15.0f;
    [SerializeField] [Range(0, 10)] private float   _speed                      = 1.0f;
    [SerializeField] private float                  _slerpSpeed                 = 5.0f;
    [SerializeField] private float                  _repathDistanceMultiplier   = 0.035f;
    [SerializeField] private float                  _repathVisualMinDuration    = 0.05f;
    [SerializeField] private float                  _repathVisualMaxDuration    = 5.0f;
    [SerializeField] private float                  _repathAudioMinDuration     = 0.25f;
    [SerializeField] private float                  _repathAudioMaxDuration     = 5.0f;
    [SerializeField] private float                  _maxDuration                = 40.0f;

    //Private Fields
    private float _timer = 0.0f;
    private float _repathTimer = 0.0f;
    private float _currentLookAtWeight = 0.0f;

    //Mandatory Overrides
    public override AIStateType GetStateType() { return AIStateType.Pursuit; }

    //Default Handlers
    public override void OnEnterState()
    {
        Debug.Log("Entering Pursuit State");

        base.OnEnterState();

        if (_robotStateMachine == null)
            return;

        //Configure State Machine
        _robotStateMachine.NavAgentControl  (true, false);
        _robotStateMachine.speed            = _speed;
        _robotStateMachine.seeking          = 0;
        _robotStateMachine.feeding          = false;
        _robotStateMachine.attackType       = 0;
        _robotStateMachine.investigating    = 0;

        //Robots will only pursue for so long before breaking off
        _timer = 0.0f;
        _repathTimer = 0.0f;


        //Set path
        _robotStateMachine.navAgent.SetDestination(_robotStateMachine.targetPosition);
        _robotStateMachine.navAgent.isStopped = false;

        _currentLookAtWeight = 0.0f;
    }

    // ---------------------------------------------------------------------
    // Name	:	OnUpdateAI
    // Desc	:	The engine of this state
    // ---------------------------------------------------------------------
    public override AIStateType OnUpdate()
    {
        _timer += Time.deltaTime;
        _repathTimer += Time.deltaTime;

        if (_timer > _maxDuration)
            return AIStateType.Patrol;

        //If we are chasing the player and have entered the melee trigger then attack
        if (_robotStateMachine.targetType == AITargetType.Visual_Player && _robotStateMachine.inMeleeRange)
        {
            return AIStateType.Attack;
        }

        //Otherwise this is navigation to areas of interest so use the standard target threshold
        if (_robotStateMachine.isTargetReached)
        {
            switch (_stateMachine.targetType)
            {

                
                case AITargetType.Audio:            //If we have reached the audio source
                case AITargetType.Visual_Light:     //If we have reached the light source
                    _stateMachine.ClearTarget();    //Clear the threat
                    //EOGHAN PLAY ROBOT SEARCHING SOUND
                    //FMODUnity.RuntimeManager.PlayOneShotAttached("event:/ScreechVoiceLine", gameObject);
                    return AIStateType.Alerted;     //Become alert and scan for targets

                case AITargetType.Visual_Food:
                    return AIStateType.Feeding;
            }
        }


        //If for any reason the nav agent has lost its path then call then drop into alerted state so it will try to re-aquire the target or eventually giveup and resume patrolling
        if (_robotStateMachine.navAgent.isPathStale || !_robotStateMachine.navAgent.hasPath || _robotStateMachine.navAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            return AIStateType.Alerted;
        }


        //If we are close to the target that was a player and we still have the player in our vision then keep facing right at the player
        if (!_robotStateMachine.useRootRotation && _robotStateMachine.targetType == AITargetType.Visual_Player && _robotStateMachine.VisualThreat.type == AITargetType.Visual_Player && _robotStateMachine.isTargetReached)
        {
            Vector3 targetPos   = _robotStateMachine.targetPosition;
            targetPos.y         = _robotStateMachine.transform.position.y;
            Quaternion newRot   = Quaternion.LookRotation(targetPos - _robotStateMachine.transform.position);

            _robotStateMachine.transform.rotation = newRot;
        }
        else if (!_robotStateMachine.useRootRotation && !_robotStateMachine.isTargetReached) //Slowly update our rotation to match the nav agents desired rotation BUT only if we are not persuing the player and are really close to him
        {
            //Generate a new Quaternion representing the rotation we should have
            Quaternion newRot = Quaternion.LookRotation(_robotStateMachine.navAgent.desiredVelocity);

            //Smoothly rotate to that new rotation over time
            _robotStateMachine.transform.rotation = Quaternion.Slerp(_robotStateMachine.transform.rotation, newRot, Time.deltaTime * _slerpSpeed);
        }
        else if (_robotStateMachine.isTargetReached)
        {
            return AIStateType.Alerted;
        }

        //Do we have a visual threat that is the player
        if (_robotStateMachine.VisualThreat.type == AITargetType.Visual_Player)
        {
            //The position is different - maybe same threat but it has moved so repath periodically
            if (_robotStateMachine.targetPosition != _robotStateMachine.VisualThreat.position)
            {
                //Repath more frequently as we get closer to the target (try and save some CPU cycles)
                if (Mathf.Clamp(_robotStateMachine.VisualThreat.distance * _repathDistanceMultiplier, _repathVisualMinDuration, _repathVisualMaxDuration) < _repathTimer)
                {
                    //Repath the agent
                    _robotStateMachine.navAgent.SetDestination(_robotStateMachine.VisualThreat.position);
                    _repathTimer = 0.0f;
                }
            }
            //Make sure this is the current target
            _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);

            //Remain in pursuit state
            return AIStateType.Pursuit;
        }

        //If our target is the last sighting of a player then remain in pursuit as nothing else can override
        if (_robotStateMachine.targetType == AITargetType.Visual_Player)
            return AIStateType.Pursuit;

        // If we have a visual threat that is the player's light
        if (_robotStateMachine.VisualThreat.type == AITargetType.Visual_Light)
        {
            // and we currently have a lower priority target then drop into alerted mode and try to find source of light
            if (_robotStateMachine.targetType == AITargetType.Audio || _robotStateMachine.targetType == AITargetType.Visual_Food)
            {
                _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
                return AIStateType.Alerted;
            }
            else if (_robotStateMachine.targetType == AITargetType.Visual_Light)
            {
                // Get unique ID of the collider of our target
                int currentID = _robotStateMachine.targetColliderID;

                // If this is the same light
                if (currentID == _robotStateMachine.VisualThreat.collider.GetInstanceID())
                {
                    // The position is different - maybe same threat but it has moved so repath periodically
                    if (_robotStateMachine.targetPosition != _robotStateMachine.VisualThreat.position)
                    {
                        // Repath more frequently as we get closer to the target (try and save some CPU cycles)
                        if (Mathf.Clamp(_robotStateMachine.VisualThreat.distance * _repathDistanceMultiplier, _repathVisualMinDuration, _repathVisualMaxDuration) < _repathTimer)
                        {
                            // Repath the agent
                            _robotStateMachine.navAgent.SetDestination(_robotStateMachine.VisualThreat.position);
                            _repathTimer = 0.0f;
                        }
                    }

                    _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
                    return AIStateType.Pursuit;
                }
                else
                {
                    _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
                    return AIStateType.Alerted;
                }
            }
        }
        else if (_robotStateMachine.AudioThreat.type == AITargetType.Audio)
        {
            if (_robotStateMachine.targetType == AITargetType.Visual_Food)
            {
                _robotStateMachine.SetTarget(_robotStateMachine.AudioThreat);
                return AIStateType.Alerted;
            }
            else if (_robotStateMachine.targetType == AITargetType.Audio)
            {
                // Get unique ID of the collider of our target
                int currentID = _robotStateMachine.targetColliderID;

                // If this is the same light
                if (currentID == _robotStateMachine.AudioThreat.collider.GetInstanceID())
                {
                    // The position is different - maybe same threat but it has moved so repath periodically
                    if (_robotStateMachine.targetPosition != _robotStateMachine.AudioThreat.position)
                    {
                        // Repath more frequently as we get closer to the target (try and save some CPU cycles)
                        if (Mathf.Clamp(_robotStateMachine.AudioThreat.distance * _repathDistanceMultiplier, _repathAudioMinDuration, _repathAudioMaxDuration) < _repathTimer)
                        {
                            // Repath the agent
                            _robotStateMachine.navAgent.SetDestination(_robotStateMachine.AudioThreat.position);
                            _repathTimer = 0.0f;
                        }
                    }

                    _robotStateMachine.SetTarget(_robotStateMachine.AudioThreat);
                    return AIStateType.Pursuit;
                }
                else
                {
                    _robotStateMachine.SetTarget(_robotStateMachine.AudioThreat);
                    return AIStateType.Alerted;
                }
            }
        }

        // Default
        return AIStateType.Pursuit;
    }

    //NOTE : Won't work with current model. Needs to be reworked
    //public override void OnAnimatorIKUpdated()
    //{
    //    if (_robotStateMachine == null) return;

    //    if (Vector3.Angle(_robotStateMachine.transform.forward, _robotStateMachine.targetPosition - _robotStateMachine.transform.position) < _lookAtAngleThreshold)
    //    {
    //        _robotStateMachine.animator.SetLookAtPosition(_robotStateMachine.targetPosition + Vector3.up);
    //        _currentLookAtWeight = Mathf.Lerp(_currentLookAtWeight, _lookAtWeight, Time.deltaTime);
    //        _robotStateMachine.animator.SetLookAtWeight(_currentLookAtWeight);
    //    }
    //    else
    //    {
    //        _currentLookAtWeight = Mathf.Lerp(_currentLookAtWeight, 0.0f, Time.deltaTime);
    //        _robotStateMachine.animator.SetLookAtWeight(_currentLookAtWeight);
    //    }
    //}
}

