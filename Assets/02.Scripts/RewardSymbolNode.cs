using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardSymbolNode : SymbolNode
{
    public Button selectBtn;

    public BattleMgr battleMgr;

    protected override void Start()
    {
        base.Start();

        battleMgr = GameObject.Find("BattleMgr").GetComponent<BattleMgr>();
        if (selectBtn != null)
        {
            selectBtn.onClick.AddListener(AddSymbol);
        }
    }

    void Update()
    {
        if(battleMgr.GetRewardState())
        {
            selectBtn.interactable = false;
        }
    }

    //심볼 리스트에 추가
    void AddSymbol()
    {
        GameObject.Find("GameMgr").GetComponent<GameMgr>().AddPlayerSymbol(symbol);
        battleMgr.ChangeRewardState();
        Debug.Log(symbol.symbolName + " 추가");
    }
}
