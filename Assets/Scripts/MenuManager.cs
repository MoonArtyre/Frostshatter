using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using TMPro;
public class MenuManager : MonoBehaviour
{
    
    [SerializeField][BoxGroup("Tweening")] private RectTransform mainMenu, optionsMenu, menuContainer;
    [SerializeField][BoxGroup("Tweening")] private Vector3 closedPosition, menuPosition, optionPosition;
    [SerializeField][BoxGroup("Tweening")] private Vector2 closedSize, menuSize, optionSize;
    [SerializeField][BoxGroup("Tweening")] private float movementSpeed, scaleSpeed;
    [SerializeField][BoxGroup("Tweening")] private Ease movementEase, scaleEase;

    [SerializeField][BoxGroup("UI_References")] private TextMeshProUGUI masterPercentage, musicPercentage, effectsPercentage;
    [SerializeField][BoxGroup("UI_References")] FMOD.Studio.Bus masterBus, effectsBus, musicBus;
    private enum openUI
    {
        none,
        Menu,
        Options
    }
    private openUI currentUI;

    private GameInputs _input;


    private void Awake()
    {
        _input = new GameInputs();
        _input.UI.Escape.started += OnEscape;

        masterBus = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        musicBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        effectsBus = FMODUnity.RuntimeManager.GetBus("bus:/Master/Effects");

    }

    private void OnEnable()
    {
        _input.Enable();
    }
    private void OnDisable()
    {
        _input.Disable();
    }

    #region Tweening
    private void OnEscape(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.currentPlayState == GameManager.PlayState.Cutscene) return;

        switch (currentUI)
        {
            case openUI.none:
                OpenMenu();
                break;
            case openUI.Menu:
                CloseMenu();
                break;
            case openUI.Options:
                CloseOptions();
                break;
        }
    }


    public void OpenMenu()
    {
        currentUI = openUI.Menu;
        GameManager.Instance.ChangePlayState(GameManager.PlayState.Paused);

        Sequence newSeq = DOTween.Sequence();
        newSeq.Append(menuContainer.DOLocalMove(menuPosition,1f / movementSpeed).SetEase(movementEase).OnComplete(() => { mainMenu.gameObject.SetActive(true); }));
        newSeq.Append(menuContainer.DOSizeDelta(menuSize, 1f / scaleSpeed).SetEase(scaleEase));
        newSeq.SetUpdate(true);
        
        newSeq.Play();
        
    }

    public void CloseMenu()
    {
        currentUI = openUI.none;
        GameManager.Instance.ChangePlayState(GameManager.PlayState.Game);

        Sequence newSeq = DOTween.Sequence();
        newSeq.Append(menuContainer.DOSizeDelta(closedSize, 1f / scaleSpeed).SetEase(scaleEase));
        newSeq.Append(menuContainer.DOLocalMove(closedPosition, 1f / movementSpeed).SetEase(movementEase).OnComplete(() => { mainMenu.gameObject.SetActive(true); }));
        newSeq.SetUpdate(true);

        newSeq.Play();

    }

    public void OpenOptions()
    {
        currentUI = openUI.Options;


        Sequence newSeq = DOTween.Sequence();
        newSeq.Append(menuContainer.DOSizeDelta(closedSize, 1f / scaleSpeed).SetEase(scaleEase));
        newSeq.Append(menuContainer.DOLocalMove(optionPosition, 1f / movementSpeed).SetEase(movementEase).OnComplete(
            () => 
            { 
                mainMenu.gameObject.SetActive(false); 
                optionsMenu.gameObject.SetActive(true); 
            }));
        newSeq.Append(menuContainer.DOSizeDelta(optionSize, 1f / scaleSpeed).SetEase(scaleEase));
        newSeq.SetUpdate(true);

        newSeq.Play();
    }

    public void CloseOptions()
    {
        currentUI = openUI.Menu;

        Sequence newSeq = DOTween.Sequence();
        newSeq.Append(menuContainer.DOSizeDelta(closedSize, 1f / scaleSpeed).SetEase(scaleEase));
        newSeq.Append(menuContainer.DOLocalMove(menuPosition, 1f / movementSpeed).SetEase(movementEase).OnComplete(()
            => {
                mainMenu.gameObject.SetActive(true);
                optionsMenu.gameObject.SetActive(false);
            }));
        newSeq.Append(menuContainer.DOSizeDelta(menuSize, 1f / scaleSpeed).SetEase(scaleEase));
        newSeq.SetUpdate(true);

        newSeq.Play();
    }
    #endregion


    public void MasterVolume(float newVol)
    {
        var vol = newVol * 0.1f;
        masterBus.setVolume(vol);
        masterPercentage.text = (vol * 100f).ToString() + "%";
    }

    public void EffectsVolume(float newVol)
    {
        var vol = newVol * 0.1f;
        effectsBus.setVolume(vol);
        effectsPercentage.text = (vol * 100f).ToString() + "%";

    }
    public void MusicVolume(float newVol)
    {
        var vol = newVol * 0.1f;
        musicBus.setVolume(vol);
        musicPercentage.text = (vol * 100f).ToString() + "%";

    }

}
