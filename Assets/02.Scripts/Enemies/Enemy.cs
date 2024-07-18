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

    //피해받는 함수
    public virtual void TakeDmgEnemy(int n)
    {
        //플레이어 약화 상태라면 피해 50%감소
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

    //체력 UI 새로고침
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

    //상태 추가 함수
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

        //버프 또는 디버프라면 효과 바로 적용
        if (condition.GetConditionType() == ConditionType.Buff ||
            condition.GetConditionType() == ConditionType.Debuff)
        {
            StartCoroutine(condition.EffectCondition(this, val));
        }

        RefreshConditionLayout();
    }

    public void RefreshConditionLayout()
    {
        foreach (Transform child in conditionsLayout.transform) //보유 아이템 리스트의 오브젝트들 제거
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
