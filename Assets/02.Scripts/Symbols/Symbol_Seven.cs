using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Seven : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        AttackEnemy(monster, basicDmg + 5);
        GameMgr.Instance.earnGold(50);
    }

    public override void PerfectEffect(GameObject monster)
    {
        AttackEnemy(monster, basicDmg + 10);
        GameMgr.Instance.earnGold(500);
    }
}
