using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolInformationPanel : MonoBehaviour
{
    [SerializeField] Text symbolName;
    [SerializeField] Text symbolDescription;

    void Start()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    void Update()
    {
        
    }

    public void SetNameText(string nameString)
    {
        symbolName.text = nameString;
    }

    public void SetDescriptionText(string descString)
    {
        symbolDescription.text = descString;
    }
}
