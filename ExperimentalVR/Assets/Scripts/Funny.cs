using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoConnect;

public class Funny : MonoBehaviour
{
    public Vector3 Offset;
    public float Lerp = 1f;
    public float Multiplier = 1f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = Arduino.ChannelValues[0] * Multiplier;
        transform.position = Offset + Vector3.Lerp(transform.position, pos, Time.deltaTime * Lerp);
    }
}
