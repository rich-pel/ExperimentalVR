using System.IO;
using DataLayer.Mapper;
using DefaultNamespace;

namespace DataLayer
{
    public class DataIOConnector
    {
        private DataMapper dataMapper;
        private CsvDeSerializer csvDeSerializer;

        public DataIOConnector( )
        {
            dataMapper = new DataMapper();
csvDeSerializer = new CsvDeSerializer();
        }

        public void GenerateAndSaveMetaDataAsCsv(VPMetaData vpMetaData, string storePath, string fileName)
        {
            csvDeSerializer.WriteCSVFile(dataMapper.MapMetaDataToStringList(vpMetaData), storePath, fileName);
        }

        public void GenerateAndSaveEventDataAsCsv(EventData eventData, string storePath, string fileName)
        {
            csvDeSerializer.WriteCSVFile(dataMapper.MapEventDataToStringList(eventData), storePath, fileName);
        }

        public void GenerateAndSaveVpMomentDataAsCsv(VPMomentData vpMomentData, string storePath, string fileName)
        {
            csvDeSerializer.WriteCSVFile(dataMapper.MapMomentDataToStringList(vpMomentData), storePath, fileName);
        }
    }
}