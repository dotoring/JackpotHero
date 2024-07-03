using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
    Buff,
    Debuff,
    ActiveOnStart,
}

public abstract class Condition : MonoBehaviour
{
    public Sprite conditionSprite;
    [SerializeField] protected string conditionName;
    [SerializeField] protected int value;
    [SerializeField] protected string description;
    [SerializeField] protected ConditionType type;
    public bool isPersist;
    public bool isEffected;

    /// <param name="obj">�÷��̾� or ��</param>
    /// <param name="val">��ġ</param>
    public abstract IEnumerator EffectCondition(object obj, int val);

    public abstract IEnumerator EndCondition(object obj);

    public ConditionType GetConditionType()
    {
        return type;
    }
}
