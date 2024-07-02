using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Symbol_One : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        AttackEnemy(monster, basicDmg + 5);
    }

    public override void PerfectEffect(GameObject monster)
    {
        AttackEnemy(monster, basicDmg + 10);
    }
}
