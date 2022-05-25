using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private List<State> stateChanges;
    [SerializeField] private Interaction nextInteraction;
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        List<Interaction> interactions = transform.parent.GetComponentsInChildren<Interaction>(true).ToList();

        foreach (var interaction in interactions)
        {
            if (interaction != this)
            {

                interaction.gameObject.SetActive(false);
            }
        }
        
        
    }

    public void Execute()
    {
        onInteract.Invoke();

        GameState.Instance.Add(stateChanges);

        if(nextInteraction != null)
            nextInteraction.gameObject.SetActive(true);
    }
}
