using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Stone : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        monster.GetComponent<Enemy>().TakeDmgEnemy(basicDmg);
    }

    public override void PerfectEffect(GameObject monster)
    {
        monster.GetComponent<Enemy>().TakeDmgEnemy(basicDmg);
    }
}
