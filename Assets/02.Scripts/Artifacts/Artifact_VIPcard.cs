using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact_VIPcard : Artifact
{
    public override void InvokeArtifact()
    {
        GameMgr.Instance.vipCard = true;
    }
}
