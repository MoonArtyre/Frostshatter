using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class SimpleTween : MonoBehaviour
{
    [SerializeField] private Transform affectedTransform;

    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    [SerializeField] private bool startAtEnd;
    [SerializeField][MinValue(0)] private float tweenDuration;

    [SerializeField] private Ease easeMode = DOTween.defaultEaseType;
    
    private bool _atEnd;


    private void Awake()
    {
        _atEnd = startAtEnd;
        affectedTransform.localPosition = _atEnd ? endPos : startPos;
    }

    public void TogglePosition()
    {
        if(_atEnd)
            GoToStart();
        else
            GoToEnd();
    }
    
    public void GoToEnd()
    {
        MoveTransform(true);
    }

    public void GoToStart()
    {
        MoveTransform(false);
    }

    private void MoveTransform(bool toEnd)
    {
        _atEnd = toEnd;

        float speed = (startPos - endPos).magnitude / tweenDuration;
        Vector3 targetPos = toEnd ? endPos : startPos;

        affectedTransform.DOKill();
        affectedTransform.DOLocalMove(targetPos, speed)
            .SetEase(easeMode)
            .SetSpeedBased()
            .OnComplete(OnTweenComplete);
        
    }

    private void OnTweenComplete()
    {
        
    }
}
