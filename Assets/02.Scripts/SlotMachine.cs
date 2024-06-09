using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    GameMgr gameMgr;
    public BattleMgr battleMgr;

    public List<Symbol> symbols;
    public List<Symbol> randomSymbols = new List<Symbol>();
    //���� �ɺ������� ������ ���� ��ųʸ�
    Dictionary<Symbol, int> symbolCounts = new Dictionary<Symbol, int>();
    public List<Image> wheels;


    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();

        //���� �ɺ��� ��������
        symbols = gameMgr.GetSymbols();
    }

    public void RollSlotMachine(BattleMgr battleMgr)
    {
        if(randomSymbols != null) //��� �ʱ�ȭ
        {
            randomSymbols.Clear();
        }
        if(symbolCounts != null)
        {
            symbolCounts.Clear();
        }

        //�ɺ� ���� �̱�
        for (int i = 0; i < wheels.Count; i++) //���� �� ������ŭ
        {
            int n = Random.Range(0, symbols.Count); //���� �ɺ��� �� �ϳ� �̱�
            randomSymbols.Add(symbols[n]); //��� ����Ʈ�� �߰�
            wheels[i].sprite = symbols[n].sprite; //�� �̹��� ���� �ɺ��� ��ü
        }

        //�ߺ� �ɺ� �˻�
        foreach (Symbol sym in randomSymbols)
        {
            if (symbolCounts.ContainsKey(sym)) //�ִ� �ɺ��̸� value+1
            {
                symbolCounts[sym]++;
            }
            else //���� �ɺ��̸� value=1
            {
                symbolCounts[sym] = 1;
            }
        }

        StartCoroutine(ExecuteResult());
    }

    public IEnumerator ExecuteResult()
    {
        //��� Ȯ�� �� ȿ�� �ߵ�
        bool isDuplicate = false;
        Symbol highest = null;
        foreach (KeyValuePair<Symbol, int> pair in symbolCounts)
        {
            //�ߺ��� ���� �ɺ��� ���
            if (pair.Value == 1)
            {
                //�ߺ��� ���� ����� ���� �⺻ �������� ���� ū �ɺ� ����
                if (highest == null)
                {
                    highest = pair.Key;
                }
                else if (highest.basicDmg < pair.Key.basicDmg)
                {
                    highest = pair.Key;
                }
            }

            //�ߺ��� �ɺ��� ���
            if (pair.Value > 1)
            {
                isDuplicate = true;
                //�ߺ� ���� ���� ���� ���� ��� => ��� ���� �ɺ��� ���
                if (pair.Value == wheels.Count)
                {
                    yield return new WaitForSeconds(0.2f);
                    battleMgr.playerAttackEffect.SetActive(true);
                    //����Ʈ ȿ�� �ߵ�
                    pair.Key.PerfectEffect();
                    yield return new WaitForSeconds(0.2f);
                    battleMgr.playerAttackEffect.SetActive(false);
                }
                else
                {
                    yield return new WaitForSeconds(0.2f);
                    battleMgr.playerAttackEffect.SetActive(true);
                    //�ߺ� ȿ�� �ߵ�
                    pair.Key.DuplicationEffect(pair.Value);
                    yield return new WaitForSeconds(0.2f);
                    battleMgr.playerAttackEffect.SetActive(false);
                }
            }
        }

        //�ߺ��� �ɺ��� �ϳ��� ���� ���
        if (!isDuplicate)
        {
            yield return new WaitForSeconds(0.2f);
            battleMgr.playerAttackEffect.SetActive(true);
            //�⺻ �������� ���� ū �ɺ��� �⺻ ȿ�� �ߵ�
            highest.BasicEffect(battleMgr.targetEnemy);
            yield return new WaitForSeconds(0.2f);
            battleMgr.playerAttackEffect.SetActive(false);
        }

        battleMgr.ChangeState(new PlayerTurnEndState());
        yield return null;
    }
}
