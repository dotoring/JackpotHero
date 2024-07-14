using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    GameMgr gameMgr;
    public BattleMgr battleMgr;

    public List<Symbol> symbols;
    //�������� ���� �ɺ��� ���� ����Ʈ
    List<Symbol> randomSymbols = new List<Symbol>();
    //���� �ɺ������� ������ ���� ��ųʸ�
    Dictionary<Symbol, int> symbolCounts = new Dictionary<Symbol, int>();

    public GameObject wheelNodePref;
    public Transform wheelLayout;

    public List<Image> wheels;

    public Button attackBtn;

    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();

        //�� ������ ���� �� ����
        for(int i = 0; i < gameMgr.GetWheelNumber(); i++)
        {
            GameObject wheel = Instantiate(wheelNodePref);
            wheel.transform.SetParent(wheelLayout, false);
            wheels.Add(wheel.GetComponent<WheelNode>().symbolImg);
        }

        //���� �ɺ��� ��������
        symbols = gameMgr.GetPlayerOwnSymbols();

        //���� ��ư ������ ȿ���ߵ�
        if(attackBtn != null)
        {
            attackBtn.onClick.AddListener(() =>
            {
                StartCoroutine(ExecuteResult());
                attackBtn.gameObject.SetActive(false);
                battleMgr.SlotMachineOff();
            });
        }
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

        if(battleMgr.rollCount == gameMgr.maxRoll) //������ ������ �ٷ� ȿ�� �ߵ�
        {
            attackBtn.gameObject.SetActive(false);
            StartCoroutine(ExecuteResult());
        }
        else
        {
            //������ �ִٸ� ���� ��ư Ȱ��ȭ(�������� �������� ����)
            attackBtn.gameObject.SetActive(true);
            //�� ��ư�� Ȱ��ȭ
            battleMgr.SlotMachineOn();
        }
    }

    public IEnumerator ExecuteResult()
    {
        //��� Ȯ�� �� ȿ�� �ߵ�
        bool isDuplicate = false;
        Symbol highest = null;

        if(gameMgr.steadyMind) //�ǽ��� ���� ���� ȿ��
        {
            int dmg = 0;
            foreach(KeyValuePair<Symbol, int> pair in symbolCounts)
            {
                dmg += pair.Key.basicDmg * pair.Value;
            }
            battleMgr.targetEnemy.GetComponent<Enemy>().TakeDmgEnemy(dmg);
        }
        else
        {
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
                        //<<<Ȳ�ݽ��� ��Ƽ��Ʈ ȿ��
                        if (gameMgr.goldenSlot)
                        {
                            gameMgr.earnGold(20);
                        }
                        //Ȳ�ݽ��� ��Ƽ��Ʈ ȿ��>>>
                        //<<<������ ��Ƽ��Ʈ ȿ��
                        if (gameMgr.luckyShield)
                        {
                            gameMgr.AddBarrier(10);
                        }
                        //������ ��Ƽ��Ʈ ȿ��>>>

                        yield return new WaitForSeconds(0.2f);
                        battleMgr.playerAttackEffect.SetActive(true);
                        //����Ʈ ȿ�� �ߵ�
                        pair.Key.PerfectEffect(battleMgr.targetEnemy);
                        yield return new WaitForSeconds(0.2f);
                        battleMgr.playerAttackEffect.SetActive(false);
                    }
                    else
                    {
                        //<<<Ȳ�ݽ��� ��Ƽ��Ʈ ȿ��
                        if (gameMgr.goldenSlot)
                        {
                            gameMgr.earnGold(5);
                        }
                        //Ȳ�ݽ��� ��Ƽ��Ʈ ȿ��>>>
                        //<<<������ ��Ƽ��Ʈ ȿ��
                        if (gameMgr.luckyShield)
                        {
                            gameMgr.AddBarrier(3);
                        }
                        //������ ��Ƽ��Ʈ ȿ��>>>

                        yield return new WaitForSeconds(0.2f);
                        battleMgr.playerAttackEffect.SetActive(true);
                        //�ߺ� ȿ�� �ߵ�
                        pair.Key.DuplicationEffect(pair.Value, battleMgr.targetEnemy);
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
        }

        battleMgr.ChangeState(new PlayerTurnEndState());
        yield return null;
    }
}
