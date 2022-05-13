using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPositionDelayed : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float moveSpeed;
    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - targetTransform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,targetTransform.position + _offset, moveSpeed * Time.deltaTime);
    }
}
