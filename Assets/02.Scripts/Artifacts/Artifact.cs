using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Artifact : MonoBehaviour
{
    [SerializeField] string artifactName;
    [SerializeField] string artifactDescription;
    [SerializeField] Sprite artifactSprite;

    public abstract void InvokeArtifact();

    public string GetArtifactName()
    {
        return artifactName;
    }

    public string GetArtifactDescription()
    {
        return artifactDescription;
    }

    public Sprite GetArtifactSprite()
    {
        return artifactSprite;
    }
}
