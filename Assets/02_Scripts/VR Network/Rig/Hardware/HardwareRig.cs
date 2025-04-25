using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Fusion;
using Fusion.Sockets;
using System;
using Vuplex.WebView.Demos;
using Fusion.XR.Host;
public enum RigPart
{
    None,
    Headset,
    LeftController,
    RightController,
    Undefined
}

// Include all rig parameters in an network input structure
public struct RigInput : INetworkInput
{
    public Vector3 playAreaPosition;
    public Quaternion playAreaRotation;
    public Vector3 leftHandPosition;
    public Quaternion leftHandRotation;
    public Vector3 rightHandPosition;
    public Quaternion rightHandRotation;
    public Vector3 headsetPosition;
    public Quaternion headsetRotation;


    public Vector2 moveDirection;
    public Vector3 avatarScale;

    //public HandCommand leftHandCommand;
    //public HandCommand rightHandCommand;

    public GrabInfo leftGrabInfo;
    public GrabInfo rightGrabInfo;

    public Emote emoteInfo;

    public NetworkBool isTalking;
    public NetworkString<_128> avatarUrl;

    public NetworkBool isVrRig;
    public NetworkBool sitted;
    public Vector3 emotePlacement;
    public int users;
    public NetworkBool isProfessor;
    public NetworkString<_128> boardURL;
    public int whatScreenIsOn;
}

public class HardwareRig : MonoBehaviour, INetworkRunnerCallbacks
{
    public HardwareHand leftHand;
    public HardwareHand rightHand;
    public HardwareHeadset headset;
    public NetworkRunner runner;

    [HideInInspector] public LocalClassChair chairSitted;
    [HideInInspector] public NetworkRig networkedRig;
    [HideInInspector] public Vector2 moveDir;

    public Emote actualEmote;

    public bool localIsTalking;
    public bool isVR;
    bool privateIsVr;

    public bool sitted;
    public string urlAvatar;
    
    
    public bool isHost;
    public GameObject Board;
    private int controller;

    public int whatBoardScreenIsOn;
    public string urlBoard;
    // Start is called before the first frame update
    void Start()
    {

        if (runner == null)
        {
            runner = FindObjectOfType<NetworkRunner>();
            Debug.LogError("Runner has to be set in the inspector to forward the input");
        }
        runner.AddCallbacks(this);
        if(!isVR)
        {

        }
        else
        {
            
        }
        privateIsVr = isVR;

    }
    private void Update()
    {
        if (runner.GameMode == GameMode.Host && controller <= 0)
        {
            isHost = true;
            //Board.GetComponent<GraphicRaycaster>().enabled = true;
            controller++;
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        RigInput rigInput = new RigInput();
        rigInput.playAreaPosition = transform.position;
        rigInput.playAreaRotation = transform.rotation;
        rigInput.leftHandPosition = leftHand.transform.position;
        rigInput.leftHandRotation = leftHand.transform.rotation;
        rigInput.rightHandPosition = rightHand.transform.position;
        rigInput.rightHandRotation = rightHand.transform.rotation;
        rigInput.headsetPosition = headset.transform.position;
        rigInput.headsetRotation = headset.transform.rotation;
        rigInput.emotePlacement = new Vector3(headset.transform.position.x, headset.transform.position.y + 0.2f, headset.transform.position.z);
        if (PlayerData.Instance)
            rigInput.avatarScale = Vector3.one * (PlayerData.Instance.avatarSize / 1.863f);

        if (PlayerData.Instance)
            rigInput.avatarUrl = PlayerData.Instance.GetAvatarUrl();
        //rigInput.leftHandCommand = leftHand.handCommand;
        //rigInput.rightHandCommand = rightHand.handCommand;

        rigInput.moveDirection = moveDir;

        rigInput.leftGrabInfo = leftHand.grabber.GrabInfo;
        rigInput.rightGrabInfo = rightHand.grabber.GrabInfo;

        rigInput.emoteInfo = actualEmote;
        rigInput.isTalking = localIsTalking;
        rigInput.isVrRig = privateIsVr;
        rigInput.sitted = sitted;
        rigInput.isProfessor = isHost;
        rigInput.whatScreenIsOn = whatBoardScreenIsOn;
        rigInput.boardURL = urlBoard;


        input.Set(rigInput);
    }

    public virtual void Rotate(float angle)
    {
        transform.RotateAround(headset.transform.position, transform.up, angle);
    }
    public virtual void Teleport(Vector3 position)
    {
        Vector3 headsetOffet = headset.transform.position - transform.position;
        headsetOffet.y = 0;
        transform.position = position - headsetOffet;
    }
    public void ChangeSittingStatus(LocalClassChair chair)
    {
        chairSitted = chair;
    }

    #region Callbacks

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        //throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        //throw new NotImplementedException();
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        //throw new NotImplementedException();
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        //throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        //throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        //throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        //throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        //throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        //throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        //throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        //throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        //throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        //throw new NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        //throw new NotImplementedException();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        //throw new NotImplementedException();
    }

    #endregion

}
