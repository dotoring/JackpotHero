using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public abstract class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyMaxHp;
    protected int enemyCurHp;
    public int enemyBasePower;
    public int enemyAdditionPower;
    public GameObject targetedImg;
    public Button button;
    public TextMeshProUGUI hpTxt;
    public BattleMgr battleMgr;
    public Dictionary<Condition, int> conditions = new Dictionary<Condition, int>();
    public Transform conditionsLayout;
    public GameObject conditionNodePref;
    protected GameMgr gameMgr;

    public bool isEnemyWeak;

    public Image attckEffect;

    protected virtual void Start()
    {
        gameMgr = GameMgr.Instance;

        enemyCurHp = enemyMaxHp;
        RefreshEnemyHP();

        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                battleMgr.SetTargetEnemy(gameObject);
            });
        }
    }

    protected virtual void Update()
    {
        if (battleMgr != null)
        {
            if (battleMgr.targetEnemy == gameObject)
            {
                targetedImg.SetActive(true);
            }
            else
            {
                targetedImg.SetActive(false);
            }
        }

        if (enemyCurHp <= 0)
        {
            battleMgr.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    //���ع޴� �Լ�
    public virtual void TakeDmgEnemy(int n)
    {
        //�÷��̾� ��ȭ ���¶�� ���� 50%����
        if(battleMgr.isPlayerWeak)
        {
            n /= 2;
        }
        enemyCurHp -= n;
        RefreshEnemyHP();

        if (enemyCurHp <= 0)
        {
            battleMgr.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    //ü�� UI ���ΰ�ħ
    public void RefreshEnemyHP()
    {
        hpTxt.text = enemyCurHp.ToString();
    }

    public void AttackPlayer()
    {
        int enemyPower = enemyBasePower + enemyAdditionPower;
        if(isEnemyWeak)
        {
            gameMgr.TakeDmg(enemyPower / 2);
        }
        else
        {
            gameMgr.TakeDmg(enemyPower);
        }
    }

    public Dictionary<Condition, int> GetEnemyConditions()
    {
        return conditions;
    }

    //���� �߰� �Լ�
    public virtual void AddEnemyCondition(Condition condition, int val)
    {
        if(conditions.ContainsKey(condition))
        {
            conditions[condition] += val;
        }
        else
        {
            conditions.Add(condition, val);
        }

        //���� �Ǵ� �������� ȿ�� �ٷ� ����
        if (condition.GetConditionType() == ConditionType.Buff ||
            condition.GetConditionType() == ConditionType.Debuff)
        {
            StartCoroutine(condition.EffectCondition(this, val));
        }

        RefreshConditionLayout();
    }

    public void RefreshConditionLayout()
    {
        foreach (Transform child in conditionsLayout.transform) //���� ������ ����Ʈ�� ������Ʈ�� ����
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<Condition, int> pair in conditions)
        {
            GameObject node = Instantiate(conditionNodePref);
            node.GetComponent<ConditionNode>().SetNode(pair.Key.conditionSprite, pair.Value);
            node.transform.SetParent(conditionsLayout.transform, false);
        }
    }

    public abstract IEnumerator ActionEnemy();
}
