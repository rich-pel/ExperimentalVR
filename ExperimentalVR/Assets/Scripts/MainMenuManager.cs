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

    private void OnGUI()
    {
//         optional =  GUI.Toggle(new Rect(10, 70, 50, 30), optional,"Start Calibration");
//         GUI.enabled = optional;
//         testButton = GUI.Button(new Rect(50, 100, 50, 30), "Start Calibration");

        if (GUI.Button(new Rect(80, 130, 50, 30), "Start Calibration"))
            LoadCalibrationScene();
        if (GUI.Button(new Rect(110, 170, 50, 30), "Start Experiment"))
            LoadExperimentalScene();
        if (GUI.Button(new Rect(140, 200, 50, 30), "Start Evaluation"))
            LoadEvaluationScene();

//        GUI.enabled = true;
    }


    private void LoadExperimentalScene()
    {
        Debug.Log("Clicked the button with Experiment");
        SceneManager.LoadScene("ExperimentScene", LoadSceneMode.Single);
    }

    private void LoadCalibrationScene()
    {
        Debug.Log("Clicked the button with Calibration");
        SceneManager.LoadScene("CalibrationScene", LoadSceneMode.Additive);
    }
    private void LoadEvaluationScene()
    {
        Debug.Log("Clicked the button with Evaluation");
    }
}