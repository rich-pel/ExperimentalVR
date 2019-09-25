using System.Collections.Generic;
using DefaultNamespace;

namespace DataLayer.Mapper
{
    public class ArduinoHeartDataMapper : ArduinoDataMapper
    {
        private const string ElectrodeName = "HeartValue";

        public ArduinoHeartDataMapper()
        {
            base.baseElectrodeName = ElectrodeName;
            base.InitialPositionValueMap();
        }

        public List<string[]> MapHearDataToStringList(VPMetaData vpMetaData)
        {
            return base.GenerateSerializableFormat(vpMetaData.GetVpNumber(), vpMetaData.GetHeartData());
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
                vPMetaData.AddHeartValue(currentMoment);
            }
        }
    }
}