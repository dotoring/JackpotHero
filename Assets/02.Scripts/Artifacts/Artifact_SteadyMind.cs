using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact_SteadyMind : Artifact
{
    public override void InvokeArtifact()
    {
        GameMgr.Instance.steadyMind = true;
    }
}
