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
        private List<EventData> _eventData;

        private String _vpNumber;
        private DateTime _startTime;
        private DateTime _endTime;


        public VPMetaData()
        {
            _vpMomentData = new List<VPMomentData>();
            _eventData = new List<EventData>();
        }

        public void StartExperimentWithVpMetaData(String vpNumber, DateTime startTime)
        {
            _vpNumber = vpNumber;
            _startTime = startTime;
//            System.DateTime;
        }

        public void EndExperimentWithVpMetaData(DateTime endTime)
        {
            _endTime = endTime;
        }
        
        public void AddMomentData(VPMomentData vpMomentDate)
        {
            _vpMomentData.Add(vpMomentDate);
        }

        public void AddEventData(EventData eventData)
        {
            _eventData.Add(eventData);
        }
    }
}