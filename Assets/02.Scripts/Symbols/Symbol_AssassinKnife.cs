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
        //�ش� ������ �ִ� ü�¸�ŭ ����
        AttackEnemy(monster, monster.GetComponent<Enemy>().enemyMaxHp);
    }
}
