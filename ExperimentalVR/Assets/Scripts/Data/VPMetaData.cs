using System.Collections.Generic;

namespace DefaultNamespace
{
    //VPNumber
    //Start Time
    //End Time
    
    public class VPMetaData
    {
        private List<VPMomentData> _vpMomentData;


        public VPMetaData()
        {
            _vpMomentData = new List<VPMomentData>();
        }
        
        public void AddMomentData(VPMomentData vpMomentDate)
        {
            _vpMomentData.Add(vpMomentDate);
        }
    }
}