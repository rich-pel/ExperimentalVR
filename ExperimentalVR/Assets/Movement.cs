using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject Punkt;
    private Vector3 cubePosition;
    //random input
    private float yaxes;
    private float xpos;
    private float ypos;
    private float i;
    private Vector3 spawnPos;
    

    void Start(){
        spawnPos = Punkt.transform.position;
   
        //Debug.Log(spawnPos.x);
        
    }

    void Update()
    {
        yaxes = (float) Random.Range(-5f, 5f);
        //An dieser stelle sollte dann die Spannung vom EMG eingespeist werden
        //yaxes = getComponent<Voltage>;
        xpos = spawnPos.x;
        ypos = yaxes + spawnPos.y;
        if (i <= 1000)
        {
            i = i + 1;
        }
        else
        {
            i = 0;
        }
        
        cubePosition = new Vector3((xpos + (i/100)), ypos, spawnPos.z);
        Punkt.transform.position = cubePosition;
     }
}
