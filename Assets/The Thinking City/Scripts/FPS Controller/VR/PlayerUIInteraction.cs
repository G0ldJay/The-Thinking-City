using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerUIInteraction : MonoBehaviour
{
    [SerializeField] private InterfaceAnimManager _holographicUI = null;

    public SteamVR_Action_Boolean HoloUIOnOff;
    public SteamVR_Input_Sources _leftHand;

    private void Start()
    {
        HoloUIOnOff.AddOnStateDownListener(TriggerDown, _leftHand);
        HoloUIOnOff.AddOnStateUpListener(TriggerUp, _leftHand);
        //if(_holographicUI == null)
        //{
        //    Debug.Log("UI is NULL");
        //}
        //_holographicUI.gameObject.SetActive(_active);
    }

    private void Update()
    {

    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger down");
        _holographicUI.gameObject.SetActive(true);
        _holographicUI.startAppear();
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger up");
        _holographicUI.startDisappear();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Right Hand"))
    //    {
    //        _active = true;
    //        DisplayHoloUI(_active);
    //    }
    //}

    //private void DisplayHoloUI(bool active)
    //{
    //    if (active)
    //    {
    //        _holographicUI.gameObject.SetActive(true);
    //        _holographicUI.startAppear();
    //    }
    //    else
    //    {
    //        _holographicUI.startDisappear();
    //    }
    //}
}
