using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_Weakness : Condition
{
    public override IEnumerator EffectCondition(object obj, int val)
    {
        //�÷��̾��� ���
        if (obj is BattleMgr battleMgr)
        {
            battleMgr.isPlayerWeak = true;
        }
        else if (obj is Enemy enemy)
        {
            enemy.isEnemyWeak = true;
        }

        yield return null;
    }
}
