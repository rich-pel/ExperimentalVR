using UnityEngine;

public class VPMomentData
{
    //Headtracking Data
    //Timestamp

    private float timeStamp;
    private Vector3 positionData;
    private Quaternion rotationData;
    private VPEventType eventType;

    public VPMomentData(float timeStamp, Vector3 positionData, Quaternion rotationData, VPEventType eventType)
    {
        this.timeStamp = timeStamp;
        this.positionData = positionData;
        this.rotationData = rotationData;
        this.eventType = eventType;
    }

    public float TimeStamp
    {
        get => timeStamp;
        set => timeStamp = value;
    }

    public Vector3 PositionData
    {
        get => positionData;
        set => positionData = value;
    }

    public Quaternion RotationData
    {
        get => rotationData;
        set => rotationData = value;
    }

    public VPEventType EventType
    {
        get => eventType;
        set => eventType = value;
    }
}