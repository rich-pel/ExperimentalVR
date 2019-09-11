using UnityEngine;
using UnityEditor;
using ArduinoConnect;

public class ArduinoWindow : EditorWindow
{
    static int SelectedCOMPort = 0;

    [MenuItem("Experiment/Arduino Window")]
    public static void Init()
    {
        // this should never fail
        ArduinoWindow window = (ArduinoWindow)GetWindow(typeof(ArduinoWindow));
        window.Show();

        // proper lib shutdown on exit
        EditorApplication.quitting += () => { Arduino.Shutdown(); };
    }

    private void Update()
    {
        if (!Arduino.IsRunning)
        {
            Arduino.Startup();
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

        Repaint();
    }

    void OnGUI()
    {
        if (!Arduino.IsRunning)
        {
            EditorGUILayout.LabelField("ArduinoConnect.dll is not startup yet!");
            return;
        }

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("COM Port (Arduino):");

        GUI.enabled = !Arduino.IsConnected;
        SelectedCOMPort = EditorGUILayout.Popup(SelectedCOMPort, Arduino.AvailableDevices);
        GUI.enabled = true;

        if (GUILayout.Button(Arduino.IsConnected ? "Disconnect" : "Connect"))
        {
            if (Arduino.IsConnected)
            {
                Arduino.Disconnect();
            }
            else
            {
                Arduino.Connect(SelectedCOMPort);
            }
        }

        EditorGUILayout.LabelField("Channel A0: " + Arduino.ChannelValues[0]);
        EditorGUILayout.LabelField("Channel A1: " + Arduino.ChannelValues[1]);

        EditorGUILayout.EndVertical();
    }
}
