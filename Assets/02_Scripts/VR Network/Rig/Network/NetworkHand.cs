using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public interface IHandRepresentation
{
    public void SetHandCommand(HandCommand command);
    public GameObject gameObject { get; }
    public void SetHandColor(Color color);
    public void SetHandMaterial(Material material);
    public void DisplayMesh(bool shouldDisplay);
    public bool IsMeshDisplayed { get; }
    public Material SharedHandMaterial { get; }

}
/**
 * 
 * Network VR user hand
 * 
 * Handle the synchronisation of the hand pose
 * Use the local HardwareRig rig hand pose when this rig is associated with the local user 
 * 
 * Position synchronization is handled in the NetworkRig
 * 
 **/

[RequireComponent(typeof(NetworkTransform))]
[OrderAfter(typeof(NetworkRig))]
public class NetworkHand : NetworkBehaviour
{
    [HideInInspector]
    public NetworkTransform networkTransform;

    //[Networked(OnChanged = nameof(OnHandCommandChange))]
    //public HandCommand HandCommand { get; set; }

    public RigPart side;
    NetworkRig rig;
    //IHandRepresentation handRepresentation;

    public bool IsLocalNetworkRig => rig.IsLocalNetworkRig;

    public HardwareHand LocalHardwareHand => IsLocalNetworkRig ? (side == RigPart.LeftController ? rig.hardwareRig.leftHand : rig.hardwareRig.rightHand) : null;


    private void Awake()
    {
        rig = GetComponentInParent<NetworkRig>();
        networkTransform = GetComponent<NetworkTransform>();
        //handRepresentation = GetComponentInChildren<IHandRepresentation>();
    }
}
