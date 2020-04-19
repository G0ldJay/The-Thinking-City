using UnityEngine;
using System.Collections;

// ----------------------------------------------------------------------
// CLASS : AIRobotState_Alerted1
// DESC	 : An AIState that implements a Robots Alerted Behaviour
// ----------------------------------------------------------------------
public class AIRobotState_Alerted1 : AIRobotState
{
    // Inspector Assigned
    [SerializeField] [Range(1, 60)] float   _maxDuration                = 10.0f;
    [SerializeField] float                  _waypointAngleThreshold     = 90.0f;
    [SerializeField] float                  _threatAngleThreshold       = 10.0f;
    [SerializeField] float                  _directionChangeTime        = 1.5f;
    [SerializeField] public SoundHandler           soundHandler;

    // Private Fields
    float _timer = 0.0f;
    float _directionChangeTimer = 0.0f;

    // ------------------------------------------------------------------
    // Name	:	GetStateType
    // Desc	:	Returns the type of the state
    // ------------------------------------------------------------------
    public override AIStateType GetStateType()
    {
        return AIStateType.Alerted;
    }

    // ------------------------------------------------------------------
    // Name	:	OnEnterState
    // Desc	:	Called by the State Machine when first transitioned into
    //			this state. It initializes a timer and configures the
    //			the state machine
    // ------------------------------------------------------------------
    public override void OnEnterState()
    {
        Debug.Log("Entering Alerted State");

        base.OnEnterState();

        if (_robotStateMachine == null)
            return;

        // Configure State Machine
        _robotStateMachine.NavAgentControl  (true, false);
        _robotStateMachine.speed            = 0;
        _robotStateMachine.seeking          = 0;
        _robotStateMachine.feeding          = false;
        _robotStateMachine.attackType       = 0;
        _robotStateMachine.investigating    = 0;

        _timer = _maxDuration;
        _directionChangeTimer = 0.0f;
    }

    // ---------------------------------------------------------------------
    // Name	:	OnUpdate
    // Desc	:	The engine of this state
    // ---------------------------------------------------------------------
    public override AIStateType OnUpdate()
    {
        // Reduce Timer
        _timer                  -= Time.deltaTime;
        _directionChangeTimer   += Time.deltaTime;

        // Transition into a patrol state if available
        if (_timer <= 0.0f)
        {
            _robotStateMachine.navAgent.SetDestination(_robotStateMachine.GetWaypointPosition(false));
            _robotStateMachine.navAgent.isStopped = false;
            _timer = _maxDuration;
        }

        // Do we have a visual threat that is the player. These take priority over audio threats
        if (_robotStateMachine.VisualThreat.type == AITargetType.Visual_Player)
        {
            //EOGHAN : PLAY SOUND WHEN PLAYER IS DETECTED
            soundHandler.playVoiceLine();
            _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
            return AIStateType.Pursuit;
        }

        if (_robotStateMachine.AudioThreat.type == AITargetType.Audio)
        {
            _robotStateMachine.SetTarget(_robotStateMachine.AudioThreat);
            _timer = _maxDuration;
        }

        if (_robotStateMachine.VisualThreat.type == AITargetType.Visual_Light)
        {
            _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
            _timer = _maxDuration;
        }

        if (_robotStateMachine.AudioThreat.type == AITargetType.None && _robotStateMachine.VisualThreat.type == AITargetType.Visual_Food && _robotStateMachine.targetType == AITargetType.None)
        {
            _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
            return AIStateType.Pursuit;
        }

        float angle;

        if ((_robotStateMachine.targetType == AITargetType.Audio || _robotStateMachine.targetType == AITargetType.Visual_Light) && !_robotStateMachine.isTargetReached)
        {
            angle = AIState.FindSignedAngle(_robotStateMachine.transform.forward, _robotStateMachine.targetPosition - _robotStateMachine.transform.position);

            if (_robotStateMachine.targetType == AITargetType.Audio && Mathf.Abs(angle) < _threatAngleThreshold)
            {
                return AIStateType.Pursuit;
            }

            if (_directionChangeTimer > _directionChangeTime)
            {
                if (Random.value < _robotStateMachine.intelligence)
                {
                    _robotStateMachine.seeking = (int)Mathf.Sign(angle);
                }
                else
                {
                    _robotStateMachine.seeking = (int)Mathf.Sign(Random.Range(-1.0f, 1.0f));
                }

                _directionChangeTimer = 0.0f;
            }
        }
        else if (_robotStateMachine.targetType == AITargetType.Waypoint && !_robotStateMachine.navAgent.pathPending)
        {
            angle = AIState.FindSignedAngle(_robotStateMachine.transform.forward, _robotStateMachine.navAgent.steeringTarget - _robotStateMachine.transform.position);

            if (Mathf.Abs(angle) < _waypointAngleThreshold) return AIStateType.Patrol;

            if (_directionChangeTimer > _directionChangeTime)
            {
                _robotStateMachine.investigating = 0;
                _robotStateMachine.seeking = (int)Mathf.Sign(angle);
                _directionChangeTimer = 0.0f;
            }
        }
        else
        {
            if (_directionChangeTimer > _directionChangeTime)
            {
                _robotStateMachine.investigating = 1;
                //_robotStateMachine.seeking = (int)Mathf.Sign(Random.Range(-1.0f, 1.0f));
                _directionChangeTimer = 0.0f;
            }
        }

        return AIStateType.Alerted;
    }
}
