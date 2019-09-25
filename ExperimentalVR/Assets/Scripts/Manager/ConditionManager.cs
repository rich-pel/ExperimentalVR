using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[ExecuteInEditMode]
public class ConditionManager : MonoBehaviour
{
    #region Singelton

    public static ConditionManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion


    [SerializeField] private ParticipantController participant;
    private string vpNumber;
    private float buttonWidth = 140;
    private float buttonHeight = 25;
    private bool measurementStarted = false;
    [SerializeField] private KeyCode resetKey;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(resetKey))
        {
            ResetPosition();
        }
    }

    public Transform GetParticipantCamera()
    {
//        _participiant.get
        return participant.GetHmdTransform();
//        return _participiant;
    }

    private void ResetPosition()
    {
        participant.ResetPosition();
    }

//    private void OnGUI()
//    {
////        GUI.Box(new Rect(ExperimentalManager.instance.GetConditionGuiPosition(), new Vector2(90,90)), "Experimental Condition");
//        GUI.Box(new Rect(new Vector2(0, 0), new Vector2(160, 120)), "Experimental Condition");
//        GUI.Label(new Rect(10, 40, 80, 30), new GUIContent("VP#: ", "Insert the VP Number"));
//        _vpNumber = GUI.TextField(new Rect(50, 40, 80, 20), _vpNumber);
//
//        if (!_measurementStarted && GUI.Button(new Rect(0, 60, _buttonWidth, _buttonHeight),
//                new GUIContent("Start Measurement", "Start the measurement of the Experiment")))
//        {
//            _measurementStarted = true;
//            //TODO: Implement the Start the Experimental Condition + check for VP-Number Condition    
//        }
//
//        if (_measurementStarted && GUI.Button(new Rect(0, 60, _buttonWidth, _buttonHeight),
//                new GUIContent("Save", "Save the generated data from the measurement")))
//        {
//            ExperimentalManager.instance.SaveMeasurementData();
//        }
//
//        GUI.Label(new Rect(10, 80, 80, 80), GUI.tooltip);
//    }


    public string GetVpNumber()
    {
        return vpNumber;
    }

    public void SetVpNumber(String vpNumber)
    {
        this.vpNumber = vpNumber;
    }


    public void StoreMeasuremenData()
    {
        ExperimentalManager.instance.SaveMeasurementData();
    }
}