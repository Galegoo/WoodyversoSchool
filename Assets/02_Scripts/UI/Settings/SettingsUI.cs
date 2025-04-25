using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance;
    [SerializeField] private TextMeshProUGUI heightText;
    public GameObject scene1;
    public GameObject classRoomScene;
    public GameObject connectionManager;
    public GameObject browser;
    //public GameObject vivox;
    public GameObject ofRig;
    public GameObject selectRig;
    public Material skybox_Scene1;
    public Material skybox_Scene2;

    private float playerSize = Mathf.Clamp(1.7f, 1.2f, 2.0f);

    [SerializeField] private PopCanvas myScreen;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        RenderSettings.skybox = skybox_Scene1;

    }
    private void Start()
    {
        ChangePlayerSize(0);
    }
    public void ChangePlayerSize(float amount)
    {
        playerSize += amount;

        if (playerSize > 2.0f || playerSize < 1.1f)
        {
            Debug.Log("Altura nao suportada, tente novamente!");
            playerSize = 1.5f;
        }

        heightText.text = playerSize.ToString();

        if(PlayerData.Instance)
        {
            PlayerData.Instance.avatarSize = playerSize;
        }
    }

    public void OpenSettings()
    {
        myScreen.PopIn();
    }

    public void CloseSettings()
    {
        MainMenuController menuController = FindObjectOfType<MainMenuController>();
        if(menuController)
        {
            //menuController.GoToClassRoom();
            GoToClassRoomSameScene();
        }
        myScreen.PopOut();
    }

    public void GoToClassRoomSameScene()
    {
        RenderSettings.skybox = skybox_Scene2;
        SelectRigScreen SR = FindObjectOfType<SelectRigScreen>();
        ofRig.transform.SetParent(selectRig.transform);
        ofRig.transform.localPosition = new Vector3(0, 0, -400);
        if (SR.isMobile)
        {
            SR.SelectVR();
            connectionManager.SetActive(true);
            //vivox.SetActive(true);
            browser.SetActive(true);
            scene1.SetActive(false);
        }

    }

    public void HideScreenWasCalled()
    {
        connectionManager.SetActive(true);
        //vivox.SetActive(true);
        browser.SetActive(true);
        scene1.SetActive(false);
    }
}
