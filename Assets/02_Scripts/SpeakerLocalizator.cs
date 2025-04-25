using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerLocalizator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
