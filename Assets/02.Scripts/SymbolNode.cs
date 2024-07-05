using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolNode : MonoBehaviour
{
    public Image symbolImg;
    public Symbol symbol;

    void Start()
    {
        symbolImg.sprite = symbol.sprite;
    }
}
