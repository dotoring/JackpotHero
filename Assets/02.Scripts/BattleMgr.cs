using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleMgr : MonoBehaviour
{
    GameMgr gameMgr;

    //플레이어
    public GameObject playerAttackEffect;
    public Dictionary<Condition, int> playerConditions = new Dictionary<Condition, int>();

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

    //보상
    public GameObject rewardLayout;
    public GameObject rewardSymbolNodePref;
    bool isPlayerGotSymbol;

    public int rewardGold;

    public Button nextStageBtn;

    //UI
    public Text playerHp;

    public GameObject ownSymbolsPanel;
    public Button symbolsInventoryBtn;
    public GameObject ownSymbolGridLayout;
    public GameObject symbolNodePref;

    public Text playerGoldText;

    public Button goldEarnBtn;
    public Text goldEarnAmountText;

    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();

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
                SceneManager.LoadScene("MapTempScene");
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

    //전투 중 심볼 추가 함수
    public void AddSymbolInBattle(Symbol symbol)
    {
        slotMachine.symbols.Add(symbol);
    }

    public void AddPlayerCondition(Condition condition)
    {
        if (playerConditions.ContainsKey(condition))
        {
            playerConditions[condition]++;
        }
        else
        {
            playerConditions.Add(condition, 1);
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
        //살아있는 적들 모두
        foreach(GameObject mon in enemies)
        {
            foreach(KeyValuePair<Condition, int> pair in mon.GetComponent<Enemy>().conditions)
            {
                //시작 시 발동되는 상태효과일 경우
                if(pair.Key.GetConditionType() == ConditionType.ActiveOnStart)
                {
                    yield return new WaitForSeconds(0.2f);
                    Debug.Log("상태 효과 발동");
                    pair.Key.EffectCondition();
                }
            }
        }


        List<Condition> conditions = new List<Condition>();
        //플레이어 상태효과 체크
        foreach(KeyValuePair<Condition, int> pair in playerConditions)
        {
            //시작 시 발동되는 상태효과일 경우
            if (pair.Key.GetConditionType() == ConditionType.ActiveOnStart)
            {
                yield return new WaitForSeconds(0.2f);
                Debug.Log("상태 효과 애니메이션");
                yield return new WaitForSeconds(0.5f);
                pair.Key.EffectCondition();

                conditions.Add(pair.Key);
            }
        }

        foreach(Condition condition in conditions)
        {
            playerConditions[condition]--;
        }

        ChangeState(new PlayerTurnState());
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
        for (int i = 0; i < gameMgr.rewardCount;) //등장할 수 있는 새 카드 수 만큼 반복
        {
            int ran = Random.Range(0, entireSymbols.Count);
            if (rewards.Contains(entireSymbols[ran]))
            {
                continue;
            }
            else
            {
                rewards.Add(entireSymbols[ran]);
                i++;
            }
        }

        Debug.Log(rewards.Count);

        foreach (Symbol reward in rewards)
        {
            GameObject rewardSymbol = Instantiate(rewardSymbolNodePref);
            rewardSymbol.transform.SetParent(rewardLayout.transform, false);
            rewardSymbol.GetComponent<RewardSymbolNode>().symbol = reward;
        }

        //골드 보상
        rewardGold = Random.Range(10, 21);
        goldEarnAmountText.text = rewardGold.ToString() + "골드";
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
}
