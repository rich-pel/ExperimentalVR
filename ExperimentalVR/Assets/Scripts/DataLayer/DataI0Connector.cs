using System.IO;
using DataLayer.Mapper;
using DefaultNamespace;

namespace DataLayer
{
    public class DataI0Connector
    {
        private DataMapper dataMapper;
        private CsvDeSerializer csvDeSerializer;

        public DataI0Connector( )
        {
            dataMapper = new DataMapper();
            csvDeSerializer = new CsvDeSerializer();
        }

        public void GenerateAndSaveMetaDataAsCsv(VPMetaData vpMetaData, string storePath, string fileName)
        {
            csvDeSerializer.WriteCSVFile(dataMapper.MapMetaDataToStringList(vpMetaData), storePath, fileName);
        }
        
        public void GenerateAndSaveArduinoDataAsCsv(VPMetaData vpMetaData, string storePath, string fileName)
        {
            csvDeSerializer.WriteCSVFile(dataMapper.MapArduinoArmDataToStringList(vpMetaData), storePath, fileName);
            csvDeSerializer.WriteCSVFile(dataMapper.MapArduinoHeartDataToStringList(vpMetaData), storePath, fileName);
        }

    }
}