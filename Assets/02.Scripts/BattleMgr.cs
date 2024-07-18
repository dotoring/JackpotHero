using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleMgr : MonoBehaviour
{
    GameMgr gameMgr;
    public SlotMachine slotMachine;
    public Button rollBtn;

    public IBattleState curState;
    public int rollCount;
    bool isRolled;

    public GameObject artifactRewardsPanel;
    public GameObject resultPanelWin;
    public GameObject resultPanelLose;

    [Header("Player")]
    //�÷��̾�
    public int power = 0;
    public bool isPlayerWeak;

    public GameObject playerAttackEffect;
    public Dictionary<Condition, int> playerConditions = new Dictionary<Condition, int>();
    public Transform playerConditionLayout;
    public GameObject conditionNodePref;

    [Header ("Enemy")]
    //�� ���� ������
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject targetEnemy;

    public GameObject testEnemyPref;

    [Header ("Rewards")]
    //����
    public Transform rewardSymbolLayout;
    public GameObject rewardSymbolNodePref;
    bool isPlayerGotSymbol;

    public int rewardGold;
    public Button goldEarnBtn;
    public Text goldEarnAmountText;

    public Transform rewardArtifactLayout;
    public GameObject rewardArtifactNodePref;

    public Button nextStageBtn;

    [Header ("UI")]
    public TextMeshProUGUI playerHp;
    public TextMeshProUGUI playerBarrier;
    public GameObject playerBarrierUI;

    [Header("for Artifacts")]
    [SerializeField] Condition powerUp;

    void Start()
    {
        gameMgr = GameMgr.Instance;
        gameMgr.ResetBarrier();

        SpawnEnemy();
        GenerateReward();
        ChangeState(new TurnStartState());
        rollCount = 0;

        if(gameMgr.muscle)
        {
            AddPlayerCondition(powerUp, 1);
        }

        if (rollBtn != null)
        {
            rollBtn.onClick.AddListener(() =>
            {
                isRolled = true; //���� ��� ������ ����
                rollCount++;
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
                gameMgr.EarnGold(rewardGold);
                goldEarnBtn.interactable = false;
            });
        }
    }

    private void Update()
    {
        playerHp.text = gameMgr.GetPlayerCurHP().ToString();
        playerBarrier.text = gameMgr.GetBarrier().ToString();
        if(gameMgr.GetBarrier() > 0)
        {
            playerBarrierUI.SetActive(true);
        }
        else
        {
            playerBarrierUI.SetActive(false);
        }
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
        if(!isRolled) //�귿 ���� �Ŀ� ���� ���� ���� �Ұ�
        {
            targetEnemy = go;
        }
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

    //���� �� �ɺ� �߰� �Լ�
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

        //���� �Ǵ� �������� ȿ�� �ٷ� ����
        if(condition.GetConditionType() == ConditionType.Buff || 
            condition.GetConditionType() == ConditionType.Debuff)
        {
            StartCoroutine(condition.EffectCondition(this, val));
        }

        RefreshConditionLayout();
    }

    public void RefreshConditionLayout()
    {
        foreach (Transform child in playerConditionLayout) //���� ������ ����Ʈ�� ������Ʈ�� ����
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
        //=========================��==============================
        //����ִ� ���� ��� ����ȿ�� �ߵ�
        for(int i = enemies.Count - 1; i >= 0; i--)
        {
            Enemy ene = enemies[i].GetComponent<Enemy>();

            //��ȭ����
            ene.isEnemyWeak = false;

            List<Condition> enemyConditions = new List<Condition>();

            foreach (KeyValuePair<Condition, int> pair in ene.conditions)
            {
                //���� �� �ߵ��Ǵ� ����ȿ���� ���
                if (!pair.Key.isPersist)
                {
                    enemyConditions.Add(pair.Key);
                }
            }


            foreach (Condition condition in enemyConditions)
            {
                //���� ȿ���� �ƴϸ� �ϳ��� ����

                //1���� ��� ������ ����
                if (ene.conditions[condition] == 1)
                {
                    yield return StartCoroutine(condition.EndCondition(ene));
                    if(ene == null) //�׾����� �Ѿ��
                    {
                        continue;
                    }

                    ene.conditions.Remove(condition);
                }
                else
                {
                    yield return StartCoroutine(condition.EffectCondition(ene, ene.conditions[condition]));
                    if (ene == null) //�׾����� �Ѿ��
                    {
                        continue;
                    }

                    //2�̻� �̶�� 1�� ����
                    ene.conditions[condition]--;
                }
            }

            if(ene != null)
            {
                ene.RefreshConditionLayout();
            }
        }


        //=========================�÷��̾�==============================
        isPlayerWeak = false;

        List<Condition> conditions = new List<Condition>();
        //�÷��̾� ����ȿ�� �ߵ�
        foreach(KeyValuePair<Condition, int> pair in playerConditions)
        {
            //���� �� �ߵ��Ǵ� ����ȿ���� ���
            if (!pair.Key.isPersist)
            {
                conditions.Add(pair.Key);
            }
        }

        //����ȿ�� ���ӱⰣ ����
        foreach(Condition condition in conditions)
        {
            if(condition.isEffected)
            {
                //1���� ��� ������ ����
                if (playerConditions[condition] == 1)
                {
                    yield return StartCoroutine(condition.EndCondition(this));

                    playerConditions.Remove(condition);
                }
                else
                {
                    yield return StartCoroutine(condition.EffectCondition(this, playerConditions[condition]));

                    //2�̻� �̶�� 1�� ����
                    playerConditions[condition]--;
                }
            }
            else
            {
                condition.isEffected = true;
            }
        }

        //����ȿ�� UI �ʱ�ȭ
        RefreshConditionLayout();

        Debug.Log("���� ���� üũ");
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

        isRolled = false; //���� ��� ���� �����ϵ���
        rollCount = 0; //�� Ƚ�� �ʱ�ȭ

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
        else if (gameMgr.GetPlayerCurHP() <= 0)
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
        List<Symbol> symbolRewards = new List<Symbol>();
        List<Symbol> entireSymbols = new List<Symbol>(gameMgr.GetEntireSymbols());
        for (int i = 0; i < gameMgr.rewardCount + (gameMgr.gamblerSensor ? 1 : 0);) //������ �� �ִ� ���� �� ��ŭ �ݺ�
        {
            int ran = Random.Range(0, entireSymbols.Count);
            if (symbolRewards.Contains(entireSymbols[ran])) //�ߺ��̸� �ٽ� �̱�
            {
                continue;
            }
            else
            {
                symbolRewards.Add(entireSymbols[ran]);
                i++;
            }
        }

        foreach (Symbol reward in symbolRewards)
        {
            GameObject rewardSymbol = Instantiate(rewardSymbolNodePref);
            rewardSymbol.transform.SetParent(rewardSymbolLayout, false);
            rewardSymbol.GetComponent<RewardSymbolNode>().symbol = reward;
        }

        //��� ����
        rewardGold = Random.Range(10, 21);
        goldEarnAmountText.text = rewardGold.ToString() + "���";

        //���� ����
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
            rewardArtifact.GetComponent<RewardArtifactNode>().battleMgr = this;
        }
    }

    public bool GetRewardState()
    {
        return isPlayerGotSymbol;
    }

    public void ChangeRewardState()
    {
        isPlayerGotSymbol = !isPlayerGotSymbol;
    }
}
