using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Reactor : MonoBehaviour
{
    [SerializeField] private List<State> conditions;
    [SerializeField] private UnityEvent onFulfilled;
    [SerializeField] private UnityEvent onUnfulfilled;

    private bool fulfilled = false;
    
    private void OnEnable()
    {
        OnStateChanged(GameState.Instance);
        GameState.stateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        GameState.stateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState gameState)
    {
        bool newFulfilled = gameState.CheckConditions(conditions);

        if (newFulfilled && !fulfilled)
        {
            onFulfilled.Invoke();
        }else if (!newFulfilled && fulfilled)
        {
            onUnfulfilled.Invoke();
        }

        fulfilled = newFulfilled;
    }
}
