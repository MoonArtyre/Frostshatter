using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SelectOnHover : MonoBehaviour, IPointerEnterHandler
{
    private Selectable mySelectable;
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(mySelectable.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        mySelectable = GetComponent<Selectable>();
    }

}
