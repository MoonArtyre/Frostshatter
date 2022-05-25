using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ChoiceButtonColorTween : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void Activate()
    {
        image.CrossFadeColor(Color.blue / 2 + Color.white / 2,0.2f,true,false);
    }

    public void Deactivate()
    {

        image.CrossFadeColor(Color.white,0.2f,true,false);
    }

    public void punch()
    {
        transform.DOPunchScale(-Vector3.one / 10,0.2f);
    }
}
