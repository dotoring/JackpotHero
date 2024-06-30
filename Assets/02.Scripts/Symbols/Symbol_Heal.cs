using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Heal : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        GameMgr.Instance.HealPlayer(10);
    }

    public override void PerfectEffect(GameObject monster)
    {
        GameMgr.Instance.HealPlayer(30);
    }
}
