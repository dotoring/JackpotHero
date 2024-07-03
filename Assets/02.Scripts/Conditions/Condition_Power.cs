using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_Power : Condition
{
    public override IEnumerator EffectCondition(object obj, int val)
    {
        //�÷��̾��� ���
        if (obj is BattleMgr battleMgr)
        {
            battleMgr.power += val;
        }
        else if (obj is Enemy enemy) //���� ���
        {
            enemy.enemyAdditionPower += val;
        }

        yield return null;
    }

    public override IEnumerator EndCondition(object obj)
    {
        //�÷��̾��� ���
        if (obj is BattleMgr battleMgr)
        {
            battleMgr.power = 0;
        }
        else if (obj is Enemy enemy) //���� ���
        {
            enemy.enemyAdditionPower = 0;
        }

        yield return null;
    }
}
