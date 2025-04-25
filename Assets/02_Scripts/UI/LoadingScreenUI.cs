using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LoadingScreenUI : MonoBehaviour
{
    [SerializeField] private RectTransform[] dots;
    [SerializeField] private float upAmount = 0.2f;
    [SerializeField] private float upDuration = 0.2f;
    [SerializeField] private float sequenceDuration = 0.7f;

    private void Awake()
    {
        StartLoadingAnimation();
    }
    void StartLoadingAnimation()
    {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(dots[0].DOPunchAnchorPos(new Vector2(0, upAmount), upDuration));
        mySequence.Append(dots[1].DOPunchAnchorPos(new Vector2(0, upAmount), upDuration));
        mySequence.Append(dots[2].DOPunchAnchorPos(new Vector2(0, upAmount), upDuration));

        mySequence.PrependInterval(sequenceDuration);

        mySequence.SetLoops(-1);

        mySequence.Play();
    }

    void StopLoadingAnimation()
    {

    }
}
