using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCameraSetter : MonoBehaviour
{
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private SmoothFollowEye followEye;
    [SerializeField] private Billboard billboard;


    public void SetCamera(Camera newCamera)
    {
        worldCanvas.worldCamera = newCamera;
        followEye.mainCam = newCamera;
        billboard.camMain = newCamera;
    }
}
