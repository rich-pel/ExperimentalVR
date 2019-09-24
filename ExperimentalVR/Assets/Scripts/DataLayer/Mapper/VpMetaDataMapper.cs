using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace DataLayer.Mapper
{
    public class VpMetaDataMapper : BaseDataMapper
    {
        private readonly Dictionary<string, int> _positionValueMap;

        //MomentData
        private const string TimeStamp = "TimeStamp";
        private const string PositionDataX = "PositionData_X";
        private const string PositionDataY = "PositionData_Y";
        private const string PositionDataZ = "PositionData_Z";
        private const string RotationDataX = "RotationData_X";
        private const string RotationDataY = "RotationData_Y";
        private const string RotationDataZ = "RotationData_Z";
        private const string RotationDataW = "RotationData_W";
        private const string EventType = "EventType";

        public VpMetaDataMapper()
        {
            _positionValueMap = new Dictionary<string, int>
            {
                //MetaData
                {vpNumber, 0},
                {startTime, 1},
                {endTime, 2},

                //MomentData
                {TimeStamp, 3},
                {PositionDataX, 4},
                {PositionDataY, 5},
                {PositionDataZ, 6},
                {RotationDataX, 7},
                {RotationDataY, 8},
                {RotationDataZ, 9},
                {RotationDataW, 10},
                {EventType, 11},
            };
        }

        internal List<string[]> MapMetaDataToStringList(VPMetaData vpMetaData)
        {
            List<string[]> serializableData = new List<string[]> {GenerateHeader()};
            GenerateBody(vpMetaData, ref serializableData);
            return serializableData;
        }

        private string[] GenerateHeader()
        {
            string[] header = new string[_positionValueMap.Count];

            //MetaData
            header[_positionValueMap[vpNumber]] = vpNumber;
            header[_positionValueMap[startTime]] = startTime;
            header[_positionValueMap[endTime]] = endTime;

            //MomentData
            header[_positionValueMap[TimeStamp]] = TimeStamp;
            header[_positionValueMap[PositionDataX]] = PositionDataX;
            header[_positionValueMap[PositionDataY]] = PositionDataY;
            header[_positionValueMap[PositionDataZ]] = PositionDataZ;
            header[_positionValueMap[RotationDataX]] = RotationDataX;
            header[_positionValueMap[RotationDataY]] = RotationDataY;
            header[_positionValueMap[RotationDataZ]] = RotationDataZ;
            header[_positionValueMap[RotationDataW]] = RotationDataW;
            header[_positionValueMap[EventType]] = EventType;
            return header;
        }

        private void GenerateBody(VPMetaData vpMetaData, ref List<string[]> serializableData)
        {
            foreach (VPMomentData momentData in vpMetaData.GetMomentData())
            {
                var singleLine = new string[_positionValueMap.Count];
                //MetaData
                singleLine[_positionValueMap[vpNumber]] = vpMetaData.GetVpNumber();
                singleLine[_positionValueMap[startTime]] = vpMetaData.GetStartTime().ToString();
                singleLine[_positionValueMap[endTime]] = vpMetaData.GetEndTime().ToString();

                //MomentData
                singleLine[_positionValueMap[TimeStamp]] = momentData.TimeStamp.ToString();
                singleLine[_positionValueMap[PositionDataX]] = momentData.PositionData.x.ToString();
                singleLine[_positionValueMap[PositionDataY]] = momentData.PositionData.y.ToString();
                singleLine[_positionValueMap[PositionDataZ]] = momentData.PositionData.z.ToString();
                singleLine[_positionValueMap[RotationDataX]] = momentData.RotationData.x.ToString();
                singleLine[_positionValueMap[RotationDataY]] = momentData.RotationData.y.ToString();
                singleLine[_positionValueMap[RotationDataZ]] = momentData.RotationData.z.ToString();
                singleLine[_positionValueMap[RotationDataW]] = momentData.RotationData.w.ToString();
                singleLine[_positionValueMap[EventType]] = momentData.EventType.ToString();
                serializableData.Add(singleLine);
            }
        }

        public void GenerateDeserializedValidationData(List<string[]> csvFile,
            ref VPMetaData vPMetaData)
        {
            string[] firstDataLine = csvFile[1];
            vPMetaData =
                new VPMetaData(
                    firstDataLine[_positionValueMap[vpNumber]],
                    Convert.ToDateTime(firstDataLine[_positionValueMap[startTime]]),
                    Convert.ToDateTime(firstDataLine[_positionValueMap[endTime]])
                );
            //Skiped the first line, because this is the header!
            for (int i = 1; i < csvFile.Count; i++)
            {
                string[] singleLine = csvFile[i];
                Debug.Log(singleLine);
                foreach (string s in singleLine)
                {
                    Debug.Log("Single Element in a singleLine: " + s);
                }

                VPMomentData currentMoment = new VPMomentData(
                    float.Parse(singleLine[_positionValueMap[TimeStamp]]),
                    new Vector3(
                        float.Parse(singleLine[_positionValueMap[PositionDataX]]),
                        float.Parse(singleLine[_positionValueMap[PositionDataY]]),
                        float.Parse(singleLine[_positionValueMap[PositionDataZ]])
                    ),
                    new Quaternion(
                        float.Parse(singleLine[_positionValueMap[RotationDataX]]),
                        float.Parse(singleLine[_positionValueMap[RotationDataY]]),
                        float.Parse(singleLine[_positionValueMap[RotationDataZ]]),
                        float.Parse(singleLine[_positionValueMap[RotationDataW]])
                    ),
                    (VPEventType) Enum.Parse(typeof(VPEventType), singleLine[_positionValueMap[EventType]])
                );

                vPMetaData.AddMomentData(currentMoment);
            }
        }
    }
}