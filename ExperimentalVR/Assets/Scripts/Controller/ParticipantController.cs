using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ParticipantController : MonoBehaviour
{
    [SerializeField] private Player player;
    
    
    [SerializeField] private Transform VRRoot;
    [SerializeField] private Transform VRHead;
    [SerializeField] private Transform Destination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetHmdTransform()
    {
        return player.hmdTransform;
    }
    
    public void ResetPosition()
    {
        VRRoot.position = Destination.position - (VRHead.position - VRRoot.position);
    }

}
