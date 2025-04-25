using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudVRBilboard : MonoBehaviour
{
    public bool lockY;
    public Camera camMain;

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = camMain.transform.rotation;

        if (lockY)
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
