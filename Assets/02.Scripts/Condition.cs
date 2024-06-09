using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
    Buff,
    Debuff,
    ActiveOnStart,
}

public class Condition : MonoBehaviour
{
    [SerializeField] string conditionName;
    [SerializeField] int value;
    [SerializeField] string description;
    [SerializeField] ConditionType type;

    public void EffectCondition()
    {
        Debug.Log("�����̻� : " + conditionName);
    }

    public IEnumerator EffectStartCondition()
    {
        Debug.Log("�����̻� : " + conditionName);
        yield return null;
    }

    public ConditionType GetConditionType()
    {
        return type;
    }
}
