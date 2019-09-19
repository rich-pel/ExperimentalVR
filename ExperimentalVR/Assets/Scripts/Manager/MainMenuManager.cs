using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
//    public bool optional;

//    public bool testButton;

    #region Singelton

    public static MainMenuManager instance;

    private bool _experimentalCondition = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

//    private void OnGUI()
//    {
////         optional =  GUI.Toggle(new Rect(10, 70, 50, 30), optional,"Start Calibration");
////         GUI.enabled = optional;
////         testButton = GUI.Button(new Rect(50, 100, 50, 30), "Start Calibration");
//
//
////        GUI.Box(new Rect(10, 10, 140, 140), "Loader Menu");
//////        GUILayout.Box("Loader Menu");
////        if (GUI.Button(new Rect(20, 30, 120, 30), new GUIContent("Calibration", "Start the Calibration")))
////            LoadCalibrationScene();
//////        GUI.Label (new Rect (150,30,140,20), GUI.tooltip);
////
////
////        if (GUI.Button(new Rect(20, 60, 120, 30), new GUIContent("Experiment", "Start the Experiment")))
////            LoadExperimentalScene();
//////        GUI.Label (new Rect (150,60,140,20), GUI.tooltip);
////
////        if (GUI.Button(new Rect(20, 90, 120, 30), new GUIContent("Evaluation", "Start the Evaluation")))
////            LoadEvaluationScene();
//
//
////GUILayout.Label(GUI.tooltip);        
////        GUI.Label(new Rect(20, 180, 140, 20), GUI.tooltip);
//
//
////        GUI.enabled = true;
//    }


    //TODO: Add a tooltip maybe
    //TODO: Maybe just a single GUIController class?


    public void LoadExperimentalScene()
    {
        Debug.Log("Clicked the button with Experiment");
        SceneManager.LoadScene("ExperimentScene", LoadSceneMode.Single);
    }

    public void LoadCalibrationScene()
    {
        Debug.Log("Clicked the button with Calibration");
        SceneManager.LoadScene("CalibrationScene", LoadSceneMode.Additive);
    }

    public void LoadEvaluationScene()
    {
        Debug.Log("Clicked the button with Evaluation");
    }


    public void SetToExperimentalCondition()
    {
        _experimentalCondition = true;
    }
}