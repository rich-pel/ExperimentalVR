using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    #region Singelton

    public static MainMenuManager instance;

    private bool _experimentalCondition = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion

    public void LoadExperimentalScene()
    {
        SceneManager.LoadScene("ExperimentSceneEditToFBX", LoadSceneMode.Single);
    }

    public void LoadCalibrationScene()
    {
        SceneManager.LoadScene("CalibrationScene", LoadSceneMode.Single);
    }

    public void LoadEvaluationScene()
    {
        SceneManager.LoadScene("EvaluationScene", LoadSceneMode.Single);
    }

    public void SetToExperimentalCondition()
    {
        _experimentalCondition = true;
    }
}