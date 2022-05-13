using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CopyRotationDelayed : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float rotationSpeed;
    
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation,rotationSpeed * Time.deltaTime);
    }
}
