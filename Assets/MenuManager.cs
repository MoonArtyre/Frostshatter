using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;
public class MenuManager : MonoBehaviour
{

    [SerializeField] private RectTransform mainMenu, optionsMenu, menuContainer;
    [SerializeField] private Vector3 closedPosition, menuPosition, optionPosition;
    [SerializeField] private Vector2 closedSize, menuSize, optionSize;
    [SerializeField] private float movementSpeed, scaleSpeed;
    [SerializeField] private Ease movementEase, scaleEase;
    [Button]
    public void OpenMenu()
    {
        Sequence newSeq = DOTween.Sequence();
        newSeq.Append(menuContainer.DOLocalMove(menuPosition,1f / movementSpeed).SetEase(movementEase).OnComplete(() => { mainMenu.gameObject.SetActive(true); }));
        newSeq.Append(menuContainer.DOSizeDelta(menuSize, 1f / scaleSpeed).SetEase(scaleEase));

        newSeq.Play();
        
    }

    [Button]
    public void CloseMenu()
    {
        Sequence newSeq = DOTween.Sequence();
        newSeq.Append(menuContainer.DOSizeDelta(closedSize, 1f / scaleSpeed).SetEase(scaleEase));
        newSeq.Append(menuContainer.DOLocalMove(closedPosition, 1f / movementSpeed).SetEase(movementEase).OnComplete(() => { mainMenu.gameObject.SetActive(true); }));

        newSeq.Play();

    }

    [Button]
    public void OpenOptions()
    {
        Sequence newSeq = DOTween.Sequence();
        newSeq.Append(menuContainer.DOSizeDelta(closedSize, 1f / scaleSpeed).SetEase(scaleEase));
        newSeq.Append(menuContainer.DOLocalMove(optionPosition, 1f / movementSpeed).SetEase(movementEase).OnComplete(() 
            => { 
                mainMenu.gameObject.SetActive(false); 
                optionsMenu.gameObject.SetActive(true); 
            }));
        newSeq.Append(menuContainer.DOSizeDelta(optionSize, 1f / scaleSpeed).SetEase(scaleEase));

        newSeq.Play();
    }

    [Button]
    public void CloseOptions()
    {
        Sequence newSeq = DOTween.Sequence();
        newSeq.Append(menuContainer.DOSizeDelta(closedSize, 1f / scaleSpeed).SetEase(scaleEase));
        newSeq.Append(menuContainer.DOLocalMove(menuPosition, 1f / movementSpeed).SetEase(movementEase).OnComplete(()
            => {
                mainMenu.gameObject.SetActive(true);
                optionsMenu.gameObject.SetActive(false);
            }));
        newSeq.Append(menuContainer.DOSizeDelta(menuSize, 1f / scaleSpeed).SetEase(scaleEase));

        newSeq.Play();
    }
}
