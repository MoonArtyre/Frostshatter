using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUIManager : MonoSingleton<DialogUIManager>
{
   public static Action<Dialog> DialogOpened;
   public static Action<Dialog> DialogClosed;
   
   [SerializeField] private DialogBox dialogBox;
   
   private Dialog _currentDialog;
   private int _currentIndex;

   private void OnEnable()
   {
      DialogBox.DialogContinued += AdvanceDialogContinued;
   }

   private void OnDisable()
   {
      DialogBox.DialogContinued -= AdvanceDialogContinued;
   }

   private void Start()
   {
      dialogBox.gameObject.SetActive(false);
   }

   public void StartDialog(Dialog dialog)
   {
      bool alreadyOpen = _currentDialog != null;

      if (alreadyOpen)
      {
         _currentDialog.onDialogueEnd.Invoke();
      }
         
      
      _currentDialog = dialog;

      if (!alreadyOpen)
      {
         OpenDialog();
         DisplayDialog(0);
      }
   }

   private void OpenDialog()
   {
      if (DialogOpened != null)
      {
         DialogOpened(_currentDialog);
      }
      
      dialogBox.gameObject.SetActive(true);
   }

   private void CloseDialog()
   {
      Dialog finishedDialog = _currentDialog;
      finishedDialog.onDialogueEnd.Invoke();
      _currentDialog = null;
      _currentIndex = 0;
      dialogBox.gameObject.SetActive(false);

      if (DialogClosed != null)
      {
         DialogClosed(finishedDialog);
      }
   }

   public void DisplayDialog(int index)
   {
      if ( index >= _currentDialog.entries.Count)
      {
         CloseDialog();
         return;
      }
      
      _currentIndex = index;
      DialogueEntry entry = _currentDialog.entries[_currentIndex];
      dialogBox.DisplayDialogEntry(entry);
   }

   private void AdvanceDialogContinued(DialogBox _)
   {
      _currentIndex++;
      DisplayDialog(_currentIndex);
   }
}
