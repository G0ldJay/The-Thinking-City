using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRobotState_Attack1 : AIRobotState
{
    [SerializeField] [Range(0, 10)]         float   _speed                  = 0.0f;
    [SerializeField] [Range(0.0f, 1.0f)]    float   _lookAtWeight           = 0.7f;
    [SerializeField] [Range(0.0f, 90.0f)]   float   _lookAtAngleThreshold   = 15.0f;
    [SerializeField]                        float   _slerpSpeed             = 5.0f;
    [SerializeField]                        float   _stoppingDistance       = 1.0f;

    private float _currentLookAtWeight = 0.0f;

    public override AIStateType GetStateType()
    {
        return AIStateType.Attack;
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering Attack State");

        base.OnEnterState();

        if (_robotStateMachine == null) return;

        _robotStateMachine.NavAgentControl  (true, false);
        _robotStateMachine.seeking          = 0;
        _robotStateMachine.feeding          = false;
        _robotStateMachine.attackType       = Random.Range(0, 101);
        _robotStateMachine.speed            = _speed;
        _currentLookAtWeight                = 0.0f;


    }

    public override void OnExitState()
    {
        _robotStateMachine.attackType = 0;
    }

    public override AIStateType OnUpdate()
    {
        Vector3 targetPos;
        Quaternion newRot;

        if(Vector3.Distance(_robotStateMachine.transform.position, _robotStateMachine.targetPosition) < _stoppingDistance)
        {
            _robotStateMachine.speed = 0;
        }
        else
        {
            _robotStateMachine.speed = _speed;
        }

        if(_robotStateMachine.VisualThreat.type == AITargetType.Visual_Player)
        {
            _robotStateMachine.SetTarget(_robotStateMachine.VisualThreat);

            if (!_robotStateMachine.inMeleeRange) return AIStateType.Pursuit;

            if (!_robotStateMachine.useRootRotation)
            {
                targetPos   = _robotStateMachine.targetPosition;
                targetPos.y = _robotStateMachine.transform.position.y;
                newRot      = Quaternion.LookRotation(targetPos - _robotStateMachine.transform.position);

                _robotStateMachine.transform.rotation  = Quaternion.Slerp(_robotStateMachine.transform.rotation, newRot, Time.deltaTime * _slerpSpeed);
            }

            _robotStateMachine.attackType = Random.Range(1, 101);

            return AIStateType.Attack;
        }

        if (!_robotStateMachine.useRootRotation)
        {
            targetPos   = _robotStateMachine.targetPosition;
            targetPos.y = _robotStateMachine.transform.position.y;
            newRot      = Quaternion.LookRotation(targetPos - _robotStateMachine.transform.position);

            _robotStateMachine.transform.rotation = newRot;
        }

        return AIStateType.Alerted;
    }

    //Note : Needs to be changed to work with new model
    //public override void OnAnimatorIKUpdated()
    //{
    //    if (_robotStateMachine == null) return;

    //    if(Vector3.Angle(_robotStateMachine.transform.forward, _robotStateMachine.targetPosition - _robotStateMachine.transform.position) < _lookAtAngleThreshold)
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
