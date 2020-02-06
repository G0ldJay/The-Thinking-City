using UnityEngine;
using System.Collections;

// ----------------------------------------------------------------------
// Class	:	AIPowerUpSensor
// Desc		:	Notifies the parent AIStateMachine of any threats that
//				enter its trigger via the AIStateMachine's OnTriggerEvent
//				method.
// ----------------------------------------------------------------------
public class AIPowerUpSensor : MonoBehaviour
{
    // Private
    private AIStateMachine _parentStateMachine = null;                               //Holds the AI State Machine

    public AIStateMachine parentStateMachine { set { _parentStateMachine = value; } }  //Sets the A.I State Machine 

    void OnTriggerEnter(Collider col) //When something enters the trigger 
    {
        if (_parentStateMachine != null) //If there is a state machine
            _parentStateMachine.PowerUp(col); //Set enum to enter as something has entered the sensor, pass in collider of what enetered 
    }
}
