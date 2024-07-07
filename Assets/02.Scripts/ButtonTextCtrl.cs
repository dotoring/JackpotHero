using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTextCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI text;

    private void Update()
    {
        Button button = GetComponent<Button>();
        if(!button.IsActive())
        {
            text.transform.localPosition = new Vector3(0, -21.125f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        text.transform.localPosition = new Vector3(0, -21.125f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        text.transform.localPosition = new Vector3(0, 7f);
    }
}
