using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Highlight : MonoBehaviour
{
    [SerializeField] private List<Renderer> renderers;

    [Min(0)][SerializeField] private float highlightDuration = 0.5f;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void OnDestroy()
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.material.DOKill();
            Destroy(renderer.material);
        }
    }

    public void SetHighlight(bool highlight)
    {
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            mat.DOKill();
            mat.DOFloat(highlight ? 1 : 0, "_Highlight_Intensitiy",1 / highlightDuration).SetSpeedBased();
        }
    }
}
