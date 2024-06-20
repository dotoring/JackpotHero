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

    public virtual void EffectCondition()
    {
        Debug.Log("상태이상 : " + conditionName);
    }

    public ConditionType GetConditionType()
    {
        return type;
    }
}
