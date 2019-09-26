using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

using Debug = UnityEngine.Debug;


[RequireComponent(typeof(Image))]
public class ShaderPlotter : MonoBehaviour
{
    const int MAX_BUFFER_SIZE = 1000;   // must be same as in shader!

    public enum EPlotSource
    {
        Heart,
        Arm
    }

    public Color PlotColor;
    public Color BackgroundColor;
    public EPlotSource PlotSource;

    Image Panel;
    float[] ValueBuffer = new float[MAX_BUFFER_SIZE];


    // Start is called before the first frame update
    void Start()
    {
        Panel = GetComponent<Image>();

        if (Panel.material == null)
        {
            Debug.LogError("Material not set!");
            return;
        }

        // visual sugar
        for (int i = 0; i < MAX_BUFFER_SIZE; ++i)
        {
            ValueBuffer[i] = 0.5f;
        }
        SendToShader();

        switch (PlotSource)
        {
            case EPlotSource.Heart:
                ArduinoTranslator.OnNextHeartValue += WriteNextValue;
                break;
            case EPlotSource.Arm:
                ArduinoTranslator.OnNextArmValue += WriteNextValue;
                break;
        }
    }

    void SendToShader()
    {
        Material mat = Panel.material;
        mat.SetFloatArray("_ValueBuffer", ValueBuffer);
    }

    void WriteNextValue(ushort value)
    {
        //Stopwatch w = new Stopwatch();
        //w.Start();

        int Range = Panel.material.GetInt("_Range");

        for (int i = 0; i < Range - 1; ++i)
        {
            ValueBuffer[i] = ValueBuffer[i + 1];
        }
        ValueBuffer[Range - 1] = value / 1024f;

        SendToShader();

        //w.Stop();
        //Debug.Log(w.ElapsedTicks);
    }
}
