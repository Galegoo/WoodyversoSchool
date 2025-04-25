using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using Fusion.XR.Host;


public class ScreensAndHudManager : MonoBehaviour
{
    [SerializeField] Button teacherBoardButton;
    [SerializeField] Button preConfigButton;
    [SerializeField] Button pushToTalkButton;
    [SerializeField] Button pushToMuteButton;

    [SerializeField] GameObject teacherBoard;
    [SerializeField] GameObject preConfig;
    [SerializeField] GameObject config;
    GameObject Screens;

    [SerializeField] private PopCanvas teacherBoardPop;
    [SerializeField] private PopCanvas preConfigPop;
    [SerializeField] private PopCanvas configPop;

    [SerializeField] GameObject hud;

    [SerializeField] private float timeToChangeScreens = 0.1f;
    PopCanvas actualCanvas;

    public int wayToTalk;
    [SerializeField] Sprite selectedBackground;
    [SerializeField] Sprite _whiteBackground;
    [SerializeField] Sprite selectedPushToTalk;
    [SerializeField] Sprite selectedPushToMute;
    [SerializeField] Sprite _whitePushToTalk;
    [SerializeField] Sprite _whitePushToMute;
    [SerializeField] TMP_Dropdown activationButtonDropDown;
    List<string> desktopActivationButtonDropOptions = new List<string> { "Ctrl Esquerdo", "Z", "X", "C" };
    [SerializeField] TMP_Dropdown resolutionDropdown;

    public bool isMuted = false;
    [SerializeField] Sprite unMuteSprite;
    [SerializeField] Sprite mutedSprite;
    [SerializeField] Button muteButton;
    int countMuteButton = 0;

    [SerializeField] GameObject debugButton;
    [SerializeField] Slider volumeSlider;
    [SerializeField] TMP_InputField inputNameAvatar;
    [SerializeField] TMP_Text AvatarNameText;
    [SerializeField] Button editAvatarNameButton;
    [SerializeField] int countEditAvatarNameButton;

    [SerializeField]float sliderValueStorage;
    [SerializeField]int timesLogged;

    [SerializeField]bool _plataformWasChosen = false;

    void Start()
    {
        actualCanvas = null;

        wayToTalk = PlayerPrefs.GetInt("wayToTalk");
    }
    private void Update()
    {
        if (hud != null)   //if any screen is on, hud is off
        {
            if (actualCanvas != null)
                hud.SetActive(false);
            else
                hud.SetActive(true);
        }

        if(sliderValueStorage != volumeSlider.value)
        {
            volumeDefiner();
        }
    }

    void OnTeacherPanelButtonClick()
    {
        if (!teacherBoard.activeSelf)
            teacherBoard.SetActive(true);
        StartCoroutine(ChangeActualScreen(teacherBoardPop));
    }
    void OnConfigButtonClick()
    {
        if (!preConfig.activeSelf)
            preConfig.SetActive(true);
        StartCoroutine(ChangeActualScreen(preConfigPop));
    }
    public void OnPushToTalk()
    {
        Image sprite1 = pushToTalkButton.transform.GetChild(0).GetComponent<Image>();
        Image sprite2 = pushToMuteButton.transform.GetChild(0).GetComponent<Image>();

        wayToTalk = 0;
        pushToTalkButton.image.sprite = selectedBackground;
        sprite1.sprite = _whitePushToTalk;
        pushToMuteButton.image.sprite = _whiteBackground;
        sprite2.sprite = selectedPushToMute;
    }

    public void OnPushToMute()
    {
        Image sprite1 = pushToMuteButton.transform.GetChild(0).GetComponent<Image>();
        Image sprite2 = pushToTalkButton.transform.GetChild(0).GetComponent<Image>();

        wayToTalk = 1;

        pushToMuteButton.image.sprite = selectedBackground;
        sprite1.sprite = _whitePushToMute;
        pushToTalkButton.image.sprite = _whiteBackground;
        sprite2.sprite = selectedPushToTalk;
    }

    public void closePanelButton()
    {
        actualCanvas.PopOut();
        actualCanvas = null;
    }

    public void SetHudGameObject(GameObject _hud)
    {
        hud = _hud;
    }

    public void plataformWasChosen()
    {
        Screens = GameObject.Find("Screens");

        _plataformWasChosen = true;

        teacherBoard = Screens.transform.GetChild(0).gameObject;
        teacherBoardPop = Screens.transform.GetChild(0).transform.GetComponentInChildren<PopCanvas>();
        teacherBoardButton = hud.transform.GetChild(1).transform.GetChild(0).GetComponent<Button>();
        teacherBoardButton.onClick.AddListener(OnTeacherPanelButtonClick);

        preConfig = Screens.transform.GetChild(1).gameObject;
        preConfigPop = Screens.transform.GetChild(1).transform.GetComponentInChildren<PopCanvas>();
        preConfigButton = hud.transform.GetChild(1).transform.GetChild(3).GetComponent<Button>();
        preConfigButton.onClick.AddListener(OnConfigButtonClick);

        config = Screens.transform.GetChild(2).gameObject;
        configPop = Screens.transform.GetChild(2).transform.GetComponentInChildren<PopCanvas>();
        
        pushToTalkButton = Screens.transform.GetChild(2).transform.GetChild(0).transform.GetChild(1).GetComponent<Button>();
        pushToTalkButton.onClick.AddListener(OnPushToTalk);
        pushToMuteButton = Screens.transform.GetChild(2).transform.GetChild(0).transform.GetChild(2).GetComponent<Button>();
        pushToMuteButton.onClick.AddListener(OnPushToMute);
        
        activationButtonDropDown = Screens.transform.GetChild(2).transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<TMP_Dropdown>();
        resolutionDropdown = Screens.transform.GetChild(2).transform.GetChild(0).transform.GetChild(5).GetComponent<TMP_Dropdown>();
        
        muteButton = GameObject.Find("Micro Button").GetComponent<Button>();
        muteButton.onClick.AddListener(OnMuteButton);
        
        debugButton = Screens.transform.GetChild(2).transform.GetChild(0).transform.GetChild(17).gameObject;
        debugButton.GetComponent<Button>().onClick.AddListener(confirmConfig);

        editAvatarNameButton = Screens.transform.GetChild(2).transform.GetChild(0).transform.GetChild(11).GetComponent<Button>();
        editAvatarNameButton.onClick.AddListener(OnEditAvatarNameButton);
        inputNameAvatar = Screens.transform.GetChild(2).transform.GetChild(0).transform.GetChild(12).GetComponent<TMP_InputField>();
        AvatarNameText = Screens.transform.GetChild(2).transform.GetChild(0).transform.GetChild(15).GetComponent<TMP_Text>();

        if (PlayerPrefs.GetString("AvatarName") != "")
            AvatarNameText.text = PlayerPrefs.GetString("AvatarName");
        else
            AvatarNameText.text = "Avatar";

        volumeSlider = Screens.transform.GetChild(2).transform.GetChild(0).transform.GetChild(16).GetComponent<Slider>();
        volumeSlider.value = 1;
        volumeDefiner();

        if (Screens.gameObject.tag == "ScreensDesktop")
        {
            activationButtonDropDown.ClearOptions();
            activationButtonDropDown.AddOptions(desktopActivationButtonDropOptions);
        }

        if (wayToTalk == 0)
            OnPushToTalk();
        else
            OnPushToMute();

    }

    IEnumerator ChangeActualScreen(PopCanvas newScreen)
    {
        yield return new WaitForSeconds(timeToChangeScreens);

        actualCanvas = newScreen;
        actualCanvas.PopIn();
    }
    public void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void GoConfigMenu()
    {
        closePanelButton();
        preConfig.SetActive(false);
        if (!config.activeSelf)
            config.SetActive(true);

        StartCoroutine(ChangeActualScreen(configPop));
        StartCoroutine(waitForDebug(debugButton));

        if (wayToTalk == 0)
            OnPushToTalk();
        else
            OnPushToMute();
    }

    public void confirmConfig()
    {
        countEditAvatarNameButton = 0;
        if (resolutionDropdown.gameObject.activeSelf)
        {
            if (resolutionDropdown.value == 0)
            {
                Screen.SetResolution(1920, 1080, true);
            }
            else if (resolutionDropdown.value == 1)
            {
                Screen.SetResolution(1366, 768, true);
            }
            else if (resolutionDropdown.value == 2)
            {
                Screen.SetResolution(2560, 1440, true);
            }
        }

        RigCustomInputs rgi = FindObjectOfType<RigCustomInputs>();

        if (Screens.gameObject.tag == "ScreensDesktop")
        {
            if (activationButtonDropDown.value == 0)
            {
                rgi.voipButton.action.ChangeBinding(0).Erase();
                rgi.voipButton.action.AddBinding("<Keyboard>/leftCtrl");
            }
            else if (activationButtonDropDown.value == 1)
            {
                rgi.voipButton.action.ChangeBinding(0).Erase();
                rgi.voipButton.action.AddBinding("<Keyboard>/#(z)");
            }
            else if (activationButtonDropDown.value == 2)
            {
                rgi.voipButton.action.ChangeBinding(0).Erase();
                rgi.voipButton.action.AddBinding("<Keyboard>/#(x)");
            }
            else if (activationButtonDropDown.value == 3)
            {
                rgi.voipButton.action.ChangeBinding(0).Erase();
                rgi.voipButton.action.AddBinding("<Keyboard>/#(c)");
            }
        }

        else
        {
            if (activationButtonDropDown.value == 0)
            {
                rgi.voipButton.action.ChangeBinding(0).Erase();
                rgi.voipButton.action.AddBinding("<XRController>{RightHand}/primaryButton");
            }
            else if (activationButtonDropDown.value == 1)
            {
                rgi.voipButton.action.ChangeBinding(0).Erase();
                rgi.voipButton.action.AddBinding("<XRController>{RightHand}/secondaryButton");
            }
        }
        PlayerPrefs.SetInt("wayToTalk", wayToTalk);
        PlayerPrefs.SetString("AvatarName", AvatarNameText.text);
        closePanelButton();
    }

    public void MuteLocal()
    {
        isMuted = true;
        muteButton.GetComponent<Image>().sprite = mutedSprite;
    }

    public void UnmuteLocal()
    {
        isMuted = false;
        muteButton.GetComponent<Image>().sprite = unMuteSprite;
    }

    public void OnMuteButton()
    {
        if (countMuteButton == 0)
        {
            MuteLocal();
            countMuteButton++;
        }
        else
        {
            countMuteButton = 0;
            UnmuteLocal();
        }
    }

    IEnumerator waitForDebug( GameObject gm)
    {
        gm.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        gm.SetActive(true);
    }

    public void OnEditAvatarNameButton()
    {
        if(countEditAvatarNameButton == 0)
        {
            inputNameAvatar.gameObject.SetActive(true);
            AvatarNameText.gameObject.SetActive(false);
            countEditAvatarNameButton++;
        }
        else
        {
            string storage = inputNameAvatar.text;
            inputNameAvatar.gameObject.SetActive(false);
            AvatarNameText.gameObject.SetActive(true);
            AvatarNameText.text = storage;
            countEditAvatarNameButton = 0;
        }
    }

    public void volumeDefiner()
    {

        ConnectionManager cm = GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>();
        if(cm != null)
        {
            foreach (var user in cm.GetSpawnedUsers().Values)
            {
                AudioSource audioSource = user.GetComponentInChildren(typeof(AudioSource)) as AudioSource;
                audioSource.volume = volumeSlider.value;
                sliderValueStorage = volumeSlider.value;
            }
        }
    }
}






