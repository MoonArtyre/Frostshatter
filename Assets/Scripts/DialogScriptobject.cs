using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "Dialog", menuName = "DialogPrompt")]
public class DialogScriptobject : ScriptableObject
{
    [Header("Dialog")]
    public CharacterObject speaker;
    [TextArea(3,10)]public string dialogText;

    [Header("Choices")]
    [InlineEditor]public DialogScriptobject defaultNextDialog;
    public List<DialogChoices> choices;

    [Header("GameStates")]
    public List<State> addToGameState;
}

[System.Serializable]
public class DialogChoices
{
    public DialogScriptobject choiceDialog;
    public string choiceName;
    public List<State> unlockRequirements;
}