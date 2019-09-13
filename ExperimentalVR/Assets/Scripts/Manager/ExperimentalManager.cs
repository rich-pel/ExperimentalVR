using System;
using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
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
            _globalVPMetaData.AddMomentData(new VPMomentData(Time.time, ConditionManager.instance.GetParticipantCamera().position, ConditionManager.instance.GetParticipantCamera().rotation));
//            ConditionManager.instance.GetParticipant();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
//        _globalVPMetaData.AddMomentData(new VPMomentData(Time.time, ));
//        _globalVPData.Add(new VPMomentData());
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
}