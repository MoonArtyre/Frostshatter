using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
