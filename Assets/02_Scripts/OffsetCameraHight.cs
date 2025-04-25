using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OffsetCameraHight : MonoBehaviour
{
    [SerializeField]TMP_Text height;
    float y;
    // Start is called before the first frame update
    void OnEnable()
    {
        y = float.Parse(height.text) - 0.1f;
        transform.position = new Vector3(transform.position.x, y , transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponentInParent<HardwareRig>().sitted)
        {
            transform.position = new Vector3(transform.position.x, float.Parse(height.text) - 0.1f, transform.position.z);
        }

        else
        {
            transform.position = new Vector3(transform.position.x, float.Parse(height.text) - 0.5f, transform.position.z);
        }
    }
            
}
