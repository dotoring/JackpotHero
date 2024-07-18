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
        for (int i = 0; i < gameMgr.rewardCount + (gameMgr.gamblerSensor ? 1 : 0);) //등장할 수 있는 보상 수 만큼 반복
        {
            int ran = Random.Range(0, entireArtifacts.Count);
            if (artifactRewards.Contains(entireArtifacts[ran])) //중복이면 다시 뽑기
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
