using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactNode : MonoBehaviour
{
    public Image img;
    public Artifact artifact;

    // Start is called before the first frame update
    void Start()
    {
        img.sprite = artifact.GetArtifactSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
