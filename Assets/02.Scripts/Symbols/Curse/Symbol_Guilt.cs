using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Guilt : Symbol
{
    [SerializeField] Condition condition;
    public override void DuplicationEffect(int n, GameObject monster)
    {
        GameObject.Find("BattleMgr").GetComponent<BattleMgr>().AddPlayerCondition(condition, 1);
    }

    public override void PerfectEffect(GameObject monster)
    {
        GameMgr.Instance.RemovePlayerSymbol(this);
    }
}
