using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Four : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        Debug.Log(symbolName + "*" + n + "의 특수 피해");
        monster.GetComponent<Enemy>().TakeDmgEnemy(basicDmg + 5);
    }

    public override void PerfectEffect(GameObject monster)
    {
        Debug.Log(symbolName + "의 퍼펙트 피해");
        monster.GetComponent<Enemy>().TakeDmgEnemy(basicDmg + 10);
    }
}
