using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    [SerializeField] private State state;

    [SerializeField] private UnityEvent onCollected;

    public void Collect()
    {
        onCollected.Invoke();
        GameState.Instance.Add(state);
        Destroy(gameObject);
    }
}
