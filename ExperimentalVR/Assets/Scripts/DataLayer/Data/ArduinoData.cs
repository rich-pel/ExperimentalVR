namespace DefaultNamespace
{
    public class ArduinoData
    {
        internal readonly float timeStamp;
        internal readonly ushort electrodeValue;
        public ArduinoData(float timeStamp, ushort electrodeValue )
        {
            this.timeStamp = timeStamp;
            this.electrodeValue = electrodeValue;
        }
        
    }
}