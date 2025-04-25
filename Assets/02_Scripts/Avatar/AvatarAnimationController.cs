using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class AvatarAnimationController : MonoBehaviour
{
    [SerializeField] private InputActionReference move;

    [SerializeField] private Animator animator;
    [SerializeField] private TwoBoneIKConstraint leftHand;
    [SerializeField] private TwoBoneIKConstraint rightHand;

    public bool sentado = false; // link com o lowerbodyavataranim
    public bool avatarMaskOff;
    public void AnimateLegs(Vector2 directionMove)
    {
        //Vector2 inputMov = move.action.ReadValue<Vector2>();
        //bool isMovingForward = move.action.ReadValue<Vector2>().y > 0;

        animator.SetFloat("directionX", directionMove.x);
        animator.SetFloat("directionY", directionMove.y);

        //if (isMovingForward)
        //{
        //    animator.SetBool("isWalking", true);
        //    animator.SetFloat("animSpeed", 1);
        //}
        //else
        //{
        //    animator.SetBool("isWalking", true);
        //    animator.SetFloat("animSpeed", -1);
        //}

        if(directionMove == Vector2.zero)
        {
            StopAnimateLegs();
        }
    }

    private void StopAnimateLegs()
    {
        animator.SetBool("isWalking", false);
        animator.SetFloat("animSpeed", 0);
        animator.SetFloat("directionX", 0);
        animator.SetFloat("directionY", 0);
    }

    public void TurnOffAvatarMask()
    {
        if(!avatarMaskOff)
        {
            avatarMaskOff = true;
            Debug.Log("Turning Off");
            animator.SetLayerWeight(1, 1f);
            leftHand.data.targetPositionWeight = 0f;
            leftHand.data.targetRotationWeight = 0f;
            rightHand.data.targetPositionWeight = 0f;
            rightHand.data.targetRotationWeight = 0f;
        }

    }


    public void Sit()
    {
        animator.SetBool("Sitted", true);
    }

    public void GetUp()
    {
        animator.SetBool("Sitted", false);
    }

}
