using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Blood : Symbol
{
    public Condition condition;
    public override void DuplicationEffect(int n, GameObject monster)
    {
        AttackEnemy(monster, basicDmg);
        monster.GetComponent<Enemy>().AddEnemyCondition(condition, 1);
        GameObject.Find("BattleMgr").GetComponent<BattleMgr>().AddPlayerCondition(condition, 1);
    }

    public override void PerfectEffect(GameObject monster)
    {
        AttackEnemy(monster, basicDmg);
        monster.GetComponent<Enemy>().AddEnemyCondition(condition, 5);
        GameObject.Find("BattleMgr").GetComponent<BattleMgr>().AddPlayerCondition(condition, 1);
    }
}
