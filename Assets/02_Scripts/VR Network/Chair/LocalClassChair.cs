using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocalClassChair : MonoBehaviour
{
    public NetworkChair netChair;
    public bool occupied;
    Vector3 lastPosition;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    PlayerSit();

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //    PlayerGetUp();
    }
    public void PlayerSit()
    {
        HardwareRig localPlayer = FindObjectOfType<HardwareRig>();
        

        if (netChair.OccupiedChair == false && !localPlayer.chairSitted)
        {
            localPlayer.ChangeSittingStatus(this);
            lastPosition = localPlayer.transform.position;
            localPlayer.sitted = true;

            //TURN OFF PLAYER COLLIDERS
            if (localPlayer.GetComponent<CharacterController>())
            {
                localPlayer.GetComponent<CharacterController>().enabled = false;
            }

            //TURN OFF PLAYER MOVEMENT
            if (localPlayer.GetComponentInChildren<ContinuousMoveProviderBase>())
            {
                localPlayer.GetComponentInChildren<ContinuousMoveProviderBase>().enabled = false;
            }

            if (localPlayer.GetComponent<RigLocomotion>())
            {
                localPlayer.GetComponent<RigLocomotion>().enabled = false;
            }

            if (localPlayer.GetComponent<DesktopController>())
            {
                localPlayer.GetComponent<DesktopController>().enabled = false;
            }

            //ROTATE PLAYER FORWARD


            localPlayer.transform.position = transform.position; //POSITION PLAYER ON CHAIR POSITION
            localPlayer.transform.rotation = new Quaternion(0, 0, 0, 90);
            localPlayer.headset.transform.rotation = new Quaternion(0, 0, 0, 90);

            netChair.RPC_ChairOccupied(true);
        }
        

    }

    public void PlayerGetUp()
    {
        HardwareRig localPlayer = FindObjectOfType<HardwareRig>();

        if (occupied)
        {
            localPlayer.ChangeSittingStatus(null);
            netChair.RPC_ChairOccupied(false);

            localPlayer.sitted = false;

            //POSITION PLAYER ON CORRIDOR
            localPlayer.transform.position = lastPosition;

            //TURN ON PLAYER COLLIDERS
            if (localPlayer.GetComponent<CharacterController>())
            {
                localPlayer.GetComponent<CharacterController>().enabled = true;
            }

            //TURN ON PLAYER MOVEMENT
            if (localPlayer.GetComponentInChildren<ContinuousMoveProviderBase>())
            {
                localPlayer.GetComponentInChildren<ContinuousMoveProviderBase>().enabled = true;
            }

            if (localPlayer.GetComponent<RigLocomotion>())
            {
                localPlayer.GetComponent<RigLocomotion>().enabled = true;
            }
            if (localPlayer.GetComponent<DesktopController>())
            {
                localPlayer.GetComponent<DesktopController>().enabled = true;
            }
        }


    }
}
