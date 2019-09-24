using System.Collections.Generic;
using DefaultNamespace;

namespace DataLayer.Mapper
{
    public class DataMapper
    {
        private readonly VpMetaDataMapper vpMetaDataMapper;
        private readonly ArduinoArmDataMapper armDataMapper ;
        private readonly ArduinoHeartDataMapper heartDataMapper;
        

        public DataMapper()
        {
            vpMetaDataMapper = new VpMetaDataMapper();
            armDataMapper = new ArduinoArmDataMapper();
            heartDataMapper = new ArduinoHeartDataMapper();
        }

        public List<string[]> MapMetaDataToStringList(VPMetaData vpMetaData)
        {
            return vpMetaDataMapper.MapMetaDataToStringList(vpMetaData);
        }


        public List<string[]> MapArduinoArmDataToStringList(VPMetaData vpMetaData)
        {
            return armDataMapper.MapArmDataToStringList(vpMetaData);
        }

        public List<string[]> MapArduinoHeartDataToStringList(VPMetaData vpMetaData)
        {
            return heartDataMapper.MapHearDataToStringList(vpMetaData);

        }
    }
}