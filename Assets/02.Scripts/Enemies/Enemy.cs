using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyHp;
    public int enemyPower;
    public GameObject targetedImg;
    public Button button;
    public Text hpTxt;
    public BattleMgr battleMgr;
    public Dictionary<Condition, int> conditions = new Dictionary<Condition, int>();
    protected GameMgr gameMgr;

    public Image attckEffect;
    public Condition testCondition;

    protected virtual void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();

        RefreshEnemyHP();

        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                Debug.Log(gameObject);
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

        if (enemyHp <= 0)
        {
            battleMgr.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public virtual void TakeDmgEnemy(int n)
    {
        enemyHp -= n;
        RefreshEnemyHP();
    }

    public void RefreshEnemyHP()
    {
        hpTxt.text = enemyHp.ToString();
    }

    public virtual void AddEnemyCondition(Condition condition)
    {
        if(conditions.ContainsKey(condition))
        {
            conditions[condition]++;
        }
        else
        {
            conditions.Add(condition, 1);
        }
    }

    public abstract IEnumerator ActionEnemy();

}
