using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private PopCanvas homeScreen;
    [SerializeField] private PopCanvas loginScreen;
    [SerializeField] private PopCanvas registerScreen;
    [SerializeField] private PopCanvas costumizationScreen;

    [SerializeField] private float timeToChangeScreens = 0.1f;
    PopCanvas actualCanvas;


    // Start is called before the first frame update
    void Start()
    {
        actualCanvas = homeScreen;
    }

    public void ReturnToMenu()
    {
        StartCoroutine(ChangeActualScreen(homeScreen));
    }

    public void GoToLogin()
    {
        StartCoroutine(ChangeActualScreen(loginScreen));
    }

    public void GoToRegister()
    {
        StartCoroutine(ChangeActualScreen(registerScreen));
    }

    public void GoToCostumization()
    {
        costumizationScreen.gameObject.SetActive(true);
        StartCoroutine(ChangeActualScreen(costumizationScreen));
    }

    IEnumerator ChangeActualScreen(PopCanvas newScreen)
    {
        if(actualCanvas)
        {
            actualCanvas.PopOut();
        }
        yield return new WaitForSeconds(timeToChangeScreens);

        actualCanvas = newScreen;
        actualCanvas.PopIn();
    }

    public void GoToClassRoom()
    {
        actualCanvas.PopOut();
        SceneLoader.Singleton.LoadNewScene(1);
    }
}
