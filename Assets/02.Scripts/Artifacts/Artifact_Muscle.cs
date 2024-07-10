using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact_Muscle : Artifact
{
    public override void InvokeArtifact()
    {
        GameMgr.Instance.muscle = true;
    }
}
