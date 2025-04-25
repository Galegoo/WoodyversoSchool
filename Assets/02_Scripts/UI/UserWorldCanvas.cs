using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Fusion.Sockets;
using System;
using Fusion.XR.Host;

public class UserWorldCanvas : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private TextMeshProUGUI userNameText;
    [SerializeField] private GameObject[] emotes;
    [SerializeField] private GameObject talkingIcon;
    [HideInInspector] public NetworkRunner runner;


    [Networked(OnChanged = nameof(OnChangeEmote))] 
    public Emote actualEmote { get; set; }

    [Networked(OnChanged = nameof(OnChangedTalking))]
    public bool isTalking { get; set; }
    IEnumerator RemoteEmoteCoroutine()
    {
        emotes[0].SetActive(true);
        yield return new WaitForSeconds(2f);
        emotes[0].SetActive(false);

    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (GetInput<RigInput>(out var input))
        {
            actualEmote = input.emoteInfo;
            isTalking = input.isTalking;

        }
    }
    
    static void OnChangeEmote(Changed<UserWorldCanvas> changed)
    {
        Emote currentEmote = changed.Behaviour.actualEmote;

        changed.LoadOld();

        Emote oldEmote = changed.Behaviour.actualEmote;
        //Debug.Log("Um player está tentando mandar emote");
        if(oldEmote == Emote.None && currentEmote != Emote.None)
        {
            changed.Behaviour.RemoteEmote();
        }
    }

    static void OnChangedTalking(Changed<UserWorldCanvas> changed)
    {
        changed.Behaviour.RemoteTalking(changed.Behaviour.isTalking);

    }
    void RemoteEmote()
    {
        StartCoroutine(RemoteEmoteCoroutine());
    }

    void RemoteTalking(bool set)
    {
        talkingIcon.SetActive(set);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Object.Runner = runner;
    }

    #region Callbacks
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }
    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        
    }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
    }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        
    }
    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }
    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    #endregion
}

public enum Emote
{
    None, RaiseHand, Agreed, Disagree
}



