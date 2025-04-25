using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowserSwitchPlatform : MonoBehaviour
{
    [SerializeField] private GameObject pcCanvas;
    [SerializeField] private GameObject androidCanvas;
    private void Awake()
    {
#if UNITY_ANDROID
        androidCanvas.SetActive(true);
        pcCanvas.SetActive(false);
#else
        androidCanvas.SetActive(false);
        pcCanvas.SetActive(true);
#endif

    }
}
