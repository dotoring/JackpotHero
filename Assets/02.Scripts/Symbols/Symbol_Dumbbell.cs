using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Dumbbell : Symbol
{
    [SerializeField] Condition condition;
    public override void DuplicationEffect(int n, GameObject monster)
    {
        AttackEnemy(monster, basicDmg);
        GameObject.Find("BattleMgr").GetComponent<BattleMgr>().AddPlayerCondition(condition, 1);
    }

    public override void PerfectEffect(GameObject monster)
    {
        AttackEnemy(monster, basicDmg);
        GameObject.Find("BattleMgr").GetComponent<BattleMgr>().AddPlayerCondition(condition, 5);
    }
}
