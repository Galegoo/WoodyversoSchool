using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Grabber : MonoBehaviour
{
    public Grabbable grabbedObject;

    Collider lastCheckedCollider;
    Grabbable lastCheckColliderGrabbable;

    HardwareHand hand;
    HardwareRig rig;

    public Vector3 ungrabPosition;
    public Quaternion ungrabRotation;
    public Vector3 ungrabVelocity;
    public Vector3 ungrabAngularVelocity;
    // Will be set by the NetworkGrabber for the local user itself, when it spawns
    public NetworkGrabber networkGrabber;

    GrabInfo _grabInfo;
    public GrabInfo GrabInfo
    {
        get
        {
            if (grabbedObject)
            {
                _grabInfo.grabbedObjectId = grabbedObject.networkGrabbable.Id;
                _grabInfo.localPositionOffset = grabbedObject.localPositionOffset;
                _grabInfo.localRotationOffset = grabbedObject.localRotationOffset;

            }
            else
            {
                _grabInfo.grabbedObjectId = NetworkBehaviourId.None;
                _grabInfo.ungrabPosition = ungrabPosition;
                _grabInfo.ungrabRotation = ungrabRotation;
                _grabInfo.ungrabVelocity = ungrabVelocity;
                _grabInfo.ungrabAngularVelocity = ungrabAngularVelocity;
            }

            return _grabInfo;
        }
    }
    public XRBaseInteractor interactor;

    private void Awake()
    {
        hand = GetComponentInParent<HardwareHand>();
        rig = GetComponentInParent<HardwareRig>();
        if(interactor)
        {
            interactor.hoverEntered.AddListener(OnHoverInteract);
            interactor.selectEntered.AddListener(Grab);
            interactor.selectExited.AddListener(Ungrab);
        }

    }

    public void OnHoverInteract(HoverEnterEventArgs arg)
    {
        if (rig && rig.runner && rig.runner.IsResimulation)
        {
            // We only manage grabbing during forward ticks, to avoid detecting past positions of the grabbable object
            return;
        }

        // Exit if an object is already grabbed
        if (grabbedObject != null)
        {
            // It is already the grabbed object or another, but we don't allow shared grabbing here
            return;
        }

        Grabbable grabbable;

        if (lastCheckedCollider == arg.interactableObject.colliders[0])
        {
            grabbable = lastCheckColliderGrabbable;
        }
        else
        {
            grabbable = arg.interactableObject.colliders[0].GetComponentInParent<Grabbable>();
        }
        // To limit the number of GetComponent calls, we cache the latest checked collider grabbable result
        lastCheckedCollider = arg.interactableObject.colliders[0];
        lastCheckColliderGrabbable = grabbable;
    }


    private void Update()
    {
        if (rig && rig.runner && rig.runner.IsResimulation)
        {
            // We only manage grabbing during forward ticks, to avoid detecting past positions of the grabbable object
            return;
        }

        if (grabbedObject != null && grabbedObject.currentGrabber != this)
        {
            // This object as been grabbed by another hand, no need to trigger an ungrab
            grabbedObject = null;
        }

    }

    // Ask the grabbable object to start following the hand
    public void Grab(SelectEnterEventArgs arg)
    {
        Grabbable grabbable;
        grabbable = arg.interactableObject.colliders[0].GetComponentInParent<Grabbable>();

        Debug.Log($"Try to grab object {grabbable.gameObject.name} with {gameObject.name}");
        grabbable.Grab(this);
        grabbedObject = grabbable;
    }

    // Ask the grabbable object to stop following the hand
    public void Ungrab(SelectExitEventArgs arg)
    {
        Grabbable grabbable;
        grabbable = arg.interactableObject.colliders[0].GetComponentInParent<Grabbable>();

        Debug.Log($"Try to ungrab object {grabbable.gameObject.name} with {gameObject.name}");
        if (grabbable.networkGrabbable)
        {
            ungrabPosition = grabbedObject.networkGrabbable.networkTransform.InterpolationTarget.transform.position;
            ungrabRotation = grabbedObject.networkGrabbable.networkTransform.InterpolationTarget.transform.rotation;
            ungrabVelocity = grabbedObject.Velocity;
            ungrabAngularVelocity = grabbedObject.AngularVelocity;
        }

        grabbedObject.Ungrab();
        grabbedObject = null;
    }
}
