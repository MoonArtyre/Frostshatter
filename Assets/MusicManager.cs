using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoSingleton<MusicManager>
{
    private List<FMOD.Studio.EventInstance> musicInstance;


    public FMODUnity.EventReference[] musicEvent;

    public enum MusicType 
    { 
        Adventure,
        MeloGuitar,
        Exploration,
        Hopeful,
        Loss,
        World
    }

    void Start()
    {
        musicInstance = new List<FMOD.Studio.EventInstance>();
        for (int i = 0; i < musicEvent.Length; i++)
        {
            musicInstance.Add(FMODUnity.RuntimeManager.CreateInstance(musicEvent[i]));
        }
        
    }

    public void PlayMusic(MusicType newMusic, float strength)
    {
        strength = Mathf.Clamp(strength,0,10);

        for (int i = 0; i < musicInstance.Count; i++)
        {
            if(i == (int)newMusic)
            {
                musicInstance[i].setParameterByName("Strength", strength);

                musicInstance[i].getPlaybackState(out var state);
                if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    musicInstance[i].start();
                }
            }
            else
            {
                musicInstance[i].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                musicInstance[i].setParameterByName("Strength", 0);

            }
        }
    }

    public void StopMusic(MusicType newMusic)
    {
        for (int i = 0; i < musicInstance.Count; i++)
        {
            if (i == (int)newMusic)
            {
                musicInstance[i].setParameterByName("Strength", 0);

                musicInstance[i].getPlaybackState(out var state);
                if (state == FMOD.Studio.PLAYBACK_STATE.PLAYING)
                {
                    musicInstance[i].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                }
            }
        }
    }
}
