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
        playerCurHP = playerMaxHP;
    }

    //�÷��̾� �ִ� ü�� ���� �Լ�
    public void IncreasePlayerHP(int val)
    {
        playerMaxHP += val;
        playerCurHP += val;
    }

    //�÷��̾� ���� ü�� get�Լ�
    public int GetPlayerHP()
    {
        return playerCurHP;
    }

    //�÷��̾� ���� �Լ�
    public void TakeDmg(int dmg)
    {
        playerCurHP -= dmg;
    }

    //�÷��̾� ȸ�� �Լ�
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

    //�÷��̾� ��� get�Լ�
    public int GetGoldAmount()
    {
        return playerGold;
    }

    //�÷��̾� ��� �߰� �Լ�
    public void earnGold(int val)
    {
        playerGold += val;
    }

    //�÷��̾� ���� �ɺ� ����Ʈ get�Լ�
    public List<Symbol> GetPlayerOwnSymbols()
    {
        return playerOwnSymbols;
    }

    //���� �ɺ� �߰� �Լ�
    public void AddPlayerSymbol(Symbol symbol)
    {
        playerOwnSymbols.Add(symbol);
    }

    //���� �� ��ü �ɺ� ����Ʈ get�Լ�
    public List<Symbol> GetEntireSymbols()
    {
        return entireSymbols;
    }

    //�÷��̾� ���� ���� ����Ʈ get�Լ�
    public List<Artifact> GetArtifacts()
    {
        return playerOwnArtifacts;
    }

    //���� ���� �߰� �Լ�
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
