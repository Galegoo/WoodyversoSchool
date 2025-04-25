using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion;
// Structure representing the inputs driving a hand pose 
[System.Serializable]
public struct HandCommand : INetworkStruct
{
    public float thumbTouchedCommand;
    public float indexTouchedCommand;
    public float gripCommand;
    public float triggerCommand;
    // Optionnal commands
    public int poseCommand;
    public float pinchCommand;// Can be computed from triggerCommand by default
}

/**
 * 
 * Hand class for the hardware rig.
 * Handles collecting the input for the hand pose, and the hand interactions
 * 
 **/

public class HardwareHand : MonoBehaviour
{
   
    public RigPart side;
    public HandCommand handCommand;
    public bool isGrabbing = false;
    public Grabber grabber;

    [Header("Hand interaction input")]
    public XRController handController;
    public float grabThreshold = 0.5f;
    //False for Desktop mode, true for VR mode: when the hand grab is triggered by other scripts (MouseTeleport in desktop mode), we do not want to update the isGrabbing. It should only be done in VR mode
    public bool updateGrabWithAction = true;
    public IHandRepresentation localHandRepresentation;


    private void Awake()
    {
        grabber = GetComponentInChildren<Grabber>();
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        // update hand pose
        //handCommand.thumbTouchedCommand = thumbAction.action.ReadValue<float>();
        //handCommand.indexTouchedCommand = indexAction.action.ReadValue<float>();
        //handCommand.gripCommand = gripAction.action.ReadValue<float>();
        //handCommand.triggerCommand = triggerAction.action.ReadValue<float>();
        //handCommand.poseCommand = handPose;
        //handCommand.pinchCommand = 0;

        // update hand interaction
        //if (updateGrabWithAction) isGrabbing = handController.selectUsage.ReadValue<float>() > grabThreshold;
    }


    public void Grabbed(SelectEnterEventArgs arg)
    {
        if (updateGrabWithAction)
            isGrabbing = true;
        Debug.Log("Grabbed an Object");
    }

    public void Dropped(SelectExitEventArgs arg)
    {
        if (updateGrabWithAction)
            isGrabbing = false;
        Debug.Log("Dropped an Object");

    }
}
