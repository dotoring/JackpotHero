using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTextCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    enum Usage
    {
        Rest,
        Shop
    }
    public TextMeshProUGUI text;
    [SerializeField] Usage buttonUsage;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(buttonUsage == Usage.Rest)
        {
            text.transform.localPosition = new Vector3(0, -21.125f);
        }
        else if(buttonUsage == Usage.Shop)
        {
            text.transform.localPosition = new Vector3(0, -11.75f);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        text.transform.localPosition = new Vector3(0, 7f);
    }
}

