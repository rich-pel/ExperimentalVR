using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using ArduinoConnect;
/// <summary>
/// leicht abgeändertes Youtube video programm
/// </summary>
public class Chart : MonoBehaviour
{
    List<float> red;
    List<float> green;
    List<float> blue;

    public int IMG_WIDTH;
    public int IMG_HEIGHT;

    public int Hertz = 50;
    public int Voltage;

    public int pattern = 0;
    public string nameOfFile;

    public GameObject thing;
    public Texture2D texture;



    void Start()
    {
        IMG_WIDTH = texture.width;
        IMG_HEIGHT = texture.height;
        saveTexture();
        //thinkTheArt(pattern);
        //malen();

    }

    void saveTexture()
    {
        var pix = texture.GetPixels32();
    }

    void coolerStuff()
    {

        for (int i = 0; i < IMG_WIDTH; i++)
        {
            for (int j = 0; j < IMG_HEIGHT; j++)
            {
                if (IMG_HEIGHT == (int) Voltage)
                {
                    red.Add(0);
                    green.Add(0);
                    blue.Add(0);
                }

            }

            texture.Apply();

        }


        void malen()
        {
            texture = new Texture2D(IMG_WIDTH, IMG_HEIGHT, TextureFormat.ARGB32, false);
            
            var redArr = red.ToArray();
            var greenArr = green.ToArray();
            var blueArr = blue.ToArray();

            for (var i = 0; i < IMG_WIDTH; i++)
            {
                for (var j = 0; j < IMG_HEIGHT; j++)
                {
                    texture.SetPixel(i, j,
                        new Color(redArr[i * IMG_HEIGHT + j], greenArr[i * IMG_HEIGHT + j],
                            blueArr[i * IMG_HEIGHT + j]));
                }

            }

            texture.Apply();
            thing.GetComponent<Image>().sprite =
                Sprite.Create(texture, new Rect(0, 0, IMG_WIDTH, IMG_HEIGHT), new Vector2(0.5f, 0.5f));
        }

        // Update is called once per frame
        void Update()
        {
            //Voltage = Arduino[0];
            Voltage = (int) Random.Range(-5, 5);
            //thinkTheArt(pattern);
            //doTheArt();

            coolerStuff();
        }
    }
}
