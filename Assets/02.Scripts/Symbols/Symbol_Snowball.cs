using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Snowball : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        Debug.Log("스노우볼의 현재 데미지" + basicDmg);
        monster.GetComponent<Enemy>().TakeDmgEnemy(basicDmg);
        basicDmg += 1;
    }

    public override void PerfectEffect(GameObject monster)
    {
        Debug.Log("스노우볼의 현재 데미지" + basicDmg);
        monster.GetComponent<Enemy>().TakeDmgEnemy(basicDmg);
        basicDmg += 5;
    }
}
