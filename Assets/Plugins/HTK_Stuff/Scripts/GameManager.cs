using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoSingleton<GameManager>
{
    /*
    private Old_PlayerController _player;

    private DialogUIManager _dialogUIManager;

    private void OnEnable()
    {
        DialogUIManager.DialogClosed += EndDialog;

    }

    private void OnDisable()
    {
        DialogUIManager.DialogClosed -= EndDialog;

    }

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Old_PlayerController>();
        
        if(_player == null) Debug.LogError("No Player could be found");
        _dialogUIManager = DialogUIManager.Instance;
        EnterPlayMode();
    }

    private void EnterPlayMode()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _player.EnableInput();
    }

    public void StartDialogue(Dialog dialog)
    {
        Cursor.lockState = CursorLockMode.None;
        _dialogUIManager.StartDialog(dialog);
        _player.DisableInput();
    }

    private void EndDialog(Dialog _)
    {
        EnterPlayMode();
    }*/

    public enum PlayState
    {
        Game,
        Cutscene,
        Paused
    }

    public PlayState currentPlayState;
    public Action<PlayState> onPlayStateChange;

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
}
