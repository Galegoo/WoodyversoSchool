using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using global::Fusion.Sockets;
using PhotonAppSettings = global::Fusion.Photon.Realtime.PhotonAppSettings;
using ExitGames.Client.Photon;
using Vuplex.WebView.Demos;
/**
* 
* Networked VR user
* 
* Handle the synchronisation of the various rig parts: headset, left hand, right hand, and playarea (represented here by the NetworkRig)
* Use the local HardwareRig rig parts position info when this network rig is associated with the local user 
* 
* 
**/
[RequireComponent(typeof(NetworkTransform))]
// We ensure to run after the NetworkTransform or NetworkRigidbody, to be able to override the interpolation target behavior in Render()
[OrderAfter(typeof(NetworkTransform), typeof(NetworkRigidbody))]
public class NetworkRig : NetworkBehaviour
{
    public HardwareRig hardwareRig;
    public NetworkHand leftHand;
    public NetworkHand rightHand;
    public NetworkHeadset headset;
    public NetworkGrabber leftGrabber;
    public NetworkGrabber rightGrabber;

    [HideInInspector]
    public NetworkTransform networkTransform;
    public UserWorldCanvas myCanvas;

    [Header("Avatar Transforms")]
    [SerializeField] private Transform ikHead;
    [SerializeField] private NetworkTransform ikHeadNetTransform;
    [SerializeField] private AvatarAnimationController avatarAnimController;
    [SerializeField] private AvatarController avatarIkHands;

    [System.Serializable]
    struct NetworkStruct : INetworkStruct
    {
        // NetworkString is a normal struct, so it doesn't require any Fusion attributes
        public NetworkString<_128> NestedString;

        // Define default initialization as a property if needed.
        public static NetworkStruct Defaults
        {
            get
            {
                var result = new NetworkStruct();
                result.NestedString = "Initialized";
                return result;
            }
        }
    }

    [Networked(OnChanged = nameof(OnChangeAvatar))]
    [UnitySerializeField]
    private NetworkStruct NestedStruct { get; set; } = NetworkStruct.Defaults;


    [Networked(OnChanged = nameof(OnChangeRig))]
    public bool IsVrRig { get; set; }
    public bool IsSitted { get; set; }
    public bool IsTalking { get; set; }


    //[Networked] public string AvatarUrl1 { get; set; }
    //[Networked] public string AvatarUrl2 { get; set; }
    //[Networked] public string AvatarUrl3 { get; set; }
    //[Networked] public string AvatarUrl4 { get; set; }
    //[Networked] public string AvatarUrl5 { get; set; }
    //[Networked] public string AvatarUrl6 { get; set; }
    //[Networked] public string AvatarUrl7 { get; set; }

    void Awake()
    {
        networkTransform = GetComponent<NetworkTransform>();
        //ikHeadNetTransform = ikHead.GetComponent<NetworkTransform>();
        leftGrabber = leftHand.GetComponent<NetworkGrabber>();
        rightGrabber = rightHand.GetComponent<NetworkGrabber>();
    }
    // As we are in host topology, we use the input authority to track which player is the local user
    public bool IsLocalNetworkRig => Object.HasInputAuthority;

    public override void Spawned()
    {
        base.Spawned();
        if (IsLocalNetworkRig)
        {
            //HIDE FROM MAIN CAMERA
            
            Renderer[] renders = GetComponentsInChildren<Renderer>();
            foreach(Renderer rend in renders)
            {
                rend.gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
            }

            hardwareRig = FindObjectOfType<HardwareRig>();

            if (hardwareRig == null) Debug.LogError("Missing HardwareRig in the scene");

            hardwareRig.networkedRig = this;

            FindObjectOfType<ChairsManager>().FindPlaceToSit(hardwareRig);
            if(!hardwareRig.isVR)
            {
                //TURN OFF MASK
                IsVrRig = false;
            }
            else
            {
                IsVrRig = true;
                avatarIkHands.leftHand.vrTargets = hardwareRig.leftHand.transform;
                avatarIkHands.rightHand.vrTargets = hardwareRig.rightHand.transform;
            }
            string originalUrl = "";
            
            if (PlayerData.Instance)
                originalUrl = PlayerData.Instance.GetAvatarUrl();

            ModifyValues(originalUrl);
            //AvatarUrl1 = originalUrl;
            //AvatarUrl2 = originalUrl.Substring(16);
            //AvatarUrl3 = originalUrl.Substring(32);
            //if (originalUrl.Length > 48)
            //    AvatarUrl4 = originalUrl.Substring(48);
            //if(originalUrl.Length > 64)
            //    AvatarUrl5 = originalUrl.Substring(64);
            //if (originalUrl.Length > 80)
            //    AvatarUrl6 = originalUrl.Substring(80);
            //if (originalUrl.Length > 96)
            //    AvatarUrl7 = originalUrl.Substring(96);
        }

        //string fullUrl = AvatarUrl1 + AvatarUrl2 + AvatarUrl3 + AvatarUrl4 + AvatarUrl5 + AvatarUrl6 + AvatarUrl7;
        //GetComponent<CustomAvatarLoader>().DownloadAvatar(originalUrl);
        if (!IsVrRig)
        {
            avatarAnimController.TurnOffAvatarMask();
        }
        else
        {

        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        // update the rig at each network tick
        if (GetInput<RigInput>(out var input))
        {
            transform.position = input.playAreaPosition;
            transform.localScale = input.avatarScale;
            //transform.rotation = input.playAreaRotation;
            leftHand.transform.position = input.leftHandPosition;
            rightHand.transform.position = input.rightHandPosition;
            headset.transform.position = input.headsetPosition;
            // we update the hand pose info. It will trigger on network hands OnHandCommandChange on all clients, and update the hand representation accordingly

            leftGrabber.GrabInfo = input.leftGrabInfo;
            rightGrabber.GrabInfo = input.rightGrabInfo;
            ikHead.transform.position = input.headsetPosition;         
            IsVrRig = input.isVrRig;
            if(!input.isVrRig)
            {
                avatarAnimController.TurnOffAvatarMask();
            }
            avatarAnimController.AnimateLegs(input.moveDirection);
            if(!NestedStruct.NestedString.Contains("ready"))
            {
                ModifyValues(input.avatarUrl.Value);
            }
            IsSitted = input.sitted;
            IsTalking = input.isTalking;

            transform.GetChild(6).transform.GetChild(0).transform.position = input.emotePlacement;
            if (input.sitted)
            {
                avatarAnimController.Sit();
                transform.rotation = new Quaternion(0, 0, 0, 0);
                ikHead.transform.rotation = new Quaternion(0, 0, 0, 0);
                leftHand.transform.rotation = new Quaternion(0, 0, 0, 0);
                rightHand.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else
            {
                avatarAnimController.GetUp(); 
                headset.transform.rotation = input.headsetRotation;
                ikHead.transform.rotation = input.headsetRotation;
                leftHand.transform.rotation = input.leftHandRotation;
                rightHand.transform.rotation = input.rightHandRotation;
            }
        }
    }


    static void OnChangeRig(Changed<NetworkRig> changed)
    {
        bool isVrSpawned = changed.Behaviour.IsVrRig;

        if(!isVrSpawned)
        {
            changed.Behaviour.avatarAnimController.TurnOffAvatarMask();
        }
    }

    static void OnChangeAvatar(Changed<NetworkRig> changed)
    {
        string newAvatar = changed.Behaviour.NestedStruct.NestedString.Value;
        changed.LoadOld();
        string oldUrl = changed.Behaviour.NestedStruct.NestedString.Value;
        changed.LoadNew();

        if (newAvatar == "Initialized")
        {
            if (oldUrl.Contains("models"))
            {
                newAvatar = oldUrl;
            }
            else
            {
                
            }
        }
        else if(!newAvatar.Contains("models"))
        {
            return;
        }
        
        Debug.Log(newAvatar);

        
        
        changed.Behaviour.GetComponent<CustomAvatarLoader>().DownloadAvatar(newAvatar);
    }

    public override void Render()
    {
        base.Render();
        if (IsLocalNetworkRig)
        {
            // Extrapolate for local user:
            // we want to have the visual at the good position as soon as possible, so we force the visuals to follow the most fresh hardware positions
            // To update the visual object, and not the actual networked position, we move the interpolation targets
            networkTransform.InterpolationTarget.position = hardwareRig.transform.position;
            networkTransform.InterpolationTarget.rotation = hardwareRig.transform.rotation;
            networkTransform.InterpolationTarget.localScale = hardwareRig.transform.localScale;

            leftHand.networkTransform.InterpolationTarget.position = hardwareRig.leftHand.transform.position;
            leftHand.networkTransform.InterpolationTarget.rotation = hardwareRig.leftHand.transform.rotation;
            rightHand.networkTransform.InterpolationTarget.position = hardwareRig.rightHand.transform.position;
            rightHand.networkTransform.InterpolationTarget.rotation = hardwareRig.rightHand.transform.rotation;
            headset.networkTransform.InterpolationTarget.position = hardwareRig.headset.transform.position;
            headset.networkTransform.InterpolationTarget.rotation = hardwareRig.headset.transform.rotation;


            ikHeadNetTransform.InterpolationTarget.position = hardwareRig.headset.transform.position;

            ikHeadNetTransform.InterpolationTarget.rotation = hardwareRig.headset.transform.rotation;
        }
    }


    public void ModifyValues(string fullUrl)
    {
        // NestedStruct is a value type, so modifications need to be performed on a copy,
        // and the modified result must be applied back to the property.
        if(fullUrl == "Initialized")
        {
            return;
        }

        var copy = NestedStruct;
        //copy.NestedDict.Add(copy.NestedDict.Count, default);
        copy.NestedString = fullUrl;
        NestedStruct = copy;
    }
}
