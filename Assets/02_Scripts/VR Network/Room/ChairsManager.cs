using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ChairsManager : MonoBehaviour
{
    [SerializeField] private bool automaticSit;
    [SerializeField] private List<LocalClassChair> chairs = new List<LocalClassChair>();

    public void FindPlaceToSit(HardwareRig localPlayer)
    {
        if (!automaticSit) return;

        LocalClassChair[] availableChairs = chairs.Where(c => !c.occupied).ToArray();
        LocalClassChair chair = availableChairs[Random.Range(0, availableChairs.Length)];

        chair.PlayerSit();
    }
}
