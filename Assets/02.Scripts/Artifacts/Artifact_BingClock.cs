using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact_BingClock : Artifact
{
    public override void InvokeArtifact()
    {
        GameMgr.Instance.maxRoll++;
    }
}
