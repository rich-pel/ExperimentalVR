using System.Collections.Generic;
using DefaultNamespace;

namespace DataLayer.Mapper
{
    public class DataMapper
    {
        private readonly VpMetaDataMapper vpMetaDataMapper;
        private readonly VpMomentDataMapper vpMomentDataMapper;

        public DataMapper()
        {
            vpMetaDataMapper = new VpMetaDataMapper();
            vpMomentDataMapper = new VpMomentDataMapper();
        }

        public List<string[]> MapMetaDataToStringList(VPMetaData vpMetaData)
        {
            return vpMetaDataMapper.MapMetaDataToStringList(vpMetaData);
        }


        public List<string[]> MapMomentDataToStringList(VPMomentData vpMomentData)
        {
            return vpMomentDataMapper.MapMomentDataToStringList(vpMomentData);
        }
    }
}