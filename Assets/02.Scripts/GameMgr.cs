using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public static GameMgr Instance { get; private set; }

    [SerializeField] int playerMaxHP;
    [SerializeField] int playerCurHP;
    [SerializeField] int playerGold;
    [SerializeField] List<Symbol> playerOwnSymbols;
    [SerializeField] List<Symbol> entireSymbols;
    [SerializeField] int wheelNumber;
    [SerializeField] List<Artifact> playerOwnArtifacts;
    [SerializeField] List<Artifact> entireArtifacts;
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
        playerCurHP = playerMaxHP;
    }

    //플레이어 최대 체력 증가 함수
    public void IncreasePlayerHP(int val)
    {
        playerMaxHP += val;
        playerCurHP += val;
    }

    //플레이어 현재 체력 get함수
    public int GetPlayerHP()
    {
        return playerCurHP;
    }

    //플레이어 피해 함수
    public void TakeDmg(int dmg)
    {
        playerCurHP -= dmg;
    }

    //플레이어 회복 함수
    public void HealPlayer(int val)
    {
        if(playerCurHP + val > playerMaxHP)
        {
            playerCurHP = playerMaxHP;
        }
        else
        {
            playerCurHP += val;
        }
    }

    //플레이어 골드 get함수
    public int GetGoldAmount()
    {
        return playerGold;
    }

    //플레이어 골드 추가 함수
    public void earnGold(int val)
    {
        playerGold += val;
    }

    //플레이어 보유 심볼 리스트 get함수
    public List<Symbol> GetPlayerOwnSymbols()
    {
        return playerOwnSymbols;
    }

    //보유 심볼 추가 함수
    public void AddPlayerSymbol(Symbol symbol)
    {
        playerOwnSymbols.Add(symbol);
    }

    //게임 내 전체 심볼 리스트 get함수
    public List<Symbol> GetEntireSymbols()
    {
        return entireSymbols;
    }

    //플레이어 보유 유물 리스트 get함수
    public List<Artifact> GetArtifacts()
    {
        return playerOwnArtifacts;
    }

    //보유 유물 추가 함수
    public void AddArtifact(Artifact artifact)
    {
        playerOwnArtifacts.Add(artifact);
        artifact.InvokeArtifact();
    }

    public List<Artifact> GetEntireArtifacts()
    {
        return entireArtifacts;
    }
}
