using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(HardwareRig))]
public class RigLocomotion : MonoBehaviour
{
    [Header("Snap turn")]
    public InputActionProperty leftControllerTurnAction;
    public InputActionProperty rightControllerTurnAction;

    public float debounceTime = 0.5f;
    public float snapDegree = 45f;
    public float rotationInputThreshold = 0.5f;

    [Header("Teleportation")]
    [Tooltip("Automatically found if not set")]
    //public List<RayBeamer> teleportBeamers;

    bool rotating = false;
    float timeStarted = 0;

    HardwareRig rig;

    public LayerMask locomotionLayerMask = 0;

    private void Awake()
    {
        rig = GetComponentInParent<HardwareRig>();
        //if (teleportBeamers.Count == 0) teleportBeamers = new List<RayBeamer>(GetComponentsInChildren<RayBeamer>());
        //foreach (var beamer in teleportBeamers)
        //{
        //    beamer.onRelease.AddListener(OnBeamRelease);
        //}

        var bindings = new List<string> { "joystick" };
        leftControllerTurnAction.EnableWithDefaultXRBindings(leftBindings: bindings);
        rightControllerTurnAction.EnableWithDefaultXRBindings(rightBindings: bindings);
    }

    private void Start()
    {
        if (locomotionLayerMask == 0)
            Debug.LogError("RigLocomotion: for locomotion to be possible, at least one layer has to be added to locomotionLayerMask, add used on locomotion surface colliders");
    }

    protected virtual void Update()
    {
        CheckSnapTurn();
    }



    protected virtual void CheckSnapTurn()
    {
        if (rotating) return;
        if (timeStarted > 0f)
        {
            // Wait for a certain amount of time before allowing another turn.
            if (timeStarted + debounceTime < Time.time)
            {
                timeStarted = 0f;
            }
            return;
        }

        var leftStickTurn = leftControllerTurnAction.action.ReadValue<Vector2>().x;
        var rightStickTurn = rightControllerTurnAction.action.ReadValue<Vector2>().x;

        if (Mathf.Abs(leftStickTurn) > rotationInputThreshold)
        {
            timeStarted = Time.time;
            StartCoroutine(Rotate(Mathf.Sign(leftStickTurn) * snapDegree));
        }
        else if (Mathf.Abs(rightStickTurn) > rotationInputThreshold)
        {
            timeStarted = Time.time;
            StartCoroutine(Rotate(Mathf.Sign(rightStickTurn) * snapDegree));
        }
    }

    IEnumerator Rotate(float angle)
    {
        timeStarted = Time.time;
        rotating = true;
        rig.Rotate(angle);
        rotating = false;
        yield return null;
    }

    public virtual bool ValidLocomotionSurface(Collider surfaceCollider)
    {
        // We check if the hit collider is in the locomoation layer mask
        bool colliderInLocomotionLayerMask = locomotionLayerMask == (locomotionLayerMask | (1 << surfaceCollider.gameObject.layer));
        return colliderInLocomotionLayerMask;
    }

    protected virtual void OnBeamRelease(Collider lastHitCollider, Vector3 position)
    {
        if (ValidLocomotionSurface(lastHitCollider))
        {
            //StartCoroutine(rig.FadedTeleport(position));
        }
    }
}
