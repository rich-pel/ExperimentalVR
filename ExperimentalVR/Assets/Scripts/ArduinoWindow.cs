using UnityEngine;
using UnityEditor;
using ArduinoConnect;

public class ArduinoWindow : EditorWindow
{
    const int MENU_WIDTH = 400;

    static int SelectedCOMPort;
    static bool bEmulation;
    static string RecordingFilePath;

    static ushort LastHeartValue;
    static ushort LastArmValue;

    [MenuItem("Experiment/Arduino Window")]
    public static void Init()
    {
        // this should never fail
        ArduinoWindow window = (ArduinoWindow)GetWindow(typeof(ArduinoWindow));
        window.Show();

        ArduinoTranslator.OnNextHeartValue += (ushort value) => { LastHeartValue = value; };
        ArduinoTranslator.OnNextArmValue   += (ushort value) => { LastArmValue   = value; };
    }

    private void Update()
    {
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

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(MENU_WIDTH));
        EditorGUILayout.LabelField("COM Port (Arduino):");

        GUI.enabled = !Arduino.IsConnected && !bEmulation;
        SelectedCOMPort = EditorGUILayout.Popup(SelectedCOMPort, Arduino.AvailableDevices);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(MENU_WIDTH));
        EditorGUILayout.LabelField("Emulation:");
        GUI.enabled = !Arduino.IsConnected;
        bEmulation = EditorGUILayout.Toggle(bEmulation);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button(Arduino.IsConnected ? "Disconnect" : "Connect", GUILayout.MaxWidth(MENU_WIDTH)))
        {
            if (Arduino.IsConnected)
            {
                Arduino.Disconnect();
            }
            else if (bEmulation)
            {
                Arduino.Connect(EditorUtility.OpenFilePanel("Choose recording", Application.dataPath, "rec"));
            }
            else
            {
                Arduino.Connect(SelectedCOMPort);
            }
        }

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(MENU_WIDTH));
        EditorGUILayout.LabelField("Heart Channel:");
        ArduinoTranslator.HeartChannel = (EArduinoChannel)EditorGUILayout.EnumPopup(ArduinoTranslator.HeartChannel);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(MENU_WIDTH));
        EditorGUILayout.LabelField("Arm Channel:");
        ArduinoTranslator.ArmChannel = (EArduinoChannel)EditorGUILayout.EnumPopup(ArduinoTranslator.ArmChannel);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Heart: " + LastHeartValue, GUILayout.MaxWidth(MENU_WIDTH));
        EditorGUILayout.LabelField("Heart BPM: " + HeartBPMDetector.BeatsPerMinute, GUILayout.MaxWidth(MENU_WIDTH));
        EditorGUILayout.LabelField("Arm: " + LastArmValue, GUILayout.MaxWidth(MENU_WIDTH));

        Recorder.ERecordingState state = Recorder.RecordingState;
        GUI.enabled = state == Recorder.ERecordingState.Stopped && !bEmulation;
        if (GUILayout.Button("Choose Recording File Path...", GUILayout.MaxWidth(MENU_WIDTH)))
        {
            RecordingFilePath = EditorUtility.SaveFilePanel("Recording File Path...", Application.dataPath, "Recording.rec", "rec");
        }
        GUI.enabled = !bEmulation;
        EditorGUILayout.LabelField("Record EKG to: " + RecordingFilePath, GUILayout.MaxWidth(MENU_WIDTH));

        switch (Recorder.RecordingState)
        {
            case Recorder.ERecordingState.Stopped:
                GUI.enabled = !string.IsNullOrEmpty(RecordingFilePath) && !bEmulation;
                if (GUILayout.Button("Start Recording", GUILayout.MaxWidth(MENU_WIDTH)))
                {
                    Recorder.StartRecording(RecordingFilePath);
                }
                GUI.enabled = !bEmulation;
                break;
            case Recorder.ERecordingState.Recording:
                if (GUILayout.Button("Stop Recording", GUILayout.MaxWidth(MENU_WIDTH)))
                {
                    Recorder.StopRecording();
                }
                break;
            case Recorder.ERecordingState.Stopping:
                GUI.enabled = false;
                GUILayout.Button("Stopping...", GUILayout.MaxWidth(MENU_WIDTH));
                GUI.enabled = !bEmulation;
                break;
            default:
                // This should never happen
                Debug.LogError("Unknown Recording State: " + state);
                break;
        }

        EditorGUILayout.EndVertical();
    }
}
