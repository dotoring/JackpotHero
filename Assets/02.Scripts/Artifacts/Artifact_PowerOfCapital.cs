using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact_PowerOfCapital : Artifact
{
    public override void InvokeArtifact()
    {
        GameMgr.Instance.powerOfCapital = true;
    }
}
