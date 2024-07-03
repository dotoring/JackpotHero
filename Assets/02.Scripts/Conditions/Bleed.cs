using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : Condition
{
    public override IEnumerator EffectCondition(object obj, int val)
    {
        //플레이어일 경우
        if (obj is BattleMgr battleMgr)
        {
            GameMgr.Instance.TakeDmg(val);
        }
        else if (obj is Enemy enemy)
        {
            enemy.TakeDmgEnemy(val);
        }

        yield return null;
    }

    public override IEnumerator EndCondition(object obj)
    {
        //플레이어일 경우
        if (obj is BattleMgr battleMgr)
        {
            GameMgr.Instance.TakeDmg(1);
        }
        else if (obj is Enemy enemy)
        {
            enemy.TakeDmgEnemy(1);
        }

        yield return null;
    }
}
