using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private UnityEvent onSelect;
    [SerializeField] private UnityEvent onDeselect;

    private void Start()
    {
        List<Interaction> interactions = GetComponentsInChildren<Interaction>(true).ToList();

        if (interactions.Count > 0)
        {
            interactions[0].gameObject.SetActive(true);
        }
    }

    public void Interact()
    {
        Debug.Log("Interact!",this);
        
        onInteract.Invoke();

        Interaction interaction = FindActiveIteraction();
        if (interaction != null)
        {
            interaction.Execute();
        }
    }

    public void Select()
    {
        onSelect.Invoke();
    }

    public void Deselect()
    {
        onDeselect.Invoke();
    }

    private Interaction FindActiveIteraction()
    {
        return GetComponentInChildren<Interaction>();
    }
}
