using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Thorn : Symbol
{
    [SerializeField] Condition condition;
    public override void DuplicationEffect(int n, GameObject monster)
    {
        //해당 적의 출혈 찾아서 스택 가져오기
        Dictionary<Condition, int> conditions = monster.GetComponent<Enemy>().GetEnemyConditions();
        int bleed = 0;
        if (conditions.ContainsKey(condition))
        {
            bleed = conditions[condition];
        }

        //출혈 스택만큼 추가 피해
        AttackEnemy(monster, basicDmg + bleed);
        //출혈 3만큼 추가
        monster.GetComponent<Enemy>().AddEnemyCondition(condition, 3);
    }

    public override void PerfectEffect(GameObject monster)
    {
        //해당 적의 출혈 찾아서 스택 가져오기
        Dictionary<Condition, int> conditions = monster.GetComponent<Enemy>().GetEnemyConditions();
        int bleed = 0;
        if (conditions.ContainsKey(condition))
        {
            bleed = conditions[condition];
        }

        //출혈 스택만큼 추가 피해
        AttackEnemy(monster, basicDmg + bleed);
        //출혈 스택 2배만들기
        if(bleed != 0)
        {
            monster.GetComponent<Enemy>().AddEnemyCondition(condition, bleed);
        }
    }
}
