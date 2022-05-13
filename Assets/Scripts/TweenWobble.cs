using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TweenWobble : MonoBehaviour
{
    [Header("AffectedAxis")] 
    [SerializeField] private bool xAxis;
    [SerializeField] private bool yAxis, zAxis;
    
    [Header("WobbleSettings")]
    [SerializeField] private Vector3 wobbleStrength;
    [SerializeField] private Vector3 wobbleSpeeds;

    [Header("Randomness")] 
    [SerializeField] private Vector3 wobbleStrengthRandomness;
    [SerializeField] private Vector3 wobbleSpeedRandomness;
    
    private Transform _transform;
    private float _timer;
    private Vector3 _startPos;

    private void Awake()
    {
        _transform = transform;
        _timer = 0;
        _startPos = _transform.localPosition;

        wobbleStrength += new Vector3(
            Random.Range(-wobbleStrengthRandomness.x, wobbleStrengthRandomness.x), 
            Random.Range(-wobbleStrengthRandomness.y, wobbleStrengthRandomness.y), 
            Random.Range(-wobbleStrengthRandomness.z, wobbleStrengthRandomness.z));
        
        wobbleSpeeds += new Vector3(
            Random.Range(-wobbleSpeedRandomness.x, wobbleSpeedRandomness.x), 
            Random.Range(-wobbleSpeedRandomness.y, wobbleSpeedRandomness.y), 
            Random.Range(-wobbleSpeedRandomness.z, wobbleSpeedRandomness.z));
    }

    void Update()
    {
        Vector3 _offset = new Vector3();
        _timer += Time.deltaTime;
        
        if (xAxis)
        {
            _offset.x = math.sin(wobbleSpeeds.x * _timer) * wobbleStrength.x;
        }
        
        if (yAxis)
        {
            _offset.y = math.sin(wobbleSpeeds.y * _timer) * wobbleStrength.y;
        }
        
        if (zAxis)
        {
            _offset.z = math.sin(wobbleSpeeds.z * _timer) * wobbleStrength.z;
        }

        _transform.localPosition = _offset + _startPos;
    }
}
