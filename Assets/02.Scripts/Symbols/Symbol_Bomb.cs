using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Bomb : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        BurstBomb(5);
    }

    public override void PerfectEffect(GameObject monster)
    {
        BurstBomb(15);
    }

    void BurstBomb(int val)
    {
        BattleMgr battleMgr = GameObject.Find("BattleMgr").GetComponent<BattleMgr>();
        
        foreach(Enemy enemy in battleMgr.GetEnemies())
        {
            enemy.TakeDmgEnemy(val);
        }
    }
}
