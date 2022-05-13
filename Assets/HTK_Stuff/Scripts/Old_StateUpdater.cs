using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Old_StateUpdater : MonoBehaviour
{
    [SerializeField] private List<State> stateUpdate;

    private void OnValidate()
    {
        if (stateUpdate == null)
        {
            return;
        }

        foreach (var state in stateUpdate)
        {
            if (string.IsNullOrWhiteSpace(state.id) && state.amount == 0)
            {
                state.id = "<ID>";
                state.amount = 1;
            }
        }
    }

    public void UpdateState()
    {
        FindObjectOfType<GameState>().Add(stateUpdate);
    }
}
