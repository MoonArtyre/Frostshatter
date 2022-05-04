using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    public static event Action<DialogBox> DialogContinued;
    
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Button continueButton;

    [SerializeField] private Transform selectionContainer;
    [SerializeField] private Button selectionButtonPrefab;
    
    private void Awake()
    {
        continueButton.onClick.AddListener(ContinueDialog);
    }

    public void DisplayDialogEntry(DialogueEntry entry)
    {
        speakerText.SetText(entry.speaker);
        dialogText.SetText(entry.text);
        
        ClearSelections();
        AddSelection(entry.selections);
    }

    private void ContinueDialog()
    {
        if (DialogContinued != null)
        {
            DialogContinued(this);
        }
    }

    private void ShowContinueButton(bool show)
    {
        continueButton.gameObject.SetActive(show);
    }

    private void ClearSelections()
    {
        foreach (Transform child in selectionContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void AddSelection(List<DialogSelection> dialogSelections)
    {
        GameObject firstButton = null;

        if (dialogSelections.Count == 0)
        {
            ShowContinueButton(true);
            firstButton = continueButton.gameObject;
        }
        else
        {
            ShowContinueButton(false);

            foreach (DialogSelection select in dialogSelections)
            {
                Button button = Instantiate(selectionButtonPrefab, selectionContainer);
                button.onClick.AddListener(select.onSelected.Invoke);

                if (select.nextDialog != null)
                {
                    button.onClick.AddListener(select.nextDialog.StartDialogue);
                }
                else
                {
                    button.onClick.AddListener(ContinueDialog);
                }

                if (firstButton == null)
                {
                    firstButton = button.gameObject;
                }
                
                button.GetComponentInChildren<TextMeshProUGUI>().SetText(select.selectionText);
            }
        }

        StartCoroutine(DelayedSelect(firstButton));
    }

    private IEnumerator DelayedSelect(GameObject selection)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(selection);
    }
}
