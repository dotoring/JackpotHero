using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Four : Symbol
{
    public override void DuplicationEffect(int n, GameObject monster)
    {
        Debug.Log(symbolName + "*" + n + "�� Ư�� ����");
        monster.GetComponent<Enemy>().TakeDmgEnemy(basicDmg + 5);
    }

    public override void PerfectEffect(GameObject monster)
    {
        Debug.Log(symbolName + "�� ����Ʈ ����");
        monster.GetComponent<Enemy>().TakeDmgEnemy(basicDmg + 10);
    }
}
