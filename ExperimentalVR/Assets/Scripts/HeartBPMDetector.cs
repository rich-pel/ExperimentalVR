using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ArduinoConnect;

using Debug = UnityEngine.Debug;

public static class HeartBPMDetector
{
    const int BUFFER_SIZE = 1024;
    const int BUFFER_SIZE_MIN = 128;
    const float BPM_CALC_DELAY = 0.5f;

    public static float PercentDiff = 0.16f;
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
        if (!Arduino.IsConnected) return;

        if (EditorApplication.timeSinceStartup - lastCalc >= BPM_CALC_DELAY)
        {
            CalcBPM();
            lastCalc = EditorApplication.timeSinceStartup;
        }
        //Debug.Log("UPDATE!!!");
    }

    static Queue<ECKVal> tmp = new Queue<ECKVal>();
    const int TAIL_SIZE = 8;
    const int HEAD_SIZE = 2;

    static void CalcBPM()
    {
        if (LastValues.Count < BUFFER_SIZE_MIN) return;

        Stopwatch w = new Stopwatch();
        w.Start();

        ushort beats = 0;
        ECKVal[] values = LastValues.ToArray();

        for (int i = 0; i < values.Length-2; ++i)
        {
            tmp.Enqueue(values[i]);
            if (tmp.Count < TAIL_SIZE + HEAD_SIZE) continue;

            ECKVal[] tmpArr = tmp.ToArray();

            float avgTail = 0;
            for (int j = 0; j < TAIL_SIZE; ++j)
            {
                avgTail += tmpArr[j].Value;
            }
            avgTail /= TAIL_SIZE;

            float avgHead = 0;
            for (int j = TAIL_SIZE; j < TAIL_SIZE + HEAD_SIZE; ++j)
            {
                avgHead += tmpArr[j].Value;
            }
            avgHead /= HEAD_SIZE;

            float diff = (avgHead - avgTail) / 1024f; // only count high spikes
            if (diff > PercentDiff)
            {
                ++beats;
            }

            tmp.Dequeue();
        }

        ref ECKVal first = ref values[0];
        ref ECKVal last = ref values[values.Length - 1];

        double timespan = (last.Timestamp - first.Timestamp);
        BeatsPerMinute = (ushort)Mathf.RoundToInt((float)((beats / timespan) * 60));

        w.Stop();
        Debug.Log("Calculated BPM: " + BeatsPerMinute + " with " + values.Length + " samples in " + w.ElapsedTicks + " Ticks");
    }
}
