using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Symbol : MonoBehaviour
{
    public string symbolName;
    public string description;
    public int basicDmg;
    public Sprite sprite;

    public virtual void BasicEffect(GameObject monster)
    {
        Debug.Log(basicDmg + "¿« «««ÿ∏¶ ¡‹");
        monster.GetComponent<Enemy>().TakeDmg(basicDmg);
    }
    public abstract void DuplicationEffect(int n);

    public abstract void PerfectEffect();
}