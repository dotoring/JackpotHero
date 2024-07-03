using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleMgr : MonoBehaviour
{
    GameMgr gameMgr;

    [Header("Player")]
    //플레이어
    public int power = 0;
    public bool isPlayerWeak;

    public GameObject playerAttackEffect;
    public Dictionary<Condition, int> playerConditions = new Dictionary<Condition, int>();
    public Transform playerConditionLayout;
    public GameObject conditionNodePref;

    [Header ("Enemy")]
    //적 관련 변수들
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject targetEnemy;

    public GameObject testEnemyPref;
    public Button enemySpawnBtn; //for test

    public GameObject slotMachinePref;
    public SlotMachine slotMachine;
    public Button rollBtn;

    public IBattleState curState;

    public GameObject resultPanelWin;
    public GameObject resultPanelLose;

    [Header ("Rewards")]
    //보상
    public GameObject rewardLayout;
    public GameObject rewardSymbolNodePref;
    bool isPlayerGotSymbol;

    public int rewardGold;
    public Button goldEarnBtn;
    public Text goldEarnAmountText;

    public GameObject rewardArtifactPosition;
    public GameObject rewardArtifactNodePref;

    public Button nextStageBtn;

    [Header ("UI")]
    //UI
    public Text playerHp;

    public GameObject ownSymbolsPanel;
    public Button symbolsInventoryBtn;
    public GameObject ownSymbolGridLayout;
    public GameObject symbolNodePref;

    public Text playerGoldText;

    public GameObject artifactNodePref;
    public GameObject ownArtifactGridLayout;

    void Start()
    {
        gameMgr = GameMgr.Instance;
        gameMgr.ResetBarrier();

        ShowArtifacts();
        SpawnEnemy();
        GenerateOwnSymbols();
        GenerateReward();
        //resultPanelWin.SetActive(false);

        if(enemySpawnBtn != null)
        {
            enemySpawnBtn.onClick.AddListener(SpawnEnemy);
        }

        if(symbolsInventoryBtn != null)
        {
            symbolsInventoryBtn.onClick.AddListener(ShowSymbols);
        }

        if (rollBtn != null)
        {
            rollBtn.onClick.AddListener(() =>
            {
                SlotMachineOff();
                slotMachine.RollSlotMachine(this);
            });
        }

        if(nextStageBtn != null)
        {
            nextStageBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("MapScene");
            });
        }

        if(goldEarnBtn != null)
        {
            goldEarnBtn.onClick.AddListener(() =>
            {
                gameMgr.earnGold(rewardGold);
                goldEarnBtn.interactable = false;
            });
        }
    }

    private void Update()
    {
        playerHp.text = gameMgr.GetPlayerHP().ToString();
        playerGoldText.text = gameMgr.GetGoldAmount().ToString();
    }

    //적 생성 함수
    void SpawnEnemy()
    {
        int r = Random.Range(1, 3);
        for (int i = 0; i < r; i++)
        {
            GameObject mon = Instantiate(testEnemyPref);
            mon.GetComponent<Enemy>().battleMgr = this;
            mon.transform.position = new Vector2(2.5f + (2.5f * i), 1.6f);
            enemies.Add(mon);
        }
    }

    //공격 대상 설정 함수
    public void SetTargetEnemy(GameObject go)
    {
        targetEnemy = go;
    }

    public List<Enemy> GetEnemies()
    {
        List<Enemy> enemiesList = new List<Enemy>();
        foreach(GameObject e in enemies)
        {
            enemiesList.Add(e.GetComponent<Enemy>());
        }
        return enemiesList;
    }

    //전투 중 심볼 추가 함수
    public void AddSymbolInBattle(Symbol symbol)
    {
        slotMachine.symbols.Add(symbol);
    }

    public void AddPlayerCondition(Condition condition, int val)
    {
        if (playerConditions.ContainsKey(condition))
        {
            playerConditions[condition] += val;
        }
        else
        {
            playerConditions.Add(condition, val);
        }

        //버프 또는 디버프라면 효과 바로 적용
        if(condition.GetConditionType() == ConditionType.Buff || 
            condition.GetConditionType() == ConditionType.Debuff)
        {
            StartCoroutine(condition.EffectCondition(this, val));
        }

        RefreshConditionLayout();
    }

    public void RefreshConditionLayout()
    {
        foreach (Transform child in playerConditionLayout) //보유 아이템 리스트의 오브젝트들 제거
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<Condition, int> pair in playerConditions)
        {
            GameObject node = Instantiate(conditionNodePref);
            node.GetComponent<ConditionNode>().SetNode(pair.Key.conditionSprite, pair.Value);
            node.transform.SetParent(playerConditionLayout, false);
        }
    }

    public void SlotMachineOn()
    {
        rollBtn.interactable = true;
    }

    public void SlotMachineOff()
    {
        rollBtn.interactable = false;
    }

    public IEnumerator EnemiesAction()
    {
        yield return new WaitForSeconds(0.5f);
        //적 리스트에 있는 모든 몹 공격 코루틴 순서대로 실행
        foreach (GameObject mon in enemies)
        {
            yield return StartCoroutine(mon.GetComponent<Enemy>().ActionEnemy());
        }

        //몬스터 턴 종료 상태로 변경
        ChangeState(new MonsterTurnEndState());
        yield return null;
    }

    //턴 시작 효과 발동 함수
    public IEnumerator TurnStartEffect()
    {
        //=========================적==============================
        //살아있는 적들 모두 상태효과 발동
        for(int i = enemies.Count - 1; i >= 0; i--)
        {
            Enemy ene = enemies[i].GetComponent<Enemy>();

            //약화해제
            ene.isEnemyWeak = false;

            List<Condition> enemyConditions = new List<Condition>();

            foreach (KeyValuePair<Condition, int> pair in ene.conditions)
            {
                //시작 시 발동되는 상태효과일 경우
                if (!pair.Key.isPersist)
                {
                    enemyConditions.Add(pair.Key);
                }
            }


            foreach (Condition condition in enemyConditions)
            {
                //지속 효과가 아니면 하나씩 감소

                //1남은 경우 였으면 삭제
                if (ene.conditions[condition] == 1)
                {
                    yield return StartCoroutine(condition.EndCondition(ene));
                    if(ene == null) //죽었으면 넘어가기
                    {
                        continue;
                    }

                    ene.conditions.Remove(condition);
                }
                else
                {
                    yield return StartCoroutine(condition.EffectCondition(ene, ene.conditions[condition]));
                    if (ene == null) //죽었으면 넘어가기
                    {
                        continue;
                    }

                    //2이상 이라면 1씩 감소
                    ene.conditions[condition]--;
                }
            }

            if(ene != null)
            {
                ene.RefreshConditionLayout();
            }
        }


        //=========================플레이어==============================
        isPlayerWeak = false;

        List<Condition> conditions = new List<Condition>();
        //플레이어 상태효과 발동
        foreach(KeyValuePair<Condition, int> pair in playerConditions)
        {
            //시작 시 발동되는 상태효과일 경우
            if (!pair.Key.isPersist)
            {
                conditions.Add(pair.Key);
            }
        }

        //컨디션 지속기간 감소
        foreach(Condition condition in conditions)
        {
            if(condition.isEffected)
            {
                //1남은 경우 였으면 삭제
                if (playerConditions[condition] == 1)
                {
                    yield return StartCoroutine(condition.EndCondition(this));

                    playerConditions.Remove(condition);
                }
                else
                {
                    yield return StartCoroutine(condition.EffectCondition(this, playerConditions[condition]));

                    //2이상 이라면 1씩 감소
                    playerConditions[condition]--;
                }
            }
            else
            {
                condition.isEffected = true;
            }
        }

        RefreshConditionLayout();

        Debug.Log("전투 상태 체크");
        int result = CheckBattleCondition();
        if (result == 0)
        {
            ChangeState(new PlayerTurnState());
        }
        else if (result == 1)
        {
            ChangeState(new BattleEndState(true));
        }
        else if (result == 2)
        {
            ChangeState(new BattleEndState(false));
        }
        else
        {
            Debug.LogError("wrong battle result");
        }
        yield return null;
    }

    //턴 상태 변경 함수
    public void ChangeState(IBattleState newState)
    {
        curState = newState;
        curState.OnState(this);
    }

    //전투 상황 체크 함수
    public int CheckBattleCondition()
    {
        if (enemies.Count == 0)
        {
            return 1;
            //승리창
        }
        else if (gameMgr.GetPlayerHP() <= 0)
        {
            return 2;
            //패배 창
        }
        else
        {
            return 0;
        }
    }

    //전투 보상 생성 함수
    public void GenerateReward()
    {
        //심볼 보상
        List<Symbol> rewards = new List<Symbol>();

        List<Symbol> entireSymbols = new List<Symbol>(gameMgr.GetEntireSymbols());
        for (int i = 0; i < gameMgr.rewardCount;) //등장할 수 있는 보상 수 만큼 반복
        {
            int ran = Random.Range(0, entireSymbols.Count);
            if (rewards.Contains(entireSymbols[ran])) //중복이면 다시 뽑기
            {
                continue;
            }
            else
            {
                rewards.Add(entireSymbols[ran]);
                i++;
            }
        }

        foreach (Symbol reward in rewards)
        {
            GameObject rewardSymbol = Instantiate(rewardSymbolNodePref);
            rewardSymbol.transform.SetParent(rewardLayout.transform, false);
            rewardSymbol.GetComponent<RewardSymbolNode>().symbol = reward;
        }

        //골드 보상
        rewardGold = Random.Range(10, 21);
        goldEarnAmountText.text = rewardGold.ToString() + "골드";

        //유물 보상 ====================임시=================
        List<Artifact> artifacts = GameMgr.Instance.GetEntireArtifacts();
        Artifact artifact = artifacts[Random.Range(0, artifacts.Count)];
        GameObject rewardArtifact = Instantiate(rewardArtifactNodePref);
        rewardArtifact.GetComponent<RewardArtifactNode>().artifact = artifact;
        rewardArtifact.transform.SetParent(rewardArtifactPosition.transform, false);
    }

    public bool GetRewardState()
    {
        return isPlayerGotSymbol;
    }

    public void ChangeRewardState()
    {
        isPlayerGotSymbol = !isPlayerGotSymbol;
    }

    //보유 심볼 생성 함수
    void GenerateOwnSymbols()
    {
        List<Symbol> Symbols = new List<Symbol>(gameMgr.GetPlayerOwnSymbols());
        foreach(Symbol symbol in Symbols)
        {
            GameObject symbolNode = Instantiate(symbolNodePref);
            symbolNode.transform.SetParent(ownSymbolGridLayout.transform, false);
            symbolNode.GetComponent<SymbolNode>().symbol = symbol;
        }
    }

    //보유 심볼 확인 버튼
    void ShowSymbols()
    {
        if(ownSymbolsPanel.activeSelf)
        {
            ownSymbolsPanel.SetActive(false);
        }
        else
        {
            ownSymbolsPanel.SetActive(true);
        }
    }

    //유물 표시
    void ShowArtifacts()
    {
        foreach(Artifact artifact in gameMgr.GetArtifacts())
        {
            GameObject artifactNode = Instantiate(artifactNodePref);
            artifactNode.GetComponent<ArtifactNode>().artifact = artifact;
            artifactNode.transform.SetParent(ownArtifactGridLayout.transform, false);
        }
    }
}
