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
        _globalVPMetaData.AddMomentData(new VPMomentData());
//        _globalVPData.Add(new VPMomentData());
    }

    private void StartExperiment()
    {
        _globalVPMetaData = new VPMetaData();
        measurementStarted = true;
    }
}