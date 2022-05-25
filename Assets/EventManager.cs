using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public List<StateEvent> stateEvents;

    public void Awake()
    {
        GameState.stateChanged += onStateUpdate;
    }

    public void Start()
    {
        onStateUpdate(GameState.Instance);

    }

    public void onStateUpdate(GameState newState)
    {
        foreach (var stateEvent in stateEvents)
        {
            if(!stateEvent.stateActive && GameState.Instance.CheckConditions(stateEvent.triggerRequirements))
            {
                stateEvent.stateActive = true;
                stateEvent.onStateActivate.Invoke();
            }
        }
    }
}

[System.Serializable]
public class StateEvent
{
    public List<State> triggerRequirements;
    public bool stateActive;

    public UnityEvent onStateActivate;
}
