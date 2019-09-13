using UnityEngine;

public class VPMomentData
{
//Headtracking Data
//Timestamp

    private float _timeStamp;
    private Vector3 _positionData;
    private Quaternion _rotationData;

    public VPMomentData(float timeStamp, Vector3 positionData, Quaternion rotationData)
    {
        _timeStamp = timeStamp;
        _positionData = positionData;
        _rotationData = rotationData;
    }
}