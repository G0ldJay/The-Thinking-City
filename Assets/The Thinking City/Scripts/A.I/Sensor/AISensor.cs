using UnityEngine;
using System.Collections;

// ----------------------------------------------------------------------
// Class	:	AISensor
// Desc		:	Notifies the parent AIStateMachine of any threats that
//				enter its trigger via the AIStateMachine's OnTriggerEvent
//				method.
// ----------------------------------------------------------------------
public class AISensor : MonoBehaviour 
{
	// Private
	private AIStateMachine	    _parentStateMachine	=	null;                               //Holds the AI State Machine

	public AIStateMachine       parentStateMachine  { set{ _parentStateMachine = value; }}  //Sets the A.I State Machine 

	void OnTriggerEnter( Collider col ) //When something enters the trigger 
	{
		if (_parentStateMachine!=null) //If there is a state machine
			_parentStateMachine.OnTriggerEvent ( AITriggerEventType.Enter,col ); //Set enum to enter as something has entered the sensor, pass in collider of what enetered 
	}

	void OnTriggerStay( Collider col ) //If object remains in the sensors radius 
	{
		if (_parentStateMachine!=null) //If there is a state machine
            _parentStateMachine.OnTriggerEvent ( AITriggerEventType.Stay, col ); //Set enum to stay as it remians, pass in collider of object 
	}

	void OnTriggerExit( Collider col ) //If object leaves sensor radius 
	{
		if (_parentStateMachine!=null) //If there is a state machine
            _parentStateMachine.OnTriggerEvent ( AITriggerEventType.Exit,  col ); //Set enum to exit as its left the sensor radius, pass in collider of object 
    }

}
