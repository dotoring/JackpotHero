using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Symbol : MonoBehaviour
{
    public string symbolName;
    public string symbolDescription;
    public int basicDmg;
    public Sprite sprite;

    public virtual void BasicEffect(GameObject monster)
    {
        Debug.Log(basicDmg + "¿« «««ÿ∏¶ ¡‹");
        monster.GetComponent<Enemy>().TakeDmgEnemy(basicDmg);
    }
    public abstract void DuplicationEffect(int n, GameObject monster);

    public abstract void PerfectEffect(GameObject monster);
}