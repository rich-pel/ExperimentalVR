using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ConditionManager : MonoBehaviour
{
    #region Singelton

    public static ConditionManager instance;
    [SerializeField] private Player _participiant;

    private void Awake()
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

    public Transform GetParticipantCamera()
    {
//        _participiant.get
        return _participiant.hmdTransform;
//        return _participiant;
    }
}