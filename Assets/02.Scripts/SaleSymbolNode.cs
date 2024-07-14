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
    public TextMeshProUGUI priceText;

    private void Start()
    {
        selectBtn = GetComponent<Button>();

        symbolImg.sprite = symbol.sprite;
        nameText.text = symbol.symbolName;
        dmgText.text = symbol.basicDmg.ToString();
        pairText.text = symbol.symbolPairDescription;
        perfectText.text = symbol.symbolPerfectDescription;

        int price;
        //<<<vip카드 효과
        if(GameMgr.Instance.vipCard)
        {
            price = (int)(symbol.symbolPrice * 0.8f);
            //vip카드 효과>>>
        }
        else
        {
            price = symbol.symbolPrice;
        }

        priceText.text = price.ToString();

        if (selectBtn != null)
        {
            selectBtn.onClick.AddListener(BuySymbol);
        }
    }

    void BuySymbol()
    {
        if(GameMgr.Instance.GetGoldAmount() >= symbol.symbolPrice)
        {
            GameMgr.Instance.AddPlayerSymbol(symbol);
            GameMgr.Instance.useGold(symbol.symbolPrice);

            GameObject.Find("UIMgr").GetComponent<UIMgr>().RefreshOwnSymbols();

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("돈 부족");
        }
    }
}
