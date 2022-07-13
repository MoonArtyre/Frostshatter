using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParameterByName : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;

    
    public FMODUnity.EventReference fmodEvent;

    [SerializeField]
    [Range(0, 10)]
    private float intensity;
    public Transform distance1, distance2;

    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
    }

    // Update is called once per frame
    void Update()
    {
        var distance = Vector3.Distance(distance1.position,distance2.position);
        instance.setParameterByName("BreachPower", Mathf.Clamp(intensity - distance,0,10));
    }
}
