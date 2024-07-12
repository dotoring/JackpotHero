using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardArtifactNode : ArtifactNode
{
    Button button;
    public BattleMgr battleMgr = null;
    ArtifactRewardPanel rewardPanel = null;

    void Start()
    {
        button = GetComponent<Button>();
        img = GetComponent<Image>();

        img.sprite = artifact.GetArtifactSprite();

        uiMgr = GameObject.Find("UIMgr").GetComponent<UIMgr>();
        rewardPanel = GetComponentInParent<ArtifactRewardPanel>();

        if (button != null )
        {
            button.onClick.AddListener(() =>
            {
                GameMgr.Instance.AddArtifact(artifact);
                rewardPanel.HidePanel();
                uiMgr.HideArtifactTooltip();
                uiMgr.RefreshOwnArtifacts();

                if(battleMgr != null )
                {
                    battleMgr.resultPanelWin.SetActive(true);
                }
            });
        }
    }
}
