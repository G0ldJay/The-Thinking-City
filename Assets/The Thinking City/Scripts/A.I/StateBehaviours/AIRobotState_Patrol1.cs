using UnityEngine;
using System.Collections;

// ----------------------------------------------------------------
// CLASS	:	AIRobotState_Patrol1
// DESC		:	Generic Patrolling Behaviour for a Robot
// ----------------------------------------------------------------
public class AIRobotState_Patrol1 : AIRobotState
{
    //Inpsector Assigned 
    [SerializeField] float _turnOnSpotThreshold = 80.0f;
    [SerializeField] float _slerpSpeed = 5.0f;

    [SerializeField] [Range(0.0f, 3.0f)] float _speed = 1.0f;

    // ------------------------------------------------------------
    // Name	:	GetStateType
    // Desc	:	Called by parent State Machine to get this state's
    //			type.
    // ------------------------------------------------------------
    public override AIStateType GetStateType()
    {
        return AIStateType.Patrol;
    }

    // ------------------------------------------------------------------
    // Name	:	OnEnterState
    // Desc	:	Called by the State Machine when first transitioned into
    //			this state. It initializes the state machine
    // ------------------------------------------------------------------
    public override void OnEnterState()
    {
        Debug.Log("Entering Patrol State");

        base.OnEnterState();

        if (_robotStateMachine == null)
            return;

        //Configure State Machine
        _robotStateMachine.NavAgentControl  (true, false);
        _robotStateMachine.speed            = _speed;
        _robotStateMachine.seeking          = 0;
        _robotStateMachine.feeding          = false;
        _robotStateMachine.attackType       = 0;

        //Set Destination
        _robotStateMachine.navAgent.SetDestination(_robotStateMachine.GetWaypointPosition(false));

        //Make sure NavAgent is switched on
        _robotStateMachine.navAgent.isStopped = false;
    }


    // ------------------------------------------------------------
    // Name	:	OnUpdate
    // Desc	:	Called by the state machine each frame to give this
    //			state a time-slice to update itself. It processes 
    //			threats and handles transitions as well as keeping
    //			the Robot aligned with its proper direction in the
    //			case where root rotation isn't being used.
    // ------------------------------------------------------------
    public override AIStateType OnUpdate()
    {
        //Do we have a visual threat that is the player
        if (_robotStateMachine.VisualThreat.type == AITargetType.Visual_Player)
        {
            //EOGHAN : PLAY SOUND WHEN PLAYER IS DETECTED
            _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
            return AIStateType.Pursuit;
        }

        if (_robotStateMachine.VisualThreat.type == AITargetType.Visual_Light)
        {
            //EOGHAN : PLAY SOUND WHEN ROBOT SEARCHES
            _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
            return AIStateType.Alerted;
        }

        //Sound is the third highest priority
        if (_robotStateMachine.AudioThreat.type == AITargetType.Audio)
        {
            //EOGHAN : PLAY SOUND WHEN ROBOT SEARCHES
            _robotStateMachine.SetTarget(_robotStateMachine.AudioThreat);
            return AIStateType.Alerted;
        }

        if (_robotStateMachine.VisualThreat.type == AITargetType.Visual_Food)
        {
            //If the distance to hunger ratio means we are hungry enough to stray off the path that far
            if ((1.0f - _robotStateMachine.satisfaction) > (_robotStateMachine.VisualThreat.distance / _robotStateMachine.sensorRadius))
            {
                _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
                return AIStateType.Pursuit;
            }
        }

        //Calculate angle we need to turn through to be facing our target
        float angle = Vector3.Angle(_robotStateMachine.transform.forward, (_robotStateMachine.navAgent.steeringTarget - _robotStateMachine.transform.position));

        //If its too big then drop out of Patrol and into Altered
        if (angle > _turnOnSpotThreshold)
        {
            return AIStateType.Alerted;
        }

        //If root rotation is not being used then we are responsible for keeping Robot rotated and facing in the right direction. 
        if (!_robotStateMachine.useRootRotation)
        {
            //Generate a new Quaternion representing the rotation we should have
            Quaternion newRot = Quaternion.LookRotation(_robotStateMachine.navAgent.desiredVelocity);

            //Smoothly rotate to that new rotation over time
            _robotStateMachine.transform.rotation = Quaternion.Slerp(_robotStateMachine.transform.rotation, newRot, Time.deltaTime * _slerpSpeed);
        }

        //If for any reason the nav agent has lost its path then call the NextWaypoint function so a new waypoint is selected and a new path assigned to the nav agent.
        if (_robotStateMachine.navAgent.isPathStale || !_robotStateMachine.navAgent.hasPath || _robotStateMachine.navAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            _robotStateMachine.navAgent.SetDestination(_robotStateMachine.GetWaypointPosition(true));
        }

        // Stay in Patrol State
        return AIStateType.Patrol;
    }



    // ----------------------------------------------------------------------
    // Name	:	OnDestinationReached
    // Desc	:	Called by the parent StateMachine when the Robot has reached
    //			its target (entered its target trigger)
    // ----------------------------------------------------------------------
    public override void OnDestinationReached(bool isReached)
    {
        // Only interesting in processing arricals not departures
        if (_robotStateMachine == null || !isReached)
            return;

        // Select the next waypoint in the waypoint network
        if (_robotStateMachine.targetType == AITargetType.Waypoint)
            _robotStateMachine.navAgent.SetDestination(_robotStateMachine.GetWaypointPosition(true));
    }

    //NOTE : Won't work with current non-humanoid model
    // -----------------------------------------------------------------------
    // Name	:	OnAnimatorIKUpdated
    // Desc	:	Override IK Goals
    // -----------------------------------------------------------------------
    /*public override void 		OnAnimatorIKUpdated()	
	{
		if (_robotStateMachine == null)
			return;

		_robotStateMachine.animator.SetLookAtPosition ( _robotStateMachine.targetPosition + Vector3.up );
		_robotStateMachine.animator.SetLookAtWeight (0.55f );
	}*/
}
