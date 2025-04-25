using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraFinder : MonoBehaviour
{
    [SerializeField] private Camera myCamera;
    private void Awake()
    {
        CanvasCameraSetter[] canvas = FindObjectsOfType<CanvasCameraSetter>();
        foreach(CanvasCameraSetter canva in canvas)
        {
            canva.SetCamera(myCamera);
        }
    }
}
