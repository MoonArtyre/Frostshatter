using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using TMPro;
public class DialogUIManager : MonoSingleton<DialogUIManager>
{
    public PlayerController playerController;

    [Header("UI References")]
    [SerializeField] private RectTransform portrait;
    [SerializeField] private RectTransform textArea;
    [SerializeField] private RectTransform nameArea;
    [SerializeField] private RectTransform choiceLine;
    [SerializeField] private List<RectTransform> choices = new List<RectTransform>();

    [Space]
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogText;
    private List<TextMeshProUGUI> choicesText = new List<TextMeshProUGUI>();

    [Header("Tweening")]
    [SerializeField] private Ease easeMode;
    [SerializeField] private float tweenTime;
    private Sequence currentSequnce;

    [Header("Dialog")]
    [SerializeField] private DialogScriptobject currentDialog = null;
    private List<DialogChoices> currentChoices = new List<DialogChoices>();
    private bool dialogOpen = false;
    public AudioSource bip;

    private void Start()
    {
        for (int i = 0; i < choices.Count; i++)
        {
            choicesText.Add(choices[i].GetComponentInChildren<TextMeshProUGUI>());
        }

        CloseChoices();
        CloseDialog();
        currentSequnce.Complete();
        currentSequnce.Kill();
    }

    private void OpenUI()
    {
        playerController.DisableInput();

        dialogOpen = true;

        currentSequnce.Complete();
        currentSequnce.Kill();

        var newSequence = DOTween.Sequence();

        newSequence.Append(portrait.DOAnchorPosY(50, tweenTime, true).SetDelay(0.2f).SetEase(easeMode));
        newSequence.Append(textArea.DOAnchorPosX(-475, tweenTime * 2, true).SetEase(easeMode));
        newSequence.Append(nameArea.DOAnchorPosY(0, tweenTime, true).SetEase(easeMode));

        currentSequnce = newSequence;
        newSequence.onComplete += () => { StartCoroutine(TypeText(currentDialog.dialogText)); };
        newSequence.Play();
    }

    private void CloseUI()
    {
        dialogOpen = false;

        currentSequnce.Complete();
        currentSequnce.Kill();

        var newSequence = DOTween.Sequence();


        newSequence.Append(nameArea.DOAnchorPosY(-100, tweenTime, true).SetEase(easeMode));
        newSequence.Append(textArea.DOAnchorPosX(-1850, tweenTime * 2, true).SetDelay(0.2f).SetEase(easeMode));
        newSequence.Append(portrait.DOAnchorPosY(-300, tweenTime, true).SetEase(easeMode));

        currentSequnce = newSequence;
        newSequence.onComplete += () => { dialogText.text = ""; };
        newSequence.Play();
    }

    private void OpenChoices(int choiceAmount)
    {
        currentSequnce.Complete();
        currentSequnce.Kill();

        var newSequence = DOTween.Sequence();

        newSequence.Append(choiceLine.DOAnchorPosY(0, tweenTime, true).SetEase(easeMode));

        for (int i = 0; i < choiceAmount; i++)
        {
            newSequence.Append(choices[i].DOAnchorPosX(300, tweenTime, true).SetEase(easeMode));
        }

        currentSequnce = newSequence;
        newSequence.Play();
    }

    private void CloseChoices()
    {

        currentSequnce.Complete();
        currentSequnce.Kill();

        var newSequence = DOTween.Sequence();

        for (int i = choices.Count - 1; i > -1; i--)
        {
            newSequence.Append(choices[i].DOAnchorPosX(-300, tweenTime, true).SetEase(easeMode));
        }

        newSequence.Append(choiceLine.DOAnchorPosY(-500, tweenTime, true).SetEase(easeMode));

        

        currentSequnce = newSequence;
        newSequence.Play();
    }

    private IEnumerator TypeText(string textToType = "Default text, bla bla bla \nhere is some text, I'm talking here \ngoodbye", float charPerSecond = 35)
    {
        dialogText.text = "";

        var charList = textToType.ToCharArray();

        for (int i = 0; i < charList.Length; i++)
        {
            dialogText.text += charList[i];
            bip.pitch = Random.Range(0.7f, 1.3f);
            bip.Play();
            if (charList[i] != '.' && charList[i] != '?' && charList[i] != '!' && charList[i] != ',')
                yield return new WaitForSeconds(1f / charPerSecond);
            else
                yield return new WaitForSeconds(0.5f);
        }

        DialogFinished();
    }

    public void StartDialogPrompt(DialogScriptobject newDialog)
    {

        dialogText.text = "";
        nameText.text = CharacterManager.Instance.GetCharacterName(newDialog.speaker);
        portraitImage.sprite = newDialog.speaker.image;

        currentDialog = newDialog;

        GameState.Instance.Add(newDialog.addToGameState);

        if(!dialogOpen)
            OpenUI();
        else
            StartCoroutine(TypeText(currentDialog.dialogText));

    }

    public void DialogFinished()
    {

        currentChoices = new List<DialogChoices>();

        if (currentDialog.choices.Count == 0)
        {
            choicesText[0].text = "Continue";
            OpenChoices(1);
            return;
        }

        var listToChoose = currentDialog.choices;

        for (int i = 0; i < listToChoose.Count; i++)
        {
            if (GameState.Instance.CheckConditions(listToChoose[i].unlockRequirements))
            {
                currentChoices.Add(listToChoose[i]);
            }
        }

        if (currentChoices.Count > 3)
        {
            var randomChoices = new List<DialogChoices>();
            var safety = 0;

            while (randomChoices.Count < 3 && safety < 100)
            {
                safety++;
                var choosenChoice = Random.Range(0, currentChoices.Count);
                randomChoices.Add(currentChoices[choosenChoice]);
                currentChoices.RemoveAt(choosenChoice);
            }

            if (safety >= 100)
                Debug.LogError("Safety reached 100, infinite Loop detected!");

            for (int i = 0; i < randomChoices.Count; i++)
            {
                choicesText[i].text = randomChoices[i].choiceName;
            }
        }
        else
        {
            for (int i = 0; i < currentChoices.Count; i++)
            {
                choicesText[i].text = currentChoices[i].choiceName;
            }
        }

        OpenChoices(currentChoices.Count);
    }

    public void MakeChoice(int choiceMade)
    {
        if (currentDialog.choices.Count > 0)
        {
            CloseChoices();

            if (currentChoices[choiceMade].choiceDialog != null)
                StartDialogPrompt(currentChoices[choiceMade].choiceDialog);
            else
                CloseDialog();
        }
        else
            ContinueDialog();

    }

    public void ContinueDialog()
    {
        if (currentDialog.defaultNextDialog != null)
        {
            StartDialogPrompt(currentDialog.defaultNextDialog);
            CloseChoices();
        }
        else
            CloseDialog();
    }

    private void CloseDialog()
    {
        playerController.EnableInput();
        currentSequnce.Complete();
        currentSequnce.Kill();
        dialogOpen = false;


        var newSequence = DOTween.Sequence();

        for (int i = choices.Count - 1; i > -1; i--)
        {
            newSequence.Append(choices[i].DOAnchorPosX(-300, tweenTime, true).SetEase(easeMode));
        }

        newSequence.Append(choiceLine.DOAnchorPosY(-500, tweenTime, true).SetEase(easeMode));
        newSequence.Append(nameArea.DOAnchorPosY(-100, tweenTime, true).SetEase(easeMode));
        newSequence.Append(textArea.DOAnchorPosX(-1850, tweenTime * 2, true).SetDelay(0.2f).SetEase(easeMode));
        newSequence.Append(portrait.DOAnchorPosY(-300, tweenTime, true).SetEase(easeMode));

        currentSequnce = newSequence;
        newSequence.onComplete += () => { dialogText.text = ""; };
        newSequence.Play();
    }





    /*
   public static Action<Dialog> DialogOpened;
   public static Action<Dialog> DialogClosed;
   
   [SerializeField] private DialogBox dialogBox;
   
   private Dialog _currentDialog;
   private int _currentIndex;

   private void OnEnable()
   {
      DialogBox.DialogContinued += OnDialogContinued;
   }

   private void OnDisable()
   {
      DialogBox.DialogContinued -= OnDialogContinued;
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
         DisplayDialogEntry(0);
      }
   }

   private void OpenDialog()
   {
      if (DialogOpened != null)
      {
         DialogOpened(_currentDialog);
      }

      dialogBox.DOShow();
      dialogBox.gameObject.SetActive(true);
   }

   private void CloseDialog()
   {
      Dialog finishedDialog = _currentDialog;
      finishedDialog.onDialogueEnd.Invoke();
      _currentDialog = null;
      _currentIndex = 0;

      dialogBox.DOHide().OnComplete(() => { dialogBox.gameObject.SetActive(false); });

      EventSystem.current.SetSelectedGameObject(null);

      if (DialogClosed != null)
      {
         DialogClosed(finishedDialog);
      }
   }

   public void DisplayDialogEntry(int index)
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

   private void OnDialogContinued(DialogBox _)
   {
      _currentIndex++;
      DisplayDialogEntry(_currentIndex);
   }*/
}
