using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    public static PlayerData Instance;
    public string avatarUrl;
    public float avatarSize;


    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    public string GetAvatarUrl()
    {
        return avatarUrl;
    }
}
