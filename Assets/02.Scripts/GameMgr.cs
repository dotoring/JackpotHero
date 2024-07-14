using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public static GameMgr Instance { get; private set; }

    [SerializeField] int playerMaxHP;
    [SerializeField] int playerCurHP;
    [SerializeField] int playerBarrier;
    [SerializeField] int playerGold;
    [SerializeField] List<Symbol> playerOwnSymbols;
    [SerializeField] List<Symbol> entireSymbols;
    [SerializeField] int wheelNumber;
    [SerializeField] List<Artifact> playerOwnArtifacts;
    [SerializeField] List<Artifact> entireArtifacts;
    //List<Item> items;

    public int rewardCount;

    [Header("Battle")]
    public bool isElite;

    [Header("Artifacts")]
    public int maxRoll;
    public bool goldenSlot;
    public bool muscle;
    public bool gamblerSensor;
    public bool vipCard;
    public bool powerOfCapital;
    public bool luckyShield;
    public bool steadyMind;

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
        SetStartValue();
    }

    void SetStartValue()
    {
        AddPlayerSymbol(entireSymbols[0]);
        AddPlayerSymbol(entireSymbols[1]);
        AddPlayerSymbol(entireSymbols[2]);
        AddPlayerSymbol(entireSymbols[3]);
        AddPlayerSymbol(entireSymbols[4]);
        AddPlayerSymbol(entireSymbols[5]);
    }

    //플레이어 최대 체력 증가 함수
    public void IncreasePlayerHP(int val)
    {
        playerMaxHP += val;
        playerCurHP += val;
    }

    public int GetPlayerMaxHP()
    {
        return playerMaxHP;
    }

    //플레이어 현재 체력 get함수
    public int GetPlayerCurHP()
    {
        return playerCurHP;
    }

    //플레이어 피해 함수
    public void TakeDmg(int dmg)
    {
        //배리어가 있다면
        if(playerBarrier > 0)
        {
            //피해량이 배리어보다 작거나 같을 때
            if(playerBarrier >= dmg)
            {
                //배리어 감소 후 끝
                playerBarrier -= dmg;
                return;
            }
            else
            {
                //피해량이 배리어보다 클 때
                //배리어 만큼 피해량 감소 후 배리어 0
                dmg -= playerBarrier;
                ResetBarrier();
            }
        }
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

    public void ResetBarrier()
    {
        playerBarrier = 0;

        //ui삭제 해주기

    }

    public void AddBarrier(int val)
    {
        playerBarrier += val;

        //ui추가 해주기

    }

    public int GetBarrier()
    {
        return playerBarrier;
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

    public void useGold(int val)
    {
        playerGold -= val;
    }

    //플레이어 보유 심볼 리스트 get함수
    public List<Symbol> GetPlayerOwnSymbols()
    {
        return playerOwnSymbols;
    }

    //보유 심볼 추가 함수
    public void AddPlayerSymbol(Symbol symbol)
    {
        Symbol s = null;

        if (!playerOwnSymbols.Contains(symbol))
        {
            s = Instantiate(symbol, transform);
        }
        else
        {
            foreach(Symbol sym in playerOwnSymbols)
            {
                if(symbol == sym)
                {
                    s = sym;
                    break;
                }
            }
        }
        
        if(s != null)
        {
            playerOwnSymbols.Add(s);
        }
        else
        {
            Debug.LogWarning("NewSymbol not found");
        }
    }

    public void RemovePlayerSymbol(Symbol symbol)
    {
        for(int i = playerOwnSymbols.Count -1; i >= 0; i--)
        {
            if (playerOwnSymbols[i] == symbol)
            {
                playerOwnSymbols.RemoveAt(i);
                return;
            }
        }
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

    public int GetWheelNumber()
    {
        return wheelNumber;
    }
}
