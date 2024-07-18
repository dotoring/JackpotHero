using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasureMgr : MonoBehaviour
{
    GameMgr gameMgr;

    public Button treasureBtn;

    public GameObject rewardArtifactsPanel;
    public GameObject rewardArtifactNodePref;
    public Transform rewardArtifactLayout;

    void Start()
    {
        gameMgr = GameMgr.Instance;

        GenRewardArtifacts();

        if(treasureBtn != null )
        {
            treasureBtn.onClick.AddListener(() =>
            {
                rewardArtifactsPanel.SetActive(true);
            });
        }
    }

    void GenRewardArtifacts()
    {
        List<Artifact> artifactRewards = new List<Artifact>();
        List<Artifact> entireArtifacts = new List<Artifact>(gameMgr.GetAvailableArtifacts());
        for (int i = 0; i < gameMgr.rewardCount + (gameMgr.gamblerSensor ? 1 : 0);) //������ �� �ִ� ���� �� ��ŭ �ݺ�
        {
            int ran = Random.Range(0, entireArtifacts.Count);
            if (artifactRewards.Contains(entireArtifacts[ran])) //�ߺ��̸� �ٽ� �̱�
            {
                continue;
            }
            else
            {
                artifactRewards.Add(entireArtifacts[ran]);
                i++;
            }
        }

        foreach (Artifact reward in artifactRewards)
        {
            GameObject rewardArtifact = Instantiate(rewardArtifactNodePref);
            rewardArtifact.transform.SetParent(rewardArtifactLayout, false);
            rewardArtifact.GetComponent<RewardArtifactNode>().artifact = reward;
        }
    }
}
