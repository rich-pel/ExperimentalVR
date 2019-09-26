using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Valve.VR;

// https://docs.unity3d.com/Manual/OpenVRControllers.html 

// https://medium.com/@sarthakghosh/a-complete-guide-to-the-steamvr-2-0-input-system-in-unity-380e3b1b3311

[ExecuteInEditMode]
public class ControllerInputs : MonoBehaviour
{
    #region Singelton

    public static ControllerInputs instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion


    string[] _joystickNames;


    // a reference to the action
    public SteamVR_Action_Boolean itsHammerTime;
    // a reference to the hand
    public SteamVR_Input_Sources InputSource;


    [SerializeField] private ParticipantController participant;

    // Use this for initialization
    void Start()
    {
        if (itsHammerTime != null)
        {
            itsHammerTime.AddOnStateDownListener(TriggerDown, InputSource);
            itsHammerTime.AddOnStateUpListener(TriggerDown, InputSource);
            itsHammerTime.AddOnChangeListener(TriggerDown, InputSource);
        }
    }

    private void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
    {
        Debug.Log("DDDDDDDDDDDD");
    }

    void OnDisable()
    {
        if (itsHammerTime != null)
        {
            itsHammerTime.RemoveOnStateDownListener(TriggerDown, InputSource);



        }
    }



    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newValue)
    {
        Debug.Log("Trigger is up");

        // start Animation

    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
        // start Animation
    }


    private void ResetPosition()
    {
        participant.ResetPosition();
    }

}