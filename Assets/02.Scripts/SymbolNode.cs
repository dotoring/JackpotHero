using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolNode : MonoBehaviour
{
    public Image symbolImg;
    public GameObject informationPanel;
    public Symbol symbol;

    protected virtual void Start()
    {
        symbolImg = GetComponent<Image>();
        informationPanel = GameObject.Find("MainCanvas").transform.Find("SymbolInformationPanel").gameObject;
        symbolImg.sprite = symbol.sprite;
    }

    void Update()
    {

    }

    private void OnMouseEnter()
    {
        informationPanel.SetActive(true);
        informationPanel.GetComponent<SymbolInformationPanel>().SetNameText(symbol.symbolName);
        informationPanel.GetComponent<SymbolInformationPanel>().SetDescriptionText(symbol.symbolDescription);
    }

    private void OnMouseExit()
    {
        informationPanel.SetActive(false);
    }
}
