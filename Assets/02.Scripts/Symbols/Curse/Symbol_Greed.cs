using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Greed : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        GameMgr.Instance.UseGold(10);
    }

    public override void PerfectEffect(GameObject monster)
    {
        GameMgr.Instance.RemovePlayerSymbol(this);
    }
}
