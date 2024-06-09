using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Two : Symbol
{
    public override void DuplicationEffect(int n)
    {
        Debug.Log(symbolName + "*" + n + "의 특수 피해");
    }

    public override void PerfectEffect()
    {
        Debug.Log(symbolName + "의 퍼펙트 피해");
    }
}
