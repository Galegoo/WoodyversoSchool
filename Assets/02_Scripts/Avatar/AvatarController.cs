using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MapTransforms
{
    public Transform vrTargets;
    public Transform ikTarget;

    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void VRMapping()
    {
        ikTarget.position = vrTargets.TransformPoint(trackingPositionOffset); //CAMERA AND CONTROLLERS IN LOCAL RIG
        ikTarget.rotation = vrTargets.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}

public class AvatarController : MonoBehaviour
{
    [SerializeField] private MapTransforms head;
    [SerializeField] public MapTransforms leftHand;
    [SerializeField] public MapTransforms rightHand;

    [SerializeField] private float turnSmoothness;
    [SerializeField] Transform ikHead;
    [SerializeField] Transform wholeRig;
    [SerializeField] Vector3 headBodyOffset;


    private void Update()
    {
        //transform.forward = wholeRig.forward;
    }

    private void LateUpdate()
    {
        //transform.position = ikHead.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

        //head.VRMapping();
        //if (leftHand.vrTargets)
        //    leftHand.VRMapping();
        //if (rightHand.vrTargets)
        //    rightHand.VRMapping();
    }
}
