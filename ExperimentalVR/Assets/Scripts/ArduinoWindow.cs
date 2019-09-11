using UnityEngine;
using UnityEditor;
using ArduinoConnect;

public class ArduinoWindow : EditorWindow
{
    static int SelectedCOMPort = 0;
    static string RecordingFilePath;

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

        Recorder.ERecordingState state = Recorder.RecordingState;
        GUI.enabled = state == Recorder.ERecordingState.Stopped;
        if (GUILayout.Button("Choose Recording File Path..."))
        {
            RecordingFilePath = EditorUtility.SaveFilePanel("Recording File Path...", Application.dataPath, "Recording.ekg", "ekg");
        }
        GUI.enabled = true;
        EditorGUILayout.LabelField("Record EKG to: " + RecordingFilePath);

        switch (Recorder.RecordingState)
        {
            case Recorder.ERecordingState.Stopped:
                GUI.enabled = !string.IsNullOrEmpty(RecordingFilePath);
                if (GUILayout.Button("Start Recording"))
                {
                    Recorder.StartRecording(RecordingFilePath);
                }
                GUI.enabled = true;
                break;
            case Recorder.ERecordingState.Recording:
                if (GUILayout.Button("Stop Recording"))
                {
                    Recorder.StopRecording();
                }
                break;
            case Recorder.ERecordingState.Stopping:
                GUI.enabled = false;
                GUILayout.Button("Stopping...");
                GUI.enabled = true;
                break;
            default:
                // This should never happen
                Debug.LogError("Unknown Recording State: " + state);
                break;
        }

        EditorGUILayout.EndVertical();
    }
}
