using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CopyRotationDelayed : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Vector3 rotOffset;
    [SerializeField] private float rotationSpeed;
    
    void Update()
    {
        var rotationTarget = targetTransform.rotation.eulerAngles + rotOffset;
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotationTarget), rotationSpeed * Time.deltaTime);
    }
}
