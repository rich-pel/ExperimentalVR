using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Plotter : MonoBehaviour
{
    const int WIDTH = 128;
    const int HEIGHT = 64;
    const int BUFFER_SIZE = 256;

    public enum EPlotSource
    {
        Heart,
        Arm
    }

    public float Delta = 0.1f;
    public Color PlotColor;
    public Color BackgroundColor;
    public EPlotSource PlotSource;

    Image Panel;
    Texture2D Plot;

    Color[] Pixels;

    int WritePos = 0;
    int ReadPos = 0;
    ushort[] RingBuffer = new ushort[BUFFER_SIZE];

    // Start is called before the first frame update
    void Start()
    {
        Panel = GetComponent<Image>();

        Plot = new Texture2D(WIDTH, HEIGHT, TextureFormat.RGBA32, false, false);
        Panel.sprite = Sprite.Create(Plot, new Rect(0, 0, WIDTH, HEIGHT), Vector2.zero);

        Pixels = new Color[WIDTH * HEIGHT];
        for (int i = 0; i < Pixels.Length; ++i)
        {
            Pixels[i] = BackgroundColor;
        }

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

    void WriteNextValue(ushort value)
    {
        RingBuffer[WritePos++] = value;
        if (WritePos >= BUFFER_SIZE)
        {
            WritePos = 0;
        }
    }

    ushort GetNextValue()
    {
        ushort value = RingBuffer[ReadPos++];
        if (ReadPos >= BUFFER_SIZE)
        {
            ReadPos = 0;
        }
        return value;
    }

    int GetReadLag()
    {
        if (ReadPos < WritePos)
        {
            return WritePos - ReadPos;
        }
        else if (ReadPos > WritePos) // we're over the buffer edge
        {
            return (BUFFER_SIZE - ReadPos) + WritePos;
        }
        return 0;
    }

    int GetIndexFromPosition(int x, int y)
    {
        return y * WIDTH + x;
    }

    // Update is called once per frame
    void Update()
    {
        int lag = GetReadLag();
        int lastX = 0;
        ushort nextValue = 0;

        for (int i = 0; i < Pixels.Length; ++i)
        {
            int x = i % WIDTH;
            int y = i / WIDTH;

            if (x < WIDTH - lag)
            {
                // shift as many columns as there a new values in the buffer
                int j = GetIndexFromPosition(x + lag, y);
                Pixels[i] = Pixels[j];
            }
            else
            {
                if (x != lastX)
                {
                    nextValue = GetNextValue();
                }

                if (y / (float)HEIGHT <= nextValue / 1024f)
                {
                    Pixels[i] = PlotColor;
                }
                else
                {
                    Pixels[i] = BackgroundColor;
                }
            }

            lastX = y;
        }

        Plot.SetPixels(Pixels);
        Plot.Apply();
    }
}
