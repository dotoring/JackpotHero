using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Symbol : MonoBehaviour
{
    public string symbolName;
    [TextArea(3, 7)]
    public string symbolDescription;
    public int basicDmg;
    public Sprite sprite;

    /// <summary>
    /// dmg = 해당 심볼의 최종 피해량
    /// </summary>
    /// <param name="monster"></param>
    /// <param name="dmg"></param>
    public void AttackEnemy(GameObject monster, int dmg)
    {
        Debug.Log(dmg + "+" + GameObject.Find("BattleMgr").GetComponent<BattleMgr>().power + "의 피해");
        monster.GetComponent<Enemy>().TakeDmgEnemy(dmg + GameObject.Find("BattleMgr").GetComponent<BattleMgr>().power);
    }

    public virtual void BasicEffect(GameObject monster)
    {
        AttackEnemy(monster, basicDmg);
    }

    public abstract void DuplicationEffect(int n, GameObject monster);

    public abstract void PerfectEffect(GameObject monster);


    // Equals 메서드 재정의
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Symbol other = (Symbol)obj;
        return symbolName == other.symbolName;
    }

    // GetHashCode 메서드 재정의
    public override int GetHashCode()
    {
        return HashCode.Combine(symbolName);
    }

    // == 연산자 오버로딩
    public static bool operator ==(Symbol p1, Symbol p2)
    {
        if (ReferenceEquals(p1, null))
        {
            return ReferenceEquals(p2, null);
        }
        return p1.Equals(p2);
    }

    // != 연산자 오버로딩
    public static bool operator !=(Symbol p1, Symbol p2)
    {
        return !(p1 == p2);
    }
}