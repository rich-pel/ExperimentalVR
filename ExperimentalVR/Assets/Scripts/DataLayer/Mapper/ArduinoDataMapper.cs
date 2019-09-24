using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace DataLayer.Mapper
{
    public abstract class ArduinoDataMapper : BaseDataMapper
    {
        protected Dictionary<string, int> _positionValueMap;

        protected const string TimeStamp = "TimeStamp";
        protected string baseElectrodeName = "BaseElectrodeName";

        protected void InitialPositionValueMap()
        {
            _positionValueMap = new Dictionary<string, int>
            {
                {vpNumber, 0},
                {TimeStamp, 1},
                {baseElectrodeName, 2}
            };
        }

        protected List<string[]> GenerateSerializableFormat(string vpNumber, List<ArduinoData> arduinoData)
        {
            List<string[]> serializableData = new List<string[]>();
            serializableData.Add(GenerateHeader());
            GenerateBody(vpNumber, arduinoData, ref serializableData);
            return serializableData;
        }


        private string[] GenerateHeader()
        {
            var header = new string[_positionValueMap.Count];
            header[_positionValueMap[vpNumber]] = vpNumber;
            header[_positionValueMap[TimeStamp]] = TimeStamp;
            header[_positionValueMap[baseElectrodeName]] = baseElectrodeName;
            return header;
        }

        protected void GenerateBody(string vpNumber, List<ArduinoData> arduinoData, ref List<string[]> serializableData)
        {
            foreach (ArduinoData data in arduinoData)
            {
                string[] singleDataLine = new String[_positionValueMap.Count];
                singleDataLine[_positionValueMap[vpNumber]] = vpNumber;
                singleDataLine[_positionValueMap[TimeStamp]] = data.timeStamp.ToString();
                singleDataLine[_positionValueMap[baseElectrodeName]] = data.electrodeValue.ToString();
                serializableData.Add(singleDataLine);
            }
        }
        


    }
}