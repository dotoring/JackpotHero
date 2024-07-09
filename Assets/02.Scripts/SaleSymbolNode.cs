using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaleSymbolNode : MonoBehaviour
{
    public Symbol symbol;

    [Header("UI")]
    public Image symbolImg;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dmgText;
    public TextMeshProUGUI pairText;
    public TextMeshProUGUI perfectText;

    Button selectBtn;
    public TextMeshProUGUI price;

    private void Start()
    {
        selectBtn = GetComponent<Button>();

        symbolImg.sprite = symbol.sprite;
        nameText.text = symbol.symbolName;
        dmgText.text = symbol.basicDmg.ToString();
        pairText.text = symbol.symbolPairDescription;
        perfectText.text = symbol.symbolPerfectDescription;
        price.text = symbol.symbolPrice.ToString();

        if (selectBtn != null)
        {
            selectBtn.onClick.AddListener(AddSymbol);
        }
    }

    void AddSymbol()
    {
        if(GameMgr.Instance.GetGoldAmount() >= symbol.symbolPrice)
        {
            GameMgr.Instance.AddPlayerSymbol(symbol);
            GameMgr.Instance.useGold(symbol.symbolPrice);

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("µ∑ ∫Œ¡∑");
        }
    }
}
