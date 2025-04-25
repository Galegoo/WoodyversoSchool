using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MouseCamera : MonoBehaviour
{
    public InputActionProperty mouseXAction;
    public InputActionProperty mouseYAction;
    public InputActionProperty mouseClickAction;

    public HardwareRig rig;
    [Header("Mouse point of view")]
    public Vector2 maxHeadRotationSpeed = new Vector2(10, 10);
    public Vector2 sensitivity = new Vector2(10, 10);
    public float maxHeadAngle = 65;
    public float minHeadAngle = 65;
    Vector3 rotationSpeed;
    Vector3 rotation = Vector3.zero;
    Vector2 mouseInput;

    Transform Head => rig == null ? null : rig.headset.transform;


    private void Awake()
    {
        if (mouseXAction.action.bindings.Count == 0) mouseXAction.action.AddBinding("<Mouse>/delta/x");
        if (mouseYAction.action.bindings.Count == 0) mouseYAction.action.AddBinding("<Mouse>/delta/y");

        mouseXAction.action.Enable();
        mouseYAction.action.Enable();
        mouseClickAction.action.Enable();

        mouseClickAction.action.performed += RaycastClick;

        if (rig == null) rig = GetComponentInParent<HardwareRig>();

        Cursor.visible = true;
    }


    private void Update()
    {
        if (Mouse.current != null)
        {
            mouseInput.x = mouseXAction.action.ReadValue<float>() * Time.deltaTime * sensitivity.x;
            mouseInput.y = mouseYAction.action.ReadValue<float>() * Time.deltaTime * sensitivity.y;

            mouseInput.y = Mathf.Clamp(mouseInput.y, -maxHeadRotationSpeed.y, maxHeadRotationSpeed.y);
            mouseInput.x = Mathf.Clamp(mouseInput.x, -maxHeadRotationSpeed.x, maxHeadRotationSpeed.x);

            rotation.x = Head.eulerAngles.x - mouseInput.y;
            rotation.y = Head.eulerAngles.y + mouseInput.x;

            if (Input.GetKey(KeyCode.Space))Cursor.lockState = CursorLockMode.None;

            if (rotation.x > maxHeadAngle && rotation.x < (360 - minHeadAngle))
            {
                if (Mathf.Abs(maxHeadAngle - rotation.x) < Mathf.Abs(rotation.x - (360 - minHeadAngle)))
                {
                    rotation.x = maxHeadAngle;
                }
                else
                {
                    rotation.x = -minHeadAngle;
                }
            }
            else if (rotation.x < -minHeadAngle)
            {
                rotation.x = -minHeadAngle;
            }

            Head.eulerAngles = rotation;

        }
        else
        {
            rotationSpeed = Vector2.zero;
        }
    }


    public void RaycastClick(InputAction.CallbackContext ctx)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Head.position, Head.forward, out hitInfo, 1f))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            if(hitInfo.collider.GetComponent<XRSimpleInteractable>())
            {
                hitInfo.collider.GetComponent<XRSimpleInteractable>().activated.Invoke(new ActivateEventArgs());
            }
        }
    }
}
