using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ArduinoConnect;


public static class HeartBPMDetector
{
    const int BUFFER_SIZE = 1024;
    const float BPM_CALC_DELAY = 0.5f;

    public static ushort BeatsPerMinute { get; private set; } = 0;

    struct ECKVal
    {
        public ushort Value;
        public double Timestamp;
    }

    // using a queue instead of a ring buffer
    // performance is not important at this point
    static Queue<ECKVal> LastValues = new Queue<ECKVal>();
    static double lastCalc;
    static double lastValueTime;


    static HeartBPMDetector()
    {
        lastCalc = EditorApplication.timeSinceStartup;
        ArduinoTranslator.OnNextHeartValue += NextHeartValue;
        EditorApplication.update += Update;
    }

    static void NextHeartValue(ushort value)
    {
        LastValues.Enqueue(new ECKVal
        {
            Value = value,
            Timestamp = EditorApplication.timeSinceStartup
        });

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
        if (LastValues.Count == 0) return;

        double timespan = EditorApplication.timeSinceStartup - LastValues.Peek().Timestamp; // seconds
        ushort beats = 0;

        foreach (ECKVal v in LastValues)
        {
            if (v.Value > 666)
            {
                beats++;
            }
        }

        BeatsPerMinute = (ushort)Mathf.RoundToInt((float)((beats / timespan) * 60));
    }
}
