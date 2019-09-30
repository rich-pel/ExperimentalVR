using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Image))]
public class Plotter : MonoBehaviour
{
    const int WIDTH = 256;
    const int HEIGHT = 128;
    const int BUFFER_SIZE = 256;

    public enum EPlotSource
    {
        Heart,
        Arm
    }

    [Range(1, WIDTH / 20)] public int Zoom = 1;
    [Range(1, HEIGHT / 10)] public int LineThickness = 1;
    public Color PlotColor;
    public Color BackgroundColor;
    public EPlotSource PlotSource;

    Image Panel;
    Texture2D Plot;

    Color[] Pixels;

    int WritePos = 0;
    int ReadPos = 0;
    ushort[] RingBuffer = new ushort[BUFFER_SIZE];

    int lastY = HEIGHT / 2;


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
        DrawLineThick(0, lastY, WIDTH - 1, lastY, LineThickness); // initial line, just visual sugar
        Plot.SetPixels(Pixels);
        Plot.Apply();

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

    void DrawLineThick(int x0, int y0, int x1, int y1, float wd)
    {
        int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
        int err = dx - dy, e2, x2, y2;                          /* error value e_xy */
        float ed = dx + dy == 0 ? 1 : Mathf.Sqrt((float)dx * dx + (float)dy * dy);

        for (wd = (wd + 1) / 2; ;)
        {                                   /* pixel loop */
            Pixels[GetIndexFromPosition(x0, y0)] = Color.Lerp(PlotColor, BackgroundColor, Mathf.Abs(err - dx + dy) / ed - wd + 1);
            e2 = err; x2 = x0;
            if (2 * e2 >= -dx)
            {                                           /* x step */
                for (e2 += dy, y2 = y0; e2 < ed * wd && (y1 != y2 || dx > dy); e2 += dx)
                    Pixels[GetIndexFromPosition(x0, y2 += sy)] = Color.Lerp(PlotColor, BackgroundColor, Mathf.Abs(e2) / ed - wd + 1);
                if (x0 == x1) break;
                e2 = err; err -= dy; x0 += sx;
            }
            if (2 * e2 <= dy)
            {                                            /* y step */
                for (e2 = dx - e2; e2 < ed * wd && (x1 != x2 || dx < dy); e2 += dy)
                    Pixels[GetIndexFromPosition(x2 += sx, y0)] = Color.Lerp(PlotColor, BackgroundColor, Mathf.Abs(e2) / ed - wd + 1);
                if (y0 == y1) break;
                err += dx; y0 += sy;
            }
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
        return Mathf.Clamp(y * WIDTH + x, 0, Pixels.Length - 1);
    }

    // Update is called once per frame
    void Update()
    {
        int lag = GetReadLag();
        ushort nextValue = 0;

        if (lag == 0) return;

        //Stopwatch w = new Stopwatch();
        //w.Start();

        int pixelLag = lag * Zoom;
        for (int i = 0; i < Pixels.Length; ++i)
        {
            int x = i % WIDTH;
            int y = i / WIDTH;

            if (x < WIDTH - pixelLag)
            {
                // shift everything left
                int j = GetIndexFromPosition(x + pixelLag, y);
                Pixels[i] = Pixels[j];
            }
            else
            {
                Pixels[i] = BackgroundColor;
            }
        }

        int nextX;
        for (int i = lag; i > 0 && pixelLag < WIDTH; --i)
        {
            pixelLag = i * Zoom;
            nextValue = GetNextValue();
            nextX = Mathf.Clamp((WIDTH - (i - 1)) * Zoom, 0, WIDTH - 1);
            int nextY = Mathf.Clamp((int)((nextValue / 1024f) * HEIGHT), 0, HEIGHT - 1);
            int currX = Mathf.Clamp(WIDTH - pixelLag - 1, 0, WIDTH - 1);
            DrawLineThick(currX, lastY, nextX, nextY, LineThickness);
            lastY = nextY;
        }

        Plot.SetPixels(Pixels);
        Plot.Apply();

        //w.Stop();
        //UnityEngine.Debug.Log(w.ElapsedTicks);
    }
}
