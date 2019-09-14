using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ArduinoConnect;
                /// <summary>
                /// Programm von einem Youtube video
                /// </summary>
public class test : MonoBehaviour
{
   List<float> red;
   List<float> green;
   List<float> blue;

   public int IMG_WIDTH = 1024;
   public int IMG_HEIGHT = 768;
   
   public int Hertz = 50;
   public float Voltage;
   
   public int pattern = 0;
   public string nameOfFile;

   public GameObject thing;
   public Texture2D texture;
   
   void Start()
   {
       thinkTheArt(pattern);
       malen();

   }

   void thinkTheArt(int type)
   {
       red = new List<float>();
       green = new List<float>();
       blue = new List<float>();

       switch (type)
       {
           case (0):
           default:
               for (int i = 0; i < IMG_WIDTH; i++)
               {
                   for (int j = 0; j < IMG_HEIGHT; j++)
                   {
                       red.Add(Random.Range(0.0f, 1.0f));
                       green.Add(Random.Range(0.0f, 1.0f));
                       blue.Add(Random.Range(0.0f, 1.0f));
                   }

               }

               break;
           case (1):
               for (int i = 0; i < IMG_WIDTH; i++)
               {
                   for (int j = 0; j < IMG_HEIGHT; j++)
                   {
                       red.Add((float) j / IMG_HEIGHT);
                       green.Add(IMG_HEIGHT/((float) j));
                       blue.Add(0);

                   }

                   

               }

               break;
           case (2):
               

               break;
               
       }
   }

   void malen()
       {
           texture = new Texture2D(IMG_WIDTH,IMG_HEIGHT,TextureFormat.ARGB32,false);
           float[] redArr = red.ToArray();
           float[] greenArr = green.ToArray();
           float[] blueArr = blue.ToArray();

           for (int i = 0; i < IMG_WIDTH; i++)
           {
               for (int j = 0; j < IMG_HEIGHT; j++)
               {
                   texture.SetPixel(i,j,new Color(redArr[i*IMG_HEIGHT+j],greenArr [i*IMG_HEIGHT+j],blueArr[i*IMG_HEIGHT+j]));
               }
               
           }
           texture.Apply();
           thing.GetComponent<Image>().sprite = Sprite.Create(texture,new Rect(0,0,IMG_WIDTH,IMG_HEIGHT),new Vector2(0.5f,0.5f) );
       }
   
    // Update is called once per frame
    void Update()
    {
        //Voltage = Arduino[0];
        Voltage = (float) Random.Range(-5f, 5f);
        //thinkTheArt(pattern);
        //doTheArt();
    }
}
