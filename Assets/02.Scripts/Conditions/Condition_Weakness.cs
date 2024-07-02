using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_Weakness : Condition
{
    public override IEnumerator EffectCondition(object obj, int val)
    {
        //플레이어일 경우
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
