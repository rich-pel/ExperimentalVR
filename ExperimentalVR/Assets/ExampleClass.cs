using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ArduinoConnect;
using Random = UnityEngine.Random;
/// <summary>
/// bisher mein bestes, aber funzt noch nicht ganz
/// </summary>
public class Example : MonoBehaviour
{
    //get image
    //get texture
    private Queue myQueuea = new Queue();
    private Queue myQueuex = new Queue();
    private Queue myQueuey = new Queue();
    public int hertz = 50;
    public float voltage;
    public GameObject thing;  
    public Texture2D texture;
    void Start()
    {
        //save texture
        
    }

    void Update()
    {
        //get Voltage
        //Voltage = Arduino[0];
        voltage = Random.Range(-1f,1f);
        //get time
        int x = 0;
        int zz = 0;
        //use time and voltage to find relevant pixel
        int y = Mathf.RoundToInt(voltage * 128);
        Color a = texture.GetPixel(x, y);
        myQueuea.Enqueue(a);
        myQueuex.Enqueue(x);
        myQueuey.Enqueue(y);
        //save it to queue 
        //change it to black SetPixel
        
        x++;
        if (x == texture.width)
        {
            x = 0;
            zz++;
        }

        if (zz >= 1)
        {
            var x1 = myQueuex.Dequeue();
            var a1 = myQueuea.Dequeue();
            var y1 = myQueuey.Dequeue();
//            texture.SetPixel(x1,y1,a1);
        }
        //poplast element of queue
        //texture.Apply();
    }
}