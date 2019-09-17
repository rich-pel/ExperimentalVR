using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ArduinoConnect;


public static class HeartBPMDetector
{
    const int BUFFER_SIZE = 1024;
    const float BPM_CALC_DELAY = 1f;

    public static ushort BeatsPerMinute { get; private set; } = 0;

    // using a queue instead of a ring buffer
    // performance is not important at this point
    static Queue<ushort> LastValues = new Queue<ushort>();
    static double lastCalc;

    static HeartBPMDetector()
    {
        ArduinoTranslator.OnNextHeartValue += NextHeartValue;
        EditorApplication.update += Update;
    }

    static void NextHeartValue(ushort value)
    {
        LastValues.Enqueue(value);
        while (LastValues.Count >= BUFFER_SIZE)
        {
            LastValues.Dequeue();
        }
    }

    static void Update()
    {
        if (EditorApplication.timeSinceStartup - lastCalc >= BPM_CALC_DELAY)
        {
            CalcBPM();
            lastCalc = EditorApplication.timeSinceStartup;
        }
    }

    static void CalcBPM()
    {
        //Debug.Log("Calculating Heart BPM...");
    }
}
