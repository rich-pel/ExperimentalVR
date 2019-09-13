using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class DataIOManager : MonoBehaviour
{
    #region Singelton

    public static DataIOManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
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
        CSVSerializer.GenerateAndSaveCSV(vpMetaDatas, ExperimentalManager.instance.GetStorePath(), "DeineMamaAufToast");
    }
}