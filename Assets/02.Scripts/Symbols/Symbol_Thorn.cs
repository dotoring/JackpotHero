using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Thorn : Symbol
{
    [SerializeField] Condition condition;
    public override void DuplicationEffect(int n, GameObject monster)
    {
        //�ش� ���� ���� ã�Ƽ� ���� ��������
        Dictionary<Condition, int> conditions = monster.GetComponent<Enemy>().GetEnemyConditions();
        int bleed = 0;
        if (conditions.ContainsKey(condition))
        {
            bleed = conditions[condition];
        }

        //���� ���ø�ŭ �߰� ����
        AttackEnemy(monster, basicDmg + bleed);
        //���� 3��ŭ �߰�
        monster.GetComponent<Enemy>().AddEnemyCondition(condition, 3);
    }

    public override void PerfectEffect(GameObject monster)
    {
        //�ش� ���� ���� ã�Ƽ� ���� ��������
        Dictionary<Condition, int> conditions = monster.GetComponent<Enemy>().GetEnemyConditions();
        int bleed = 0;
        if (conditions.ContainsKey(condition))
        {
            bleed = conditions[condition];
        }

        //���� ���ø�ŭ �߰� ����
        AttackEnemy(monster, basicDmg + bleed);
        //���� ���� 2�踸���
        if(bleed != 0)
        {
            monster.GetComponent<Enemy>().AddEnemyCondition(condition, bleed);
        }
    }
}
