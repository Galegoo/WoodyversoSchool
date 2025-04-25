using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pause : MonoBehaviour
{
    [SerializeField] private PopCanvas popEffect;
    [SerializeField] private Canvas worldPauseCanvas;
    [SerializeField] private SmoothFollowEye smoothfollow;
    private bool isOpen;

    private void Awake()
    {
        worldPauseCanvas.worldCamera = Camera.main;
        smoothfollow.mainCam = Camera.main;
    }

    public void DynamicPause()
    {
        if(isOpen)
        {
            popEffect.PopOut();
            isOpen = false;
        }
        else
        {
            popEffect.PopIn();
            isOpen = true;
        }
    }
}
