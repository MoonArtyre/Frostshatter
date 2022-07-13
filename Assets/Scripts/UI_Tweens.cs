using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using TMPro;
public class UI_Tweens : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerDownHandler
{
    [SerializeField] bool onSelectPunch;
    [SerializeField][ShowIf("onSelectPunch")] Vector3 selectPunchStrength;
    [SerializeField][ShowIf("onSelectPunch")] float selectPunchLength;


    [SerializeField] bool onClickPunch; 
    [SerializeField][ShowIf("onClickPunch")] Vector3 clickPunchStrength;
    [SerializeField][ShowIf("onClickPunch")] float clickPunchLength;

    [Space]
    [SerializeField] bool changeTextColorOnSelect;
    [SerializeField][ShowIf("changeTextColorOnSelect")] TextMeshProUGUI[] affectedText;
    [SerializeField][ShowIf("changeTextColorOnSelect")] Color textSelectedColor, textDeselectedColor;
    [SerializeField][ShowIf("changeTextColorOnSelect")] float colorChangeLength;


    public void OnDeselect(BaseEventData eventData)
    {
        if (changeTextColorOnSelect)
        {
            foreach (var item in affectedText)
            {
                item.DOColor(textDeselectedColor, colorChangeLength).SetUpdate(true);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onClickPunch)
        {
            transform.DOComplete();
            transform.DOPunchScale(clickPunchStrength, clickPunchLength).SetUpdate(true);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (onSelectPunch)
        {
            transform.DOComplete();
            transform.DOPunchScale(selectPunchStrength, selectPunchLength).SetUpdate(true);
        }

        if (changeTextColorOnSelect)
        {
            foreach (var item in affectedText)
            {
                item.DOColor(textSelectedColor, colorChangeLength).SetUpdate(true);
            }
        }
    }
}
