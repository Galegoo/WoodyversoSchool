using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Singleton;
    //[SerializeField] private CanvasGroup _canvas;
    [SerializeField] private PopCanvas _canvas;
    [SerializeField] private float durationInSeconds;
    [SerializeField] private float durationInSecondsToFadeOut = 5f;

    private void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReloadScene()
    {
        _FadeIn(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadNewScene(string name)
    {
        _FadeIn(name);
    }

    public void LoadNewScene(int number)
    {
        _FadeIn(number);
    }
    public void LoadNewScene(int number, float customTime)
    {
        _FadeIn(number);
    }
    private void _FadeIn(string name)
    {

        //_canvas.interactable = true;
        //_canvas.blocksRaycasts = true;
        _canvas.PopIn();
        SceneLoad(name);
        //_canvas.DOFade(1f, durationInSeconds).OnComplete(() =>
        //{
            
        //});

    }

    private void _FadeIn(int index)
    {
        _canvas.PopIn();
        SceneLoad(index);
    }

    private void _FadeOut()
    {
        _canvas.PopOut();
    }

    void SceneLoad(string name)
    {
        StartCoroutine(LoadYourAsyncScene(name));
    }

    void SceneLoad(int index)
    {
        StartCoroutine(LoadYourAsyncScene(index));
    }

    IEnumerator LoadYourAsyncScene(int index)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        _FadeOut();
    }

    IEnumerator LoadYourAsyncScene(string name)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        _FadeOut();
    }
}
