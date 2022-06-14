using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParameterByName : MonoBehaviour
{
    private FMOD.Studio.EventInstance instance;

    
    public FMODUnity.EventReference fmodEvent;

    [SerializeField]
    [Range(0, 100)]
    private float intensity;

    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
    }

    // Update is called once per frame
    void Update()
    {
        instance.setParameterByName("Intensity", intensity);
    }
}
