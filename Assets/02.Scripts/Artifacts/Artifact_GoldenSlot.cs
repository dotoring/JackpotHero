using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact_GoldenSlot : Artifact
{
    public override void InvokeArtifact()
    {
        GameMgr.Instance.goldenSlot = true;
    }
}
