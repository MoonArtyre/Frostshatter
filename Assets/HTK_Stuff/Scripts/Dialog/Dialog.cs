using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    public List<DialogueEntry> entries;

    public UnityEvent onDialogueEnd;

    public void StartDialogue()
    {
        if (entries.Count == 0)
        {
            Debug.LogWarning("Dialogue has no entries", this);
            return;
        }
        
        GameManager.Instance.StartDialogue(this);
    }
}

[Serializable]
public class DialogueEntry
{
    public string speaker;

    [TextArea(3,4)] public string text;

    public List<DialogSelection> selections;
}

[Serializable]
public class DialogSelection
{
    public string selectionText;
    public UnityEvent onSelected;
    public Dialog nextDialog;
}
