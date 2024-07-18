using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Blood : Symbol
{
    [SerializeField] Condition condition;
    public override void DuplicationEffect(int n, GameObject monster)
    {
        AttackEnemy(monster, basicDmg);
        monster.GetComponent<Enemy>().AddEnemyCondition(condition, 1);
    }

    public override void PerfectEffect(GameObject monster)
    {
        AttackEnemy(monster, basicDmg);
        monster.GetComponent<Enemy>().AddEnemyCondition(condition, 5);
    }
}
