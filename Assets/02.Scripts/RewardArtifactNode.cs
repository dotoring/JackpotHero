using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardArtifactNode : MonoBehaviour
{
    Button button;
    Image image;
    public Artifact artifact;

    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        image.sprite = artifact.GetArtifactSprite();

        if(button != null )
        {
            button.onClick.AddListener(() =>
            {
                GameMgr.Instance.AddArtifact(artifact);
            });
        }
    }
}
