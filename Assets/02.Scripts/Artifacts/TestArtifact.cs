using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArtifact : Artifact
{
    public override void InvokeArtifact()
    {
        GameMgr.Instance.IncreasePlayerHP(15);
    }
}
