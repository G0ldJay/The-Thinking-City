using UnityEngine;
using System.Collections;

// -----------------------------------------------------------------
// Class	: AIRobotState_Attack1
// Desc		: A Robot state used for attacking a target
// -----------------------------------------------------------------
public class AIRobotState_Attack1 :  AIRobotState 
{
	// Inspector Assigned
	[SerializeField]	[Range(0,10)]		 float	_speed					=	0.0f;
	[SerializeField]	[Range(0.0f,1.0f)]	 float	_lookAtWeight			= 	0.7f;
	[SerializeField]	[Range(0.0f, 90.0f)] float  _lookAtAngleThreshold	=	15.0f;
	[SerializeField]						 float	_slerpSpeed				=	5.0f;


	// Private Variables
	private float _currentLookAtWeight = 0.0f;

	// Mandatory Overrides
	public override AIStateType GetStateType() { return AIStateType.Attack; }

    // ------------------------------------------------------------------
    // Name	:	OnEnterState
    // Desc	:	Called by the State Machine when first transitioned into
    //			this state. It initializes and configures the
    //		    state machine
    // ------------------------------------------------------------------
    public override void 		OnEnterState()
	{
		Debug.Log ("Entering Attack State");

		base.OnEnterState ();
		if (_robotStateMachine == null)
			return;

        // Configure State Machine
        _robotStateMachine.NavAgentControl (true, false);
        _robotStateMachine.seeking 	= 0;
        _robotStateMachine.feeding 	= false;
        _robotStateMachine.attackType 	= Random.Range (1, 100);;
        _robotStateMachine.speed 		= _speed;
		_currentLookAtWeight = 0.0f;
	}

	public override void	OnExitState()
	{
        _robotStateMachine.attackType = 0;
	}

	// ---------------------------------------------------------------------
	// Name	:	OnUpdateAI
	// Desc	:	The engine of this state
	// ---------------------------------------------------------------------
	public override AIStateType	OnUpdate( )	
	{ 
		Vector3 targetPos;
		Quaternion newRot;

		// Do we have a visual threat that is the player
		if (_robotStateMachine.VisualThreat.type==AITargetType.Visual_Player)
		{
            // Set new target
            _robotStateMachine.SetTarget ( _stateMachine.VisualThreat );

			// If we are not in melee range any more than fo back to pursuit mode
			if (!_robotStateMachine.inMeleeRange)	return AIStateType.Pursuit;

			if (!_robotStateMachine.useRootRotation)
			{
				// Keep the zombie facing the player at all times
				targetPos = _robotStateMachine.targetPosition;
				targetPos.y = _robotStateMachine.transform.position.y;
				newRot = Quaternion.LookRotation (  targetPos - _robotStateMachine.transform.position);
                _robotStateMachine.transform.rotation = Quaternion.Slerp(_robotStateMachine.transform.rotation, newRot, Time.deltaTime* _slerpSpeed);
			}

            _robotStateMachine.attackType = Random.Range (1,100);

			return AIStateType.Attack;
		}

		// PLayer has stepped outside out FOV or hidden so face in his/her direction and then
		// drop back to Alerted mode to give the AI a chance to re-aquire target
		if (!_robotStateMachine.useRootRotation)
		{
			targetPos = _robotStateMachine.targetPosition;
			targetPos.y = _robotStateMachine.transform.position.y;
			newRot = Quaternion.LookRotation (  targetPos - _robotStateMachine.transform.position);
            _robotStateMachine.transform.rotation = newRot;
		}

		// Stay in Patrol State
		return AIStateType.Alerted;
	}

	// -----------------------------------------------------------------------
	// Name	:	OnAnimatorIKUpdated
	// Desc	:	Override IK Goals
	// -----------------------------------------------------------------------
	public override void 		OnAnimatorIKUpdated()	
	{
		if (_robotStateMachine == null)
			return;

		if (Vector3.Angle (_robotStateMachine.transform.forward, _robotStateMachine.targetPosition - _robotStateMachine.transform.position) < _lookAtAngleThreshold)
		{
            _robotStateMachine.animator.SetLookAtPosition (_robotStateMachine.targetPosition + Vector3.up );
			_currentLookAtWeight = Mathf.Lerp (_currentLookAtWeight, _lookAtWeight, Time.deltaTime);
            _robotStateMachine.animator.SetLookAtWeight (_currentLookAtWeight);
		} 
		else 
		{
			_currentLookAtWeight = Mathf.Lerp (_currentLookAtWeight, 0.0f, Time.deltaTime);
            _robotStateMachine.animator.SetLookAtWeight (_currentLookAtWeight);	
		}
	}
}
