using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int hp;
    public int power;
    public GameObject targetedImg;
    public Button button;
    public Text hpTxt;
    public BattleMgr battleMgr;
    public Dictionary<Condition, int> conditions = new Dictionary<Condition, int>();
    GameMgr gameMgr;

    public Image attckEffect;
    public Condition testCondition;

    // Start is called before the first frame update
    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();

        RefreshHP();

        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                Debug.Log(gameObject);
                battleMgr.SetTargetEnemy(gameObject);
            });
        }
    }

    // Update is called once per frame
    void Update()
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

        if (hp <= 0)
        {
            battleMgr.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    public void TakeDmg(int n)
    {
        hp -= n;
        RefreshHP();
    }

    public void RefreshHP()
    {
        hpTxt.text = hp.ToString();
    }

    public void AddEnemyCondition(Condition condition)
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

    public IEnumerator Action()
    {
        yield return new WaitForSeconds(0.2f);
        attckEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gameMgr.TakeDmg(power);
        battleMgr.AddPlayerCondition(testCondition);
        yield return new WaitForSeconds(0.2f);
        attckEffect.gameObject.SetActive(false);

        yield return null;
    }
}
