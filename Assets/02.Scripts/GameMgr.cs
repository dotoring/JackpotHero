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
    List<Artifact> availableArtifacts; //�ߺ� ������ ����
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
        SetStartValue();
        availableArtifacts = entireArtifacts;
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

    //�÷��̾� �ִ� ü�� ���� �Լ�
    public void IncreasePlayerHP(int val)
    {
        playerMaxHP += val;
        playerCurHP += val;
    }

    public int GetPlayerMaxHP()
    {
        return playerMaxHP;
    }

    //�÷��̾� ���� ü�� get�Լ�
    public int GetPlayerCurHP()
    {
        return playerCurHP;
    }

    //�÷��̾� ���� �Լ�
    public void TakeDmg(int dmg)
    {
        //�踮� �ִٸ�
        if(playerBarrier > 0)
        {
            //���ط��� �踮��� �۰ų� ���� ��
            if(playerBarrier >= dmg)
            {
                //�踮�� ���� �� ��
                playerBarrier -= dmg;
                return;
            }
            else
            {
                //���ط��� �踮��� Ŭ ��
                //�踮�� ��ŭ ���ط� ���� �� �踮�� 0
                dmg -= playerBarrier;
                ResetBarrier();
            }
        }
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

    public void HealPlayerByRate(float val)
    {
        int healVal = (int)(playerMaxHP * val);

        if (playerCurHP + healVal > playerMaxHP)
        {
            playerCurHP = playerMaxHP;
        }
        else
        {
            playerCurHP += healVal;
        }
    }

    public void ResetBarrier()
    {
        playerBarrier = 0;

        //ui���� ���ֱ�

    }

    public void AddBarrier(int val)
    {
        playerBarrier += val;

        //ui�߰� ���ֱ�

    }

    public int GetBarrier()
    {
        return playerBarrier;
    }

    //�÷��̾� ��� get�Լ�
    public int GetGoldAmount()
    {
        return playerGold;
    }

    //�÷��̾� ��� �߰� �Լ�
    public void EarnGold(int val)
    {
        playerGold += val;
    }

    public void UseGold(int val)
    {
        playerGold -= val;
    }

    //�÷��̾� ���� �ɺ� ����Ʈ get�Լ�
    public List<Symbol> GetPlayerOwnSymbols()
    {
        return playerOwnSymbols;
    }

    //���� �ɺ� �߰� �Լ�
    public void AddPlayerSymbol(Symbol symbol)
    {
        Symbol s = null;

        //�������� �ɺ��� �ƴҰ��
        if (!playerOwnSymbols.Contains(symbol))
        {
            //���� ����
            s = Instantiate(symbol, transform);
        }
        else //�������� �ɺ��� ���
        {
            //�����Ǿ��ִ� �ɺ� ã��
            foreach(Symbol sym in playerOwnSymbols)
            {
                if(symbol == sym)
                {
                    s = sym;
                    break;
                }
            }
        }
        
        //�ɺ� �߰�
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

    public void ResetPlayerSymbol()
    {
        playerOwnSymbols.Clear();
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
        //���� ��������Ʈ�� �߰�
        playerOwnArtifacts.Add(artifact);
        //���� ���� ���� ����Ʈ���� ����
        availableArtifacts.Remove(artifact);
        artifact.InvokeArtifact();
    }

    public List<Artifact> GetAvailableArtifacts()
    {
        return availableArtifacts;
    }

    public void AddWheel()
    {
        Debug.Log("TEST1");
        wheelNumber++;
    }

    public void RemoveWheel()
    {
        Debug.Log("TEST2");
        wheelNumber--;
    }

    public int GetWheelNumber()
    {
        return wheelNumber;
    }
}
