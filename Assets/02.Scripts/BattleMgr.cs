using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleMgr : MonoBehaviour
{
    GameMgr gameMgr;

    //�÷��̾�
    public GameObject playerAttackEffect;
    public Dictionary<Condition, int> playerConditions = new Dictionary<Condition, int>();

    //�� ���� ������
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

    //����
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

    //�� ���� �Լ�
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

    //���� ��� ���� �Լ�
    public void SetTargetEnemy(GameObject go)
    {
        targetEnemy = go;
    }

    //���� �� �ɺ� �߰� �Լ�
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
        //�� ����Ʈ�� �ִ� ��� �� ���� �ڷ�ƾ ������� ����
        foreach (GameObject mon in enemies)
        {
            yield return StartCoroutine(mon.GetComponent<Enemy>().ActionEnemy());
        }

        //���� �� ���� ���·� ����
        ChangeState(new MonsterTurnEndState());
        yield return null;
    }

    //�� ���� ȿ�� �ߵ� �Լ�
    public IEnumerator TurnStartEffect()
    {
        //����ִ� ���� ���
        foreach(GameObject mon in enemies)
        {
            foreach(KeyValuePair<Condition, int> pair in mon.GetComponent<Enemy>().conditions)
            {
                //���� �� �ߵ��Ǵ� ����ȿ���� ���
                if(pair.Key.GetConditionType() == ConditionType.ActiveOnStart)
                {
                    yield return new WaitForSeconds(0.2f);
                    Debug.Log("���� ȿ�� �ߵ�");
                    pair.Key.EffectCondition();
                }
            }
        }


        List<Condition> conditions = new List<Condition>();
        //�÷��̾� ����ȿ�� üũ
        foreach(KeyValuePair<Condition, int> pair in playerConditions)
        {
            //���� �� �ߵ��Ǵ� ����ȿ���� ���
            if (pair.Key.GetConditionType() == ConditionType.ActiveOnStart)
            {
                yield return new WaitForSeconds(0.2f);
                Debug.Log("���� ȿ�� �ִϸ��̼�");
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

    //�� ���� ���� �Լ�
    public void ChangeState(IBattleState newState)
    {
        curState = newState;
        curState.OnState(this);
    }

    //���� ��Ȳ üũ �Լ�
    public int CheckBattleCondition()
    {
        if (enemies.Count == 0)
        {
            return 1;
            //�¸�â
        }
        else if (gameMgr.GetPlayerHP() <= 0)
        {
            return 2;
            //�й� â
        }
        else
        {
            return 0;
        }
    }

    //���� ���� ���� �Լ�
    public void GenerateReward()
    {
        //�ɺ� ����
        List<Symbol> rewards = new List<Symbol>();

        List<Symbol> entireSymbols = new List<Symbol>(gameMgr.GetEntireSymbols());
        for (int i = 0; i < gameMgr.rewardCount;) //������ �� �ִ� �� ī�� �� ��ŭ �ݺ�
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

        //��� ����
        rewardGold = Random.Range(10, 21);
        goldEarnAmountText.text = rewardGold.ToString() + "���";
    }

    public bool GetRewardState()
    {
        return isPlayerGotSymbol;
    }

    public void ChangeRewardState()
    {
        isPlayerGotSymbol = !isPlayerGotSymbol;
    }

    //���� �ɺ� ���� �Լ�
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

    //���� �ɺ� Ȯ�� ��ư
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
