﻿using UnityEngine;
using System.Collections;

// -----------------------------------------------------------------------------------------
// CLASS	:	AIRobotState
// DESC		:	The immediate base class of all Robot states. It provides the event
//				processing and storage of the current threats.
// ----------------------------------------------------------------------------------------
public abstract class AIRobotState : AIState 
{
	// Private
	protected int 						_playerLayerMask 	= -1;
	protected int 						_bodyPartLayer		= -1;
	protected int						_visualLayerMask	= -1;
	protected AIRobotStateMachine       _robotStateMachine = null;

	// -------------------------------------------------------------------------------------
	// Name	:	Awake
	// Desc	:	Calculate the masks and layers used for raycasting and layer testing
	// -------------------------------------------------------------------------------------
	void Awake()
	{
		// Get a mask for line of sight testing with the player.
		_playerLayerMask = LayerMask.GetMask ("Player", "AI Body Part")+1;
		_visualLayerMask = LayerMask.GetMask ("Player", "AI Body Part", "Visual Aggravator")+1;

        // Get the layer index of the AI Body Part layer
        _bodyPartLayer 	= LayerMask.NameToLayer ("AI Body Part");
	}

	// -------------------------------------------------------------------------------------
	// Name	:	SetStateMachine
	// Desc	:	Check for type compliance and store reference as derived type
	// -------------------------------------------------------------------------------------
	public override void SetStateMachine( AIStateMachine stateMachine )
	{
		if (stateMachine.GetType () == typeof(AIRobotStateMachine)) 
		{
			
			base.SetStateMachine (stateMachine);
            _robotStateMachine = (AIRobotStateMachine)stateMachine;

		}
	}

	// -------------------------------------------------------------------------------------
	// Name	:	OnTriggerEvent
	// Desc	:	Called by the parent state machine when threats enter/stay/exit the Robots
	//			sensor trigger, this will be any colliders assigned to the Visual or Audio
	//			Aggravator layers or the player.
	//			It examines the threat and stores it in the parent machine Visual or Audio
	//			threat members if found to be a higher priority threat.
	// --------------------------------------------------------------------------------------
	public override void OnTriggerEvent( AITriggerEventType eventType, Collider other )
	{
		//If we don't have a parent state machine then exit
		if (_robotStateMachine == null)
			return;

		//We are not interested in exit events so only step in and process if its an enter or stay.
		if (eventType != AITriggerEventType.Exit) 
		{
			//What is the type of the current visual threat we have stored
			AITargetType curType = _robotStateMachine.VisualThreat.type;

			//If the collider that has entered our sensor is a player
			if (other.CompareTag ("Player")) 
            {
				//Get distance from the sensor origin to the collider
				float distance = Vector3.Distance (_robotStateMachine.sensorPosition, other.transform.position);

				//If the currently stored threat is not a player or if this player is closer than a player previously stored as the visual threat, this could be more important
				if (curType != AITargetType.Visual_Player || (curType == AITargetType.Visual_Player && distance < _robotStateMachine.VisualThreat.distance)) 
                {
					// Is the collider within our view cone and do we have line or sight
					RaycastHit hitInfo;
					if (ColliderIsVisible (other, out hitInfo, _playerLayerMask)) 
					{
                        //it's close and in our FOV and we have line of sight so store as the current most dangerous threat
                        _robotStateMachine.VisualThreat.Set (AITargetType.Visual_Player, other, other.transform.position, distance);
					}
				}
			}
            //If the collider that has entered our sensor is a flash light & we don't have a player in view
            else if (other.CompareTag ("Flash Light") && curType != AITargetType.Visual_Player) 
			{
				BoxCollider flashLightTrigger   = (BoxCollider)other;
				float distanceToThreat          = Vector3.Distance (_robotStateMachine.sensorPosition, flashLightTrigger.transform.position);
				float zSize                     = flashLightTrigger.size.z * flashLightTrigger.transform.lossyScale.z;
				float aggrFactor                = distanceToThreat / zSize;

				if (aggrFactor <= _robotStateMachine.sight && aggrFactor <= _robotStateMachine.intelligence) //If you're light is close enough that its sight and intelligence is higher than your distance
				{
                    _robotStateMachine.VisualThreat.Set ( AITargetType.Visual_Light, other, other.transform.position, distanceToThreat); //Set new targget to be this
				}
			}
			else if (other.CompareTag ("AI Sound Emitter")) //If an alarming sound has come into sensor radius
			{
				SphereCollider  soundTrigger = (SphereCollider) other;
				if (soundTrigger==null) return;
				
				//Get the position of the Agent Sensor 
				Vector3 agentSensorPosition = _robotStateMachine.sensorPosition;

				Vector3 soundPos;
				float   soundRadius;

				AIState.ConvertSphereColliderToWorldSpace( soundTrigger, out soundPos, out soundRadius ); 

				//How far inside the sound's radius are we
				float distanceToThreat = (soundPos - agentSensorPosition).magnitude; 

				//Calculate a distance factor such that it is 1.0 when at sound radius 0 when at center
				float distanceFactor = (distanceToThreat / soundRadius);
				
				//Bias the factor based on hearing ability of Agent.
				distanceFactor += distanceFactor*(1.0f - _robotStateMachine.hearing);
				
				//Too far away
				if (distanceFactor > 1.0f)
					return;
						
				// if We can hear it and is it closer then what we previously have stored
				if (distanceToThreat < _robotStateMachine.AudioThreat.distance)
				{
                    //Most dangerous Audio Threat so far
                    _robotStateMachine.AudioThreat.Set ( AITargetType.Audio, other, soundPos, distanceToThreat );
				}
			}
            //If "food/charging station" has come into view and we have no other threats in view and the satisfaction level is low 
			else if (other.CompareTag ("AI Food") && curType != AITargetType.Visual_Player && curType != AITargetType.Visual_Light && _robotStateMachine.satisfaction <=0.9f && _robotStateMachine.AudioThreat.type == AITargetType.None   )
			{	
				//How far away is the threat from us
				float distanceToThreat = Vector3.Distance( other.transform.position, _robotStateMachine.sensorPosition);

				//Is this smaller then anything we have previous stored
				if (distanceToThreat < _robotStateMachine.VisualThreat.distance)
				{
					//If so then check that it is in our FOV and it is within the range of this AIs sight
					RaycastHit hitInfo;

					if ( ColliderIsVisible( other, out hitInfo, _visualLayerMask))
					{
                        //This is our most appealing target so far
                        _robotStateMachine.VisualThreat.Set ( AITargetType.Visual_Food, other, other.transform.position, distanceToThreat );
					}
				}
			}

		}
	}

	// -------------------------------------------------------------------------------------
	// Name	:	ColliderIsVisible
	// Desc	:	Test the passed collider against the Robot FOV and using the passed
	//			layer mask for line of sight testing.
	// -------------------------------------------------------------------------------------
	protected virtual bool	ColliderIsVisible( Collider other,  out RaycastHit hitInfo, int layerMask=-1 )
	{
		//Let's make sure we have something to return
		hitInfo = new RaycastHit ();

		//We need the state machine to be an AIRobotStateMachine
		if (_robotStateMachine == null) return false; 

		//Calculate the angle between the sensor origin and the direction of the collider
		Vector3 head 		= _stateMachine.sensorPosition;
		Vector3 direction	= other.transform.position - head;
		float	angle 		= Vector3.Angle (direction, transform.forward);

		//If thr angle is greater than half our FOV then it is outside the view cone so
		//return false - no visibility
		if (angle > _robotStateMachine.fov * 0.5f)
			return false;

		//Now we need to test line of sight. Perform a ray cast from our sensor origin in the direction of the collider for distance
		//of our sensor radius scaled by the Robots sight ability. This will return ALL hits.
		RaycastHit[] hits = Physics.RaycastAll( head, direction.normalized, _robotStateMachine.sensorRadius * _robotStateMachine.sight, layerMask);

		// Find the closest collider that is NOT the AIs own body part. If its not the target then the target is obstructed
		float 		closestColliderDistance = float.MaxValue;
		Collider	closestCollider			= null;

		//Examine each hit
		for (int i = 0; i < hits.Length; i++) 
		{
			RaycastHit hit = hits [i];

			//Is this hit closer than any we previously have found and stored
			if (hit.distance < closestColliderDistance) 
			{
				//If the hit is on the body part layer
				if (hit.transform.gameObject.layer == _bodyPartLayer) 
				{
					//And assuming it is not our own body part
					if (_stateMachine != GameManager.instance.GetAIStateMachine (hit.rigidbody.GetInstanceID ())) 
					{
						// Store the collider, distance and hit info.
						closestColliderDistance     = hit.distance;
						closestCollider             = hit.collider;
						hitInfo                     = hit;
					}
				} 
				else 
				{
					//Its not a body part so simply store this as the new closest hit we have found
					closestColliderDistance     = hit.distance;
					closestCollider             = hit.collider;
					hitInfo                     = hit;
				}
			}
		}

		//If the closest hit is the collider we are testing against, it means we have line-of-sight so return true.
		if (closestCollider && closestCollider.gameObject==other.gameObject) return true;

		//otherwise, something else is closer to us than the collider so line-of-sight is blocked
		return false;
	}
}
