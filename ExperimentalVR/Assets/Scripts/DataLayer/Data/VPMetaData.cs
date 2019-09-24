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
        private List<VPMomentData> _vpMomentData;

        private String vpNumber;
        private DateTime startTime;
        private DateTime endTime;


        public VPMetaData()
        {
            _vpMomentData = new List<VPMomentData>();
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
//            System.DateTime;
        }

        public void EndExperimentWithVpMetaData(DateTime endTime)
        {
            this.endTime = endTime;
        }

        public void AddMomentData(VPMomentData vpMomentDate)
        {
            _vpMomentData.Add(vpMomentDate);
        }

        public List<VPMomentData> GetMomentData()
        {
            return _vpMomentData;
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
    }
}