using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symbol_Two : Symbol
{
    public override void DuplicationEffect(int n)
    {
        Debug.Log(symbolName + "*" + n + "�� Ư�� ����");
    }

    public override void PerfectEffect()
    {
        Debug.Log(symbolName + "�� ����Ʈ ����");
    }
}
