using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ArduinoConnect;


public enum EArduinoChannel
{
    A0, A1, A2, A3, A4, A5
}

[InitializeOnLoad]
public static class ArduinoTranslator
{
    const int ARDUINO_BUFFER_SIZE = 256;

    public static EArduinoChannel HeartChannel = EArduinoChannel.A0;
    public static EArduinoChannel ArmChannel = EArduinoChannel.A1;

    public static Action<ushort> OnNextHeartValue;
    public static Action<ushort> OnNextArmValue;


    static ArduinoTranslator()
    {
        if (!Arduino.IsRunning)
        {
            Arduino.Startup(ARDUINO_BUFFER_SIZE);
        }

        // proper lib shutdown on exit
        EditorApplication.quitting += () => { Arduino.Shutdown(); };
        EditorApplication.update += Update;
    }

    static void Update()
    {
        while (Arduino.GetNextValue((int)HeartChannel, out ushort value))
        {
            OnNextHeartValue?.Invoke(value);
        }

        while (Arduino.GetNextValue((int)ArmChannel, out ushort value))
        {
            OnNextArmValue?.Invoke(value);
        }

        if (ArduinoConnect.Logger.HasNewMessage(out string msg, out ELogType type))
        {
            switch (type)
            {
                case ELogType.Log:
                    Debug.Log(msg);
                    break;
                case ELogType.Warning:
                    Debug.LogWarning(msg);
                    break;
                case ELogType.Error:
                    Debug.LogError(msg);
                    break;
            }
        }
    }
}
