using UnityEngine;
using System.Collections;

// ----------------------------------------------------------------------
// CLASS : AIRobotState_Idle1
// DESC	 : An AIState that implements a Robots Idle Behaviour
// ----------------------------------------------------------------------
public class AIRobotState_Idle1 : AIRobotState 
{
	//Inspector Assigned 
	[SerializeField] Vector2 _idleTimeRange = new Vector2(10.0f, 60.0f);

	//Private
	float _idleTime	= 0.0f;
	float _timer	= 0.0f;

	// ------------------------------------------------------------------
	// Name	:	GetStateType
	// Desc	:	Returns the type of the state
	// ------------------------------------------------------------------
	public override AIStateType GetStateType()
	{
		return AIStateType.Idle;
	}

	// ------------------------------------------------------------------
	// Name	:	OnEnterState
	// Desc	:	Called by the State Machine when first transitioned into
	//			this state. It initializes a timer and configures the
	//			the state machine
	// ------------------------------------------------------------------
	public override void OnEnterState()			
	{
		Debug.Log ("Entering Idle State");

		base.OnEnterState ();

		if (_robotStateMachine == null)
			return;

		//Set Idle Time
		_idleTime = Random.Range (_idleTimeRange.x, _idleTimeRange.y);
		_timer 	  = 0.0f;

        //Configure State Machine
        _robotStateMachine.NavAgentControl  (true, false);
        _robotStateMachine.speed            = 0;
        _robotStateMachine.seeking          = 0;
        _robotStateMachine.feeding          = false;
        _robotStateMachine.attackType       = 0;

        _robotStateMachine.ClearTarget();
	}

	// -------------------------------------------------------------------
	// Name	:	OnUpdate
	// Desc	:	Called by the state machine each frame
	// -------------------------------------------------------------------
	public override AIStateType OnUpdate()
	{
		//No state machine then bail
		if (_robotStateMachine == null)
			return AIStateType.Idle;

        //if (_robotStateMachine.poweredUp)
        //{

            //Is the player visible
            if (_robotStateMachine.VisualThreat.type == AITargetType.Visual_Player)
            {
            //EOGHAN : PLAY SOUND WHEN PLAYER IS DETECTED
            _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
                return AIStateType.Pursuit;
            }

            //Is the threat a flashlight
            if (_robotStateMachine.VisualThreat.type == AITargetType.Visual_Light)
            {
            //EOGHAN : PLAY SOUND WHEN ROBOT IS SEARCHING HERE
            _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
                return AIStateType.Alerted;
            }

            //Is the threat an audio emitter
            if (_robotStateMachine.AudioThreat.type == AITargetType.Audio)
            {
            //EOGHAN : PLAY SOUND WHEN ROBOT IS SEARCHING HERE
                _robotStateMachine.SetTarget(_robotStateMachine.AudioThreat);
                return AIStateType.Alerted;
            }

            //Is the threat food
            if (_robotStateMachine.VisualThreat.type == AITargetType.Visual_Food)
            {
                _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);
                return AIStateType.Pursuit;
            }

            //Update the idle timer
            _timer += Time.deltaTime;

            //Patrol if idle time has been exceeded
            if (_timer > _idleTime)
            {
                _robotStateMachine.navAgent.SetDestination(_robotStateMachine.GetWaypointPosition(false));
                _robotStateMachine.navAgent.isStopped = false;
                return AIStateType.Alerted;
            }

        //}
		// No state change required
		return AIStateType.Idle;
	}
}
