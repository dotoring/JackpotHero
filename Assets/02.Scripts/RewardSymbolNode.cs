using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RewardSymbolNode : MonoBehaviour
{
    public Symbol symbol;
    public BattleMgr battleMgr;

    [Header("UI")]
    public Image symbolImg;
    public Text nameText;
    public Text dmgText;
    public Text descriptionText;

    Button selectBtn;

    void Start()
    {
        battleMgr = GameObject.Find("BattleMgr").GetComponent<BattleMgr>();
        selectBtn = GetComponent<Button>();

        symbolImg.sprite = symbol.sprite;
        nameText.text = symbol.symbolName;
        dmgText.text = symbol.basicDmg.ToString();
        descriptionText.text = symbol.symbolDescription;

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

    void AddSymbol()
    {
        GameObject.Find("GameMgr").GetComponent<GameMgr>().AddPlayerSymbol(symbol);
        battleMgr.ChangeRewardState();
        Debug.Log(symbol.symbolName + " Ãß°¡");
    }
}
