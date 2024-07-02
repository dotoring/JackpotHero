using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_Power : Condition
{
    public override IEnumerator EffectCondition(object obj, int val)
    {
        //플레이어일 경우
        if (obj is BattleMgr battleMgr)
        {
            battleMgr.power += val;
        }
        else if (obj is Enemy enemy) //적일 경우
        {
            enemy.enemyAdditionPower += val;
        }

        yield return null;
    }
}
