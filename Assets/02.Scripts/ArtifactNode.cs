using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArtifactNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image img;
    public Artifact artifact;

    protected UIMgr uiMgr;

    void Start()
    {
        img.sprite = artifact.GetArtifactSprite();

        uiMgr = GameObject.Find("UIMgr").GetComponent<UIMgr>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiMgr.ShowAndSetArtifactTooltip(artifact.GetArtifactName(), artifact.GetArtifactDescription());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiMgr.HideArtifactTooltip();
    }
}
