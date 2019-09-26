using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using DefaultNamespace;
using UnityEngine;

public class ExperimentalManager : MonoBehaviour
{
    #region Singelton

    public static ExperimentalManager instance;
//    private List<VPMomentData> _globalVPData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
//            _globalVPData = new List<VPMomentData>();
        }
    }

    #endregion


    [SerializeField] private bool _isExperimentalCondition = false;
    
    private bool measurementStarted;
    private VPMetaData _globalVPMetaData;
    [SerializeField] private Vector2 _conditionGuiPosistion;
    [SerializeField] private string _storingPath = @"D:\TestVrExperiment";

    private VPEventType currentEvent = VPEventType.Nothing;

    // Start is called before the first frame update
    void Start()
    {
        ArduinoTranslator.OnNextArmValue += OnNextArmValue;
        ArduinoTranslator.OnNextHeartValue += OnNextHeartValue;
    }

    private void OnNextHeartValue(ushort electrodeValue)
    {
        _globalVPMetaData?.AddHeartValue(Time.time, electrodeValue);
    }

    private void OnNextArmValue(ushort electrodeValue)
    {
        _globalVPMetaData?.AddArmValue(Time.time, electrodeValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (measurementStarted)
        {
            AddVPMomentDataPoint();
        }
    }

    private void AddVPMomentDataPoint()
    {
        try
        {
            //TODO: find a suitableway for the VPEventType
            _globalVPMetaData.AddMomentData(new VPMomentData(Time.time, ConditionManager.instance.GetParticipantCamera().position, ConditionManager.instance.GetParticipantCamera().rotation, currentEvent));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void StartExperiment()
    {
        string vpNumber = "";
        _globalVPMetaData = new VPMetaData();
        _globalVPMetaData.StartExperimentWithVpMetaData(vpNumber, DateTime.Now);
        measurementStarted = true;
    }

    public Vector2 GetConditionGuiPosition()
    {
        return _conditionGuiPosistion;
    }

    public void SaveMeasurementData()
    {
        DataIOManager.instance.SaveMeasurementData(_globalVPMetaData);
    }

    public string GetStorePath()
    {
        return _storingPath;
    }

    private void UpdateCurrentEvent(VPEventType newEventStatus)
    {
        currentEvent = newEventStatus;
    }

    public void HammerBegin()
    {
        UpdateCurrentEvent(VPEventType.Hammer);
    }

    public void HammerEnd()
    {
        UpdateCurrentEvent(VPEventType.Nothing);
    }
}