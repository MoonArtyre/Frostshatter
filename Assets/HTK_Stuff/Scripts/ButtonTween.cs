using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ButtonTween : MonoBehaviour
{
    [SerializeField] private Color pressedColor = Color.yellow;
    [SerializeField] private float yMovement = -0.02f;

    [SerializeField][MinValue(0)] private float downDuration = 0.5f;

    [Header("In")] 
    [SerializeField] private Ease easeIn = DOTween.defaultEaseType;
    [SerializeField] [MinValue(0)] private float durationIn;

    [Header("out")]
    [SerializeField] private Ease easeOut = DOTween.defaultEaseType;
    [SerializeField] [MinValue(0)] private float durationOut;

    private MeshRenderer _meshRenderer;
    private Color _originColor;

    private Sequence _sequence;
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originColor = _meshRenderer.material.color;
    }

    public void PlayTween()
    {
        _sequence.Complete();
        
        var newSequence = DOTween.Sequence();

        newSequence.Append(transform.DOLocalMoveY(yMovement, durationIn).SetRelative().SetEase(easeIn));
        newSequence.Join(_meshRenderer.material.DOColor(pressedColor, durationIn).SetEase(Ease.Linear));
        newSequence.AppendInterval(downDuration);
        newSequence.Append(transform.DOLocalMoveY(-yMovement, durationOut).SetRelative().SetEase(easeOut));
        newSequence.Join(_meshRenderer.material.DOColor(_originColor, durationOut).SetEase(Ease.Linear));

        newSequence.Play();

        _sequence = newSequence;
    }

}
