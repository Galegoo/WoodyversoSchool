using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PopCanvas : MonoBehaviour
{

    public bool open;
    [SerializeField] private float timeToPop;
    [SerializeField] private Ease easeType;
    Vector3 cacheScale;

    private void Awake()
    {
        cacheScale = transform.localScale;
        if(!open)
        {
            transform.localScale = Vector3.zero;
        }
    }
    public void PopIn()
    {
        transform.DOComplete();
        open = true;
        transform.DOScale(cacheScale, timeToPop).SetEase(easeType);
    }

    public void PopOut()
    {
        transform.DOComplete();
        open = false;
        transform.DOScale(Vector3.zero, timeToPop).SetEase(easeType);
    }

    public void DynamicPop()
    {
        if(open)
        {
            PopOut();
        }
        else
        {
            PopIn();
        }
    }
}
