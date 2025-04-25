using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Fusion.XR.Host;
using System.Collections.Generic;
using Fusion;

public class SettingsMenuDesktop : MonoBehaviour
{
    [Header("space between menu items")]
    [SerializeField] Vector2 spacing;

    [Space]
    [Header("Main button rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] Ease rotationEase;

    [Space]
    [Header("Animation")]
    [SerializeField] float expandDuration;
    [SerializeField] float collapseDuration;
    [SerializeField] Ease expandEase;
    [SerializeField] Ease collapseEase;

    [Space]
    [Header("Fading")]
    [SerializeField] float expandFadeDuration;
    [SerializeField] float collapseFadeDuration;

    Button mainButton;
    [SerializeField] SettingsMenuItem[] menuItems;
    [SerializeField] InputField inputField;

    //is menu opened or not
    bool isExpanded = false;

    Vector2 mainButtonPosition;
    int itemsCount;
    Color noAlpha = new Color(1, 1, 1, 1);
    Color alpha = new Color(1, 1, 1, 1);


    void Start()
    {

        noAlpha.a = 0; // set alpha from buttons
        alpha.a = 1;



        itemsCount = transform.childCount - 1;         //add all the items to the menuItems array
        menuItems = new SettingsMenuItem[itemsCount];

        for (int i = 0; i < itemsCount; i++)
        {
            // +1 to ignore the main button
            menuItems[i] = transform.GetChild(i + 1).GetComponent<SettingsMenuItem>();
        }

        mainButton = transform.GetChild(0).GetComponent<Button>(); // set the first child as main button
        mainButton.onClick.AddListener(ToggleMenu);




        mainButton.transform.SetAsLastSibling(); //SetAsLastSibling () to make sure that the main button will be always at the top layer

        mainButtonPosition = mainButton.GetComponent<RectTransform>().anchoredPosition;            //set all menu items position to mainButtonPosition
        menuItems[4].transform.GetChild(0).GetComponent<Image>().DOFade(0f, collapseFadeDuration);


        ResetPositions(); // set every button but the main to alpha 0 and disable button

    }

    void ResetPositions()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.anchoredPosition = mainButtonPosition;
            menuItems[i].GetComponent<Button>().enabled = false;
            menuItems[i].GetComponent<Image>().color = noAlpha;
        }
    }

    void ToggleMenu()
    {
        isExpanded = !isExpanded;

        if (isExpanded)
        {
            //menu opened
            for (int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i].GetComponent<Image>().color = alpha;
                menuItems[i].GetComponent<Button>().enabled = true;
                menuItems[i].rectTrans.DOAnchorPos(new Vector2(mainButtonPosition.x, mainButtonPosition.y), expandDuration).SetEase(expandEase);
            }

            /*menuItems [4].rectTrans.DOAnchorPos(new Vector2 (-54, 156), expandDuration).SetEase(expandEase);
            menuItems [4].rectTrans.DOAnchorPos(new Vector2 (mainButtonPosition.x, mainButtonPosition.y), expandDuration).SetEase(expandEase);
            menuItems[0].rectTrans.DOAnchorPos(new Vector2(-103, 144), expandDuration).SetEase(expandEase);
            menuItems[1].rectTrans.DOAnchorPos(new Vector2(-136, 114), expandDuration).SetEase(expandEase);
            menuItems[2].rectTrans.DOAnchorPos(new Vector2(-154, 69), expandDuration).SetEase(expandEase);
            menuItems[3].rectTrans.DOAnchorPos(new Vector2(-136, 21), expandDuration).SetEase(expandEase);*/

            for (int i = 0; i < itemsCount; i++)
            {
                //Fade to alpha=1 starting from alpha=0 immediately
                menuItems[i].img.DOFade(1f, expandFadeDuration).From(0f);
            }
            menuItems[4].transform.GetChild(0).GetComponent<Image>().DOFade(1f, expandFadeDuration).From(0f);
        }
        else
        {
            //menu closed
            for (int i = 0; i < itemsCount; i++)
            {
                menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition, collapseDuration).SetEase(collapseEase);
                //Fade to alpha=0
                menuItems[i].img.DOFade(0f, collapseFadeDuration);
            }
            menuItems[4].transform.GetChild(0).GetComponent<Image>().DOFade(0f, collapseFadeDuration);
            if (FindObjectOfType<SettingsMenu2>().isExpanded)
            {
                FindObjectOfType<SettingsMenu2>().ToggleMenu();
                FindObjectOfType<SettingsMenu2>().ResetPositions();
            }
        }

        //rotate main button arround Z axis by 180 degree starting from 0
        /* mainButton.transform
              .DORotate (Vector3.forward * 180f, rotationDuration)
              .From (Vector3.zero)
              .SetEase (rotationEase) ;*/
    }

    void OnDestroy()
    {
        //remove click listener to avoid memory leaks
        mainButton.onClick.RemoveListener(ToggleMenu);
    }

    public void KickOutButton()
    {
        if (inputField == null)
        {
            Debug.Log("Input field is null!");
            return;
        }

        string userID = inputField.text;

        if (string.IsNullOrEmpty(userID))
        {
            Debug.Log("User ID is null!");
            return;
        }

        ConnectionManager cm = GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>();

        if (cm == null)
        {
            Debug.Log("Connection Manager not found!");
            return;
        }

        foreach (var user in cm.GetSpawnedUsers().Values)
        {
            Debug.Log(user.Id.ToString());

            if (user.Id.ToString() == userID)
            {
                cm.runner.Despawn(user);
            }
        }
    }
    public void UnMuteAllButton()
    {
        ConnectionManager cm = GameObject.Find("ConnectionManager").GetComponent<ConnectionManager>();

        foreach (var user in cm.GetSpawnedUsers().Values)
        {
            AudioSource audioSource = user.GetComponentInChildren(typeof(AudioSource)) as AudioSource;
            audioSource.mute = false;
        }
    }
}

