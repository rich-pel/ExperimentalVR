using UnityEngine;

public class EventData
{

    private VPEventType _eventType;
    private Time _timeStamp;

    public EventData(VPEventType eventType, Time timeStamp)
    {
        _eventType = eventType;
        _timeStamp = timeStamp;
    }
}