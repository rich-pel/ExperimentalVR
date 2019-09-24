using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    //VPNumber
    //Start Time
    //End Time

    public class VPMetaData
    {
        private String vpNumber;
        private DateTime startTime;
        private DateTime endTime;

        private List<VPMomentData> vpMomentData;
        private List<ArduinoData> heartData;
        private List<ArduinoData> armData;


        public VPMetaData()
        {
            vpMomentData = new List<VPMomentData>();
            heartData = new List<ArduinoData>();
            armData = new List<ArduinoData>();
        }

        public VPMetaData(string vpNumber, DateTime startTime, DateTime endTime)
        {
            this.vpNumber = vpNumber;
            this.startTime = startTime;
            this.endTime = endTime;
        }

        public void StartExperimentWithVpMetaData(String vpNumber, DateTime startTime)
        {
            this.vpNumber = vpNumber;
            this.startTime = startTime;
        }

        public void EndExperimentWithVpMetaData(DateTime endTime)
        {
            this.endTime = endTime;
        }

        public void AddMomentData(VPMomentData vpMomentDate)
        {
            vpMomentData.Add(vpMomentDate);
        }

        public List<VPMomentData> GetMomentData()
        {
            return vpMomentData;
        }

        public string GetVpNumber()
        {
            return vpNumber;
        }

        public DateTime GetStartTime()
        {
            return startTime;
        }

        public DateTime GetEndTime()
        {
            return endTime;
        }

        public void AddHeartValue(float time, ushort electrodeValue)
        {
            AddHeartValue(new ArduinoData(time, electrodeValue));
        }

        public void AddHeartValue(ArduinoData data)
        {
            heartData.Add(data);
        }

        public void AddArmValue(ArduinoData data)
        {
            armData.Add(data);
        }

        public void AddArmValue(float time, ushort electrodeValue)
        {
            AddArmValue(new ArduinoData(time, electrodeValue));
        }

        public List<ArduinoData> GetHeartData()
        {
            return heartData;
        }

        public List<ArduinoData> GetArmData()
        {
            return armData;
        }
    }
}