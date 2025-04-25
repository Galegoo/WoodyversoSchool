using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class NetworkChair : NetworkBehaviour
{
    
    [Networked]
    public bool OccupiedChair { get; set; }
    [Networked]
    public int playerID { get; set; }
    public void SitOnMe()
    {
        OccupiedChair = true;
    }

    public override void Spawned()
    {
        base.Spawned();
        if(OccupiedChair)
        {
            GetComponent<LocalClassChair>().occupied = true;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ChairOccupied(bool occupied, RpcInfo info = default)
    {
        if(!occupied && playerID == info.Source.PlayerId)
        {
            //SE SOU EU TENTANDO LEVANTAR
            playerID = 2000;
            LocalClassChair chair = GetComponent<LocalClassChair>();
            Debug.Log("Player GET UP on: " + info.Source.PlayerId.ToString());
            OccupiedChair = occupied;
            chair.occupied = occupied;
        }
        else if(occupied)
        {
            //SE ESTÃO TENTANDO SENTAR
            LocalClassChair chair = GetComponent<LocalClassChair>();
            playerID = info.Source.PlayerId;
            Debug.Log("Player SITTED on: " + info.Source.PlayerId.ToString());
            OccupiedChair = occupied;
            chair.occupied = occupied;
        }
    }
}
