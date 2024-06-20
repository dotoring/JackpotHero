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
        // 싱글톤 인스턴스가 없으면 현재 인스턴스로 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 현재 인스턴스를 파괴
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
