using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
public class TriggerEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent onTriggerEnter;
    [SerializeField] private UnityEvent onTriggerExit;
    [SerializeField] private bool FilterByTag = false;
    [SerializeField][ShowIf("FilterByTag")] private string reactOn = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(reactOn) && FilterByTag) return;
        
        onTriggerEnter.Invoke();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag(reactOn) && FilterByTag) return;
        
        onTriggerExit.Invoke();
    }
}
