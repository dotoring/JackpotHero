using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_AssassinKnife : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        AttackEnemy(monster, basicDmg * 2);
    }

    public override void PerfectEffect(GameObject monster)
    {
        //해당 몬스터의 최대 체력만큼 피해
        AttackEnemy(monster, monster.GetComponent<Enemy>().enemyMaxHp);
    }
}
