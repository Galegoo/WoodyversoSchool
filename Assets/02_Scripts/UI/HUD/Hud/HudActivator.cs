using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudActivator : MonoBehaviour
{
    ScreensAndHudManager hudManagerRef;
    GameObject hudDesktop;
    GameObject screensDesktop;

    private void Start()
    {
        hudManagerRef = FindObjectOfType<ScreensAndHudManager>();
        StartCoroutine(DelayForOnVR());
    }
    public void OnDesktop()
    {
        hudDesktop.SetActive(true);
        hudManagerRef.SetHudGameObject(hudDesktop);

        GameObject hudVR = GameObject.Find("Hud VR");
        if (hudVR != null)
        {
            hudVR.SetActive(false);
        }

        GameObject screensVR = GameObject.FindGameObjectWithTag("ScreensVR");
        if (screensVR != null)
        {
            screensVR.SetActive(false);
        }
        screensDesktop.SetActive(true);
        hudManagerRef.plataformWasChosen();
    }
    public void OnVR()
    {
        hudManagerRef.SetHudGameObject(GameObject.Find("Hud VR"));

        hudDesktop = GameObject.Find("Hud Desktop");
        if (hudDesktop != null)
        {
            hudDesktop.SetActive(false);
        }

        screensDesktop = GameObject.FindGameObjectWithTag("ScreensDesktop");
        if(screensDesktop != null)
        {
            screensDesktop.SetActive(false);
        }
        hudManagerRef.plataformWasChosen();
    }

    IEnumerator DelayForOnVR()
    {
        yield return new WaitForSeconds(0.1f);
        OnVR();

    }
}
