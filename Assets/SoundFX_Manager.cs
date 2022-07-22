using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX_Manager : MonoSingleton<SoundFX_Manager>
{
    private List<FMOD.Studio.EventInstance> SoundInstance;


    public FMODUnity.EventReference[] musicEvent;

    public enum SoundType
    {
        GenericTextSpeech
    }

    void Start()
    {
        SoundInstance = new List<FMOD.Studio.EventInstance>();
        for (int i = 0; i < musicEvent.Length; i++)
        {
            SoundInstance.Add(FMODUnity.RuntimeManager.CreateInstance(musicEvent[i]));
        }

    }

    public void PlaySound(SoundType newMusic)
    {
        for (int i = 0; i < SoundInstance.Count; i++)
        {
            if (i == (int)newMusic)
            {
                SoundInstance[i].getPlaybackState(out var state);
                if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    SoundInstance[i].start();
                }
            }
        }
    }
}
