using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoSingleton<GameState>
{
    public static event Action<GameState> stateChanged;
    
    [SerializeField] private List<State> states;

    public State Get(string id)
    {
        foreach (var state in states)
        {
            if (state.id == id)
            {
                return state;
            }
        }

        return null;
    }

    public void Add(string id, int amount, bool invokeEvent = true)
    {
        State state = Get(id);

        if (state == null)
        {
            State newState = new State
            {
                id = id,
                amount = amount
            };

            states.Add(newState);
        }
        else
        {
            state.amount += amount;
        }

        if (stateChanged != null && invokeEvent)
        {
            stateChanged(this);
        }
    }

    public void Add(State state, bool invokeEvent = true)
    {
        Add(state.id,state.amount, invokeEvent);
    }

    public void Add(List<State> states)
    {
        foreach (var state in states)
        {
            Add(state.id,state.amount, false);
        }

        if (stateChanged != null)
        {
            stateChanged(this);
        }
    }

    public bool CheckConditions(List<State> conditions)
    {
        foreach (var condition in conditions)
        {
            State state = Get(condition.id);
            int stateAmount = state != null ? state.amount : 0;
            if (stateAmount < condition.amount)
            {
                return false;
            } 
        }

        return true;
    }
}

[Serializable]
public class State
{
    public string id;

    public int amount;
}
