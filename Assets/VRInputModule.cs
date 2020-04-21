using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRInputModule : BaseInputModule {

    public Camera _Camera;
    public SteamVR_Input_Sources _TargetSource;
    public SteamVR_Action_Boolean _ClickAction;

    private GameObject       _CurrentObject = null;
    private PointerEventData _Data          = null;

    protected override void Awake() {
        base.Awake();

        _Data = new PointerEventData(eventSystem);
    }

    public override void Process() {
        // if game in paused mode
        if (Time.timeScale == 0) {

            // reset data 
            _Data.Reset();
            _Data.position = new Vector2(_Camera.pixelWidth / 2, _Camera.pixelHeight / 2);

            // raycast
            eventSystem.RaycastAll(_Data, m_RaycastResultCache);
            _Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
            _CurrentObject = _Data.pointerCurrentRaycast.gameObject;

            // clear
            m_RaycastResultCache.Clear();

            // hover
            HandlePointerExitAndEnter(_Data, _CurrentObject);

            // press
            if (_ClickAction.GetStateDown(_TargetSource)) {
                ProcessPress(_Data);
            }

            // release
            if (_ClickAction.GetStateUp(_TargetSource)) {
                ProcessRelease(_Data);
            }
        }
    }

    public PointerEventData GetData() {
        return _Data;
    }

    private void ProcessPress(PointerEventData data) {
        // set raycast
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        // check for object hit, get the down handler, call
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(_CurrentObject, data, ExecuteEvents.pointerDownHandler);

        // if no down handler, try and get click handler
        if (newPointerPress == null) {
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(_CurrentObject);
        }

        //// if not paused, and clicked object is throwable, bring it to current hand
        //if (Time.timeScale != 0f && newPointerPress != null) { 
        //    if(newPointerPress.GetComponent<Throwable>() != null) {
        //        if (_TargetSource == SteamVR_Input_Sources.RightHand) {
        //            // start bringing the object to right hand
        //            RightHand.AttachObject(newPointerPress, GrabTypes.Grip);
        //        }
        //        else if(_TargetSource == SteamVR_Input_Sources.LeftHand) {
        //            // start bringing object to left hand
        //            LeftHand.AttachObject(newPointerPress, GrabTypes.Grip);
        //        }
        //    }
        //}

        // set data
        data.pressPosition   = data.position;
        data.pointerPress    = newPointerPress;
        data.rawPointerPress = _CurrentObject;
    }

    private void ProcessRelease(PointerEventData data) {
        // execute pointer up
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        // check for click handler
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(_CurrentObject);

        // check if actual
        if(data.pointerPress == pointerUpHandler) {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        // clear selected gameobject
        eventSystem.SetSelectedGameObject(null);

        // reset data
        data.pressPosition   = Vector2.zero;
        data.pointerPress    = null;
        data.rawPointerPress = null;

    }
}
