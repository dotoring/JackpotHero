using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Shield : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        AttackEnemy(monster, basicDmg);
        GameMgr.Instance.AddBarrier(5);
    }

    public override void PerfectEffect(GameObject monster)
    {
        AttackEnemy(monster, basicDmg);
        GameMgr.Instance.AddBarrier(15);
    }
}
