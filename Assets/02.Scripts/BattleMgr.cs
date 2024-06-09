using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();

        if(enemySpawnBtn != null)
        {
            enemySpawnBtn.onClick.AddListener(SpawnEnemy);
        }

        if (rollBtn != null)
        {
            rollBtn.onClick.AddListener(() =>
            {
                SlotMachineOff();
                slotMachine.RollSlotMachine(this);
            });
        }
    }

    //�� ���� �Լ�
    void SpawnEnemy()
    {
        int r = Random.Range(1, 4);
        for (int i = 0; i < r; i++)
        {
            GameObject mon = Instantiate(testEnemyPref);
            mon.GetComponent<Enemy>().battleMgr = this;
            mon.transform.position = new Vector2(2.5f + (2.5f * i), 2);
            enemies.Add(mon);
        }
    }

    //���� ��� ���� �Լ�
    public void SetTargetEnemy(GameObject go)
    {
        targetEnemy = go;
    }

    //���� �� �ɺ� �߰� �Լ�
    public void AddSymbol(Symbol symbol)
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
            yield return StartCoroutine(mon.GetComponent<Enemy>().Action());
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
                    pair.Key.StartCoroutine(pair.Key.EffectStartCondition());
                }
            }
        }

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

                //�����̻� Ƚ�� ����
                if(pair.Value == 1)
                {
                    playerConditions.Remove(pair.Key);
                }
                else
                {
                    playerConditions[pair.Key]--;
                }
            }
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
}
