using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using TMPro;
public class UI_Tweens : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] bool onSelectPunch;
    [SerializeField][ShowIf("onSelectPunch")] Vector3 punchStrength;
    [SerializeField][ShowIf("onSelectPunch")] float punchLength;

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
                item.DOColor(textDeselectedColor, colorChangeLength);
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (onSelectPunch)
        {
            transform.DOComplete();
            transform.DOPunchScale(punchStrength,punchLength);
        }

        if (changeTextColorOnSelect)
        {
            foreach (var item in affectedText)
            {
                item.DOColor(textSelectedColor, colorChangeLength);
            }
        }
    }
}
