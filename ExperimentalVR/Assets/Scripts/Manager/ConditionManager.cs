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
    [SerializeField] private Player _participiant;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion


    private string _vpNumber;
    private float _buttonWidth = 140;
    private float _buttonHeight = 25;
    private bool _measurementStarted = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Transform GetParticipantCamera()
    {
//        _participiant.get
        return _participiant.hmdTransform;
//        return _participiant;
    }

    private void OnGUI()
    {
//        GUI.Box(new Rect(ExperimentalManager.instance.GetConditionGuiPosition(), new Vector2(90,90)), "Experimental Condition");
        GUI.Box(new Rect(new Vector2(0, 0), new Vector2(160, 120)), "Experimental Condition");
        GUI.Label(new Rect(10, 40, 80, 30), new GUIContent("VP#: ", "Insert the VP Number"));
        _vpNumber = GUI.TextField(new Rect(50, 40, 80, 20), _vpNumber);

        if (!_measurementStarted && GUI.Button(new Rect(0, 60, _buttonWidth, _buttonHeight),
                new GUIContent("Start Measurement", "Start the measurement of the Experiment")))
        {
            _measurementStarted = true;
            //TODO: Implement the Start the Experimental Condition + check for VP-Number Condition    
        }

        if (_measurementStarted && GUI.Button(new Rect(0, 60, _buttonWidth, _buttonHeight),
                new GUIContent("Save", "Save the generated data from the measurement")))
        {
            ExperimentalManager.instance.SaveMeasurementData();
        }

        GUI.Label(new Rect(10, 80, 80, 80), GUI.tooltip);
    }


    public string GetVpNumber()
    {
        return _vpNumber;
    }

    public void SetVpNumber(String vpNumber)
    {
        _vpNumber = vpNumber;
    }


    public void StoreMeasuremenData()
    {
        ExperimentalManager.instance.SaveMeasurementData();
    }
}