using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public static GameMgr Instance { get; private set; }

    [SerializeField] int playerHP;
    [SerializeField] int playerGold;
    [SerializeField] List<Symbol> playerOwnSymbols;
    [SerializeField] List<Symbol> entireSymbols;
    [SerializeField] int wheelNumber;
    //List<Artifact> artifacts;
    //List<Item> items;

    public int rewardCount;

    private void Awake()
    {
        // �̱��� �ν��Ͻ��� ������ ���� �ν��Ͻ��� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� �ʵ��� ����
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� ���� �ν��Ͻ��� �ı�
        }
    }

    void Start()
    {
        //RefreshHp();
    }

    void Update()
    {
        
    }

    public int GetPlayerHP()
    {
        return playerHP;
    }

    public void TakeDmg(int dmg)
    {
        playerHP -= dmg;
        //RefreshHp();
    }

    public int GetGoldAmount()
    {
        return playerGold;
    }

    public void earnGold(int val)
    {
        playerGold += val;
    }

    //public void RefreshHp()
    //{
    //    playerHpText.text = playerHP.ToString();
    //}

    public List<Symbol> GetPlayerOwnSymbols()
    {
        return playerOwnSymbols;
    }

    public void AddPlayerSymbol(Symbol symbol)
    {
        playerOwnSymbols.Add(symbol);
    }

    public List<Symbol> GetEntireSymbols()
    {
        return entireSymbols;
    }
}
