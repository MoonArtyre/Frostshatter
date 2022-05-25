using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    /*
    public static event Action<DialogBox> DialogContinued;
    
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private Button continueButton;

    [SerializeField] private Transform selectionContainer;
    [SerializeField] private Button selectionButtonPrefab;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Sequence _slideInOutSequence;
    
    private void Awake()
    {
        continueButton.onClick.AddListener(ContinueDialog);
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
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

    public Tween DOShow()
    {
        float height = _rectTransform.rect.height;

        _slideInOutSequence = DOTween.Sequence()
            .Append(DOMove(Vector2.zero).From(new Vector2(0, -height)))
            .Join(DOFade(1).From(0));

        return _slideInOutSequence;
    }

    public Tween DOHide()
    {
        float height = _rectTransform.rect.height;

        _slideInOutSequence = DOTween.Sequence()
            .Append(DOMove(new Vector2(0, -height)).From(Vector2.zero))
            .Join(DOFade(0).From(1));

        return _slideInOutSequence;
    }

    private TweenerCore<Vector2, Vector2, VectorOptions> DOMove(Vector2 targetPos)
    {
        return _rectTransform.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.InOutBack);
    }

    private TweenerCore<float, float, FloatOptions> DOFade(float targetAlpha)
    {
        return _canvasGroup.DOFade(targetAlpha, 0.5f).SetEase(Ease.InOutBack);
    }
    */
}
