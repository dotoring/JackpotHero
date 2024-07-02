using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Snowball : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        AttackEnemy(monster, basicDmg);
        basicDmg += 1;
    }

    public override void PerfectEffect(GameObject monster)
    {
        AttackEnemy(monster, basicDmg);
        basicDmg += 5;
    }
}
