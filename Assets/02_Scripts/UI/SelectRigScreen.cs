using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
using Fusion;
public class SelectRigScreen : MonoBehaviour
{
    [SerializeField] private GameObject canvasParent;
    [SerializeField] private GameObject connectingScreen;
    [SerializeField] private GameObject selectScreen;
    [SerializeField] private GameObject desktopRig;
    [SerializeField] private GameObject xrRig;

    [SerializeField] private NetworkRunner runner;
    bool checkingForRunner;
    public bool isMobile = false;

    private void Awake()
    {
        #if UNITY_ANDROID
            isMobile = true;
            //SelectVR();
        #endif
        if (!isMobile)
        {
            selectScreen.SetActive(true);
        }
    }

    private void Update()
    {
        if(checkingForRunner)
        {
            if(runner.IsRunning)
            {
                checkingForRunner = false;
                connectingScreen.SetActive(false);
                canvasParent.SetActive(false);
            }
        }
    }


    public void SelectVR()
    {
        //ENABLE VR
        xrRig.SetActive(true);
        HideScreen();
    }

    public void SelectDesktop()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StopXR();
        desktopRig.SetActive(true);
        HideScreen();
    }

    void HideScreen()
    {
        runner.gameObject.SetActive(true);
        selectScreen.SetActive(false);
        connectingScreen.SetActive(true);
        checkingForRunner = true;
        FindObjectOfType<SettingsUI>().HideScreenWasCalled();
    }

    public IEnumerator StartXRCoroutine()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }

    void StopXR()
    {
        Debug.Log("Stopping XR...");

        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR stopped completely.");
    }
}



