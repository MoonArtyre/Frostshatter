using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
public class GameManager : MonoSingleton<GameManager>
{
    
    public enum PlayState
    {
        Game,
        Cutscene,
        Paused
    }

    public PlayState currentPlayState;
    public Action<PlayState> onPlayStateChange;
    public CinemachineVirtualCamera currentCutsceneCam;
    public void ChangePlayState(PlayState newPlayState)
    {
        currentPlayState = newPlayState;
        onPlayStateChange.Invoke(newPlayState);

        switch (newPlayState)
        {
            case PlayState.Game:
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
                break;

            case PlayState.Cutscene:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 1;
                break;

            case PlayState.Paused:
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                break;
        }
    }

    public void SetCutsceneCam(CinemachineVirtualCamera newCam)
    {
        currentCutsceneCam = newCam;
        currentCutsceneCam.Priority = 15;
    }

    public void Update()
    {
        if(currentPlayState == PlayState.Game)
        {
            if(currentCutsceneCam != null)
                currentCutsceneCam.Priority = 0;
        }
    }
}
