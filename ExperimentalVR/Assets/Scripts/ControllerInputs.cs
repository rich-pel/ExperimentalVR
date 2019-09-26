using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


using Valve.VR;

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


    [SerializeField] private ParticipantController participant;
    [SerializeField] private KeyCode resetKey;

    // a reference to the action
    public SteamVR_Action_Boolean itsHammerTime;
    public SteamVR_Action_Boolean itsRestTime;
    // a reference to the hand
    public SteamVR_Input_Sources InputSource;



    // Use this for initialization
    void Start()
    {
        if (itsHammerTime != null)
        {
            itsHammerTime.AddOnStateDownListener(TriggerDown, InputSource);

        }

        if (itsRestTime != null)
        {
            itsRestTime.AddOnStateDownListener(GrapDown, InputSource);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(resetKey))
        {
            ResetPosition();
        }
    }


    void OnDisable()
    {
        if (itsHammerTime != null)
        {
            itsHammerTime.RemoveOnStateDownListener(TriggerDown, InputSource);

        }

        if (itsRestTime != null)
        {
            itsRestTime.RemoveOnStateDownListener(GrapDown, InputSource);

        }
    }

    public void GrapDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Grap is down");
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