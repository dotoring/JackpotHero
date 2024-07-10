using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact_GamblerSensor : Artifact
{
    public override void InvokeArtifact()
    {
        GameMgr.Instance.gamblerSensor = true;
    }
}
