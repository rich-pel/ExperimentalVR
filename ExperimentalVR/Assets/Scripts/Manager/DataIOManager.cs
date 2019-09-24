using System;
using System.Collections;
using System.Collections.Generic;
using DataLayer;
using DefaultNamespace;
using UnityEngine;

public class DataIOManager : MonoBehaviour
{
    #region Singelton

    public static DataIOManager instance;
    private DataI0Connector dataI0Connector;
    private const string FileName = "DeineMamaAufToast";

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        dataI0Connector = new DataI0Connector();
    }

    // Update is called once per frame
    void Update()
    {
    }



    public string GetDefaultPrefix()
    {
        return DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + "_" +
               DateTime.Now.Hour + "" + DateTime.Now.Minute + "" + DateTime.Now.Second;
    }

    public void SaveMeasurementData(VPMetaData vpMetaData)
    {
        //Shitty workaround 
        List<VPMetaData> vpMetaDatas = new List<VPMetaData>();
        vpMetaDatas.Add(vpMetaData);
//        CSVSerializer.GenerateAndSaveCSV(vpMetaDatas, ExperimentalManager.instance.GetStorePath(), "DeineMamaAufToast");
        dataI0Connector.GenerateAndSaveMetaDataAsCsv(vpMetaData, ExperimentalManager.instance.GetStorePath(),
            FileName);
    }
}