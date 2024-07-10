using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact_LuckyShield : Artifact
{
    public override void InvokeArtifact()
    {
        GameMgr.Instance.luckyShield = true;
    }
}
