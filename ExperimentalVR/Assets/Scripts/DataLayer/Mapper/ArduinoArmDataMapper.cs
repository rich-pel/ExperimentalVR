using System;
using System.Collections.Generic;
using DefaultNamespace;

namespace DataLayer.Mapper
{
    public class ArduinoArmDataMapper:ArduinoDataMapper
    {
        private const string ElectrodeName = "ArmValue";

        public ArduinoArmDataMapper()
        {
            base.baseElectrodeName = ElectrodeName;
            base.InitialPositionValueMap();
        }
        

        public List<string[]> MapArmDataToStringList(VPMetaData vpMetaData)
        {
            return base.GenerateSerializableFormat(vpMetaData.GetVpNumber(), vpMetaData.GetArmData());
        }
        
        protected void GenerateDeserializedValidationData(List<string[]> csvFile, ref VPMetaData vPMetaData)
        {
            //Skiped the first line, because this is the header!
            for (int i = 1; i < csvFile.Count; i++)
            {
                string[] singleLine = csvFile[i];
                ArduinoData currentMoment = new ArduinoData(
                    float.Parse(singleLine[_positionValueMap[TimeStamp]]),
                    ushort.Parse(singleLine[_positionValueMap[baseElectrodeName]])
                );
                vPMetaData.AddArmValue(currentMoment);
            }
        }
    }
}