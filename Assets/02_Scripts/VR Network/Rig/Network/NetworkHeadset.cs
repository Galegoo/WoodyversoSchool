using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


[RequireComponent(typeof(NetworkTransform))]
public class NetworkHeadset : NetworkBehaviour
{
    [HideInInspector]
    public NetworkTransform networkTransform;

    private void Awake()
    {
        if (networkTransform == null) networkTransform = GetComponent<NetworkTransform>();
    }
}
