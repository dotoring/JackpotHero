using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RewardSymbolNode : MonoBehaviour
{
    public Symbol symbol;
    public BattleMgr battleMgr;

    [Header("UI")]
    public Image symbolImg;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dmgText;
    public TextMeshProUGUI pairText;
    public TextMeshProUGUI perfectText;

    Button selectBtn;

    void Start()
    {
        battleMgr = GameObject.Find("BattleMgr").GetComponent<BattleMgr>();
        selectBtn = GetComponent<Button>();

        symbolImg.sprite = symbol.sprite;
        nameText.text = symbol.symbolName;
        dmgText.text = symbol.basicDmg.ToString();
        pairText.text = symbol.symbolPairDescription;
        perfectText.text = symbol.symbolPerfectDescription;

        if (selectBtn != null)
        {
            selectBtn.onClick.AddListener(AddSymbol);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (battleMgr.GetRewardState())
        {
            selectBtn.interactable = false;
        }
    }

    protected virtual void AddSymbol()
    {
        GameMgr.Instance.AddPlayerSymbol(symbol);
        battleMgr.ChangeRewardState();
        Debug.Log(symbol.symbolName + " Ãß°¡");
    }
}
