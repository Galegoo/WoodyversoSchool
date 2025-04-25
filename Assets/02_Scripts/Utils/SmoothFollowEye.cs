using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowEye : MonoBehaviour
{
    public float distance = 10f;
    public float smoothTime = 80f;

    private Vector3 velocity = Vector3.zero;

    public Camera mainCam;
    public float distanceRequiredToMove = 3f;

    public bool followY = true;
    public bool followX = true;

    bool needToMove;
    // Start is called before the first frame update
    void Start()
    {
        //mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(mainCam)
        {
            Vector3 targetPosition = mainCam.transform.TransformPoint(new Vector3(0, 0, distance));

            if (!followX)
            {
                targetPosition.x = transform.position.x;
            }

            if (!followY)
            {
                targetPosition.y = transform.position.y;
            }

            float actualDistance = Vector3.Distance(targetPosition, transform.position);
            if (actualDistance > distanceRequiredToMove)
            {
                needToMove = true;
            }



            if (needToMove)
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime * Time.deltaTime);
                if (actualDistance < 0.01f)
                {
                    needToMove = false;
                }
            }
        }

        
    }
}
