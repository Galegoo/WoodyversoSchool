using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PointerEnlager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleMultiplier;
    [SerializeField] private float timeToScale;
    Vector3 scaleCache;

    private void Awake()
    {
        scaleCache = transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOComplete();
        transform.DOScale(scaleCache * scaleMultiplier, timeToScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOComplete();
        transform.DOScale(scaleCache, timeToScale);
    }
}
