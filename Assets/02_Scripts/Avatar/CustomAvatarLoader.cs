using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReadyPlayerMe;
using Vuplex.WebView;

public class CustomAvatarLoader : MonoBehaviour
{
    private GameObject avatar;
    private AvatarLoader avatarLoader;

    [Header("Avatar Local")]
    [SerializeField] private SkinnedMeshRenderer meshRend;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Animator animator;


    private void Awake()
    {
        avatarLoader = new AvatarLoader();
        avatarLoader.OnCompleted += OnAvatarLoadCompleted;

    }

    public void DownloadAvatar(string url)
    {
        avatarLoader.LoadAvatar(url);
    }

    private void OnAvatarLoadCompleted(object sender, CompletionEventArgs args)
    {
        //GetComponent<NetworkRig>().hideMeshFromMainCamera();
        avatar = args.Avatar;
        avatar.transform.Rotate(Vector3.up, 180);
        avatar.transform.position = new Vector3(0.75f, 0, 0.75f);
        avatar.gameObject.SetActive(true);
        Renderer avatarRend = avatar.GetComponentInChildren<Renderer>();
        meshRend.material = avatarRend.material;
        meshRend.sharedMesh = avatarRend.GetComponent<SkinnedMeshRenderer>().sharedMesh;

        Destroy(avatar);
        //meshRend. = avatarRend.material;

        //loading.SetActive(false);
        //SetWebViewVisibility(false);

        Debug.Log("Avatar Load Completed");      
    }
}
