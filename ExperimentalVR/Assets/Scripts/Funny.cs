using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoConnect;

public class Funny : MonoBehaviour
{
    public Vector3 Offset;
    public float Lerp = 1f;
    public float Multiplier = 1f;

    ushort LastHeartValue = 0;


    // Start is called before the first frame update
    void Start()
    {
        ArduinoTranslator.OnNextHeartValue += (ushort value) => { LastHeartValue = value; };
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = LastHeartValue * Multiplier;
        transform.position = Offset + Vector3.Lerp(transform.position, pos, Time.deltaTime * Lerp);
    }
}
