using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion.XR.Host;
using Photon.Voice.Fusion;


public class Placa : MonoBehaviour
{
    public TMP_Text Build;
    public TMP_Text amoutOfPlayers;
    public int Players;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Players = FindObjectsOfType<NetworkRig>().Length;
        amoutOfPlayers.text = "Usuários online: " + Players;

        Build.text = "Build: " + FindObjectOfType<FusionVoiceClient>().Client.SerializationProtocol;
    }
}
