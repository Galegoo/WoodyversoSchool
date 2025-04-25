using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotesLocalizer : MonoBehaviour
{
    public GameObject head;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
            transform.position = new Vector3 (transform.position.x, head.transform.position.y + 0.4f, transform.position.z);
    }
}
