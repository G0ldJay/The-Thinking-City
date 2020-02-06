using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicEnabler : AIStateMachineLink
{
    public bool OnEnter = false;
    public bool OnExit  = false;

    // ------------------------------------------------------------------
    // Name	:	OnStateEnter
    // Desc	:	Called prior to the first frame the
    //			animation assigned to this state
    // ------------------------------------------------------------------
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_stateMachine) _stateMachine.cinematicEnabled = OnEnter;
    }

    // ------------------------------------------------------------------
    // Name	:	OnStateExit
    // Desc	:	Called by the last frame of the animation prior
    //			to leaving the state
    // ------------------------------------------------------------------
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_stateMachine) _stateMachine.cinematicEnabled = OnExit;
    }
}
