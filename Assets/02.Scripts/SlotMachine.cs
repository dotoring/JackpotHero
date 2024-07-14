using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    GameMgr gameMgr;
    public BattleMgr battleMgr;

    public List<Symbol> symbols;
    //랜덤으로 뽑은 심볼들 저장 리스트
    List<Symbol> randomSymbols = new List<Symbol>();
    //나온 심볼종류와 갯수를 세는 딕셔너리
    Dictionary<Symbol, int> symbolCounts = new Dictionary<Symbol, int>();

    public GameObject wheelNodePref;
    public Transform wheelLayout;

    public List<Image> wheels;

    public Button attackBtn;

    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();

        //휠 갯수에 따른 휠 생성
        for(int i = 0; i < gameMgr.GetWheelNumber(); i++)
        {
            GameObject wheel = Instantiate(wheelNodePref);
            wheel.transform.SetParent(wheelLayout, false);
            wheels.Add(wheel.GetComponent<WheelNode>().symbolImg);
        }

        //보유 심볼들 가져오기
        symbols = gameMgr.GetPlayerOwnSymbols();

        //공격 버튼 누르면 효과발동
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
        if(randomSymbols != null) //결과 초기화
        {
            randomSymbols.Clear();
        }
        if(symbolCounts != null)
        {
            symbolCounts.Clear();
        }

        //심볼 랜덤 뽑기
        for (int i = 0; i < wheels.Count; i++) //보유 휠 갯수만큼
        {
            int n = Random.Range(0, symbols.Count); //보유 심볼들 중 하나 뽑기
            randomSymbols.Add(symbols[n]); //결과 리스트에 추가
            wheels[i].sprite = symbols[n].sprite; //휠 이미지 뽑은 심볼로 교체
        }

        //중복 심볼 검사
        foreach (Symbol sym in randomSymbols)
        {
            if (symbolCounts.ContainsKey(sym)) //있는 심볼이면 value+1
            {
                symbolCounts[sym]++;
            }
            else //없던 심볼이면 value=1
            {
                symbolCounts[sym] = 1;
            }
        }

        if(battleMgr.rollCount == gameMgr.maxRoll) //리롤이 없으면 바로 효과 발동
        {
            attackBtn.gameObject.SetActive(false);
            StartCoroutine(ExecuteResult());
        }
        else
        {
            //리롤이 있다면 공격 버튼 활성화(공격할지 리롤할지 결정)
            attackBtn.gameObject.SetActive(true);
            //롤 버튼도 활성화
            battleMgr.SlotMachineOn();
        }
    }

    public IEnumerator ExecuteResult()
    {
        //결과 확인 및 효과 발동
        bool isDuplicate = false;
        Symbol highest = null;

        if(gameMgr.steadyMind) //건실한 정신 유물 효과
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
                //중복이 없는 심볼일 경우
                if (pair.Value == 1)
                {
                    //중복이 없는 결과를 위해 기본 데미지가 가장 큰 심볼 저장
                    if (highest == null)
                    {
                        highest = pair.Key;
                    }
                    else if (highest.basicDmg < pair.Key.basicDmg)
                    {
                        highest = pair.Key;
                    }
                }

                //중복된 심볼일 경우
                if (pair.Value > 1)
                {
                    isDuplicate = true;
                    //중복 수와 휠의 수가 같을 경우 => 모두 같은 심볼일 경우
                    if (pair.Value == wheels.Count)
                    {
                        //<<<황금슬롯 아티팩트 효과
                        if (gameMgr.goldenSlot)
                        {
                            gameMgr.earnGold(20);
                        }
                        //황금슬롯 아티팩트 효과>>>
                        //<<<행운방패 아티팩트 효과
                        if (gameMgr.luckyShield)
                        {
                            gameMgr.AddBarrier(10);
                        }
                        //행운방패 아티팩트 효과>>>

                        yield return new WaitForSeconds(0.2f);
                        battleMgr.playerAttackEffect.SetActive(true);
                        //퍼펙트 효과 발동
                        pair.Key.PerfectEffect(battleMgr.targetEnemy);
                        yield return new WaitForSeconds(0.2f);
                        battleMgr.playerAttackEffect.SetActive(false);
                    }
                    else
                    {
                        //<<<황금슬롯 아티팩트 효과
                        if (gameMgr.goldenSlot)
                        {
                            gameMgr.earnGold(5);
                        }
                        //황금슬롯 아티팩트 효과>>>
                        //<<<행운방패 아티팩트 효과
                        if (gameMgr.luckyShield)
                        {
                            gameMgr.AddBarrier(3);
                        }
                        //행운방패 아티팩트 효과>>>

                        yield return new WaitForSeconds(0.2f);
                        battleMgr.playerAttackEffect.SetActive(true);
                        //중복 효과 발동
                        pair.Key.DuplicationEffect(pair.Value, battleMgr.targetEnemy);
                        yield return new WaitForSeconds(0.2f);
                        battleMgr.playerAttackEffect.SetActive(false);
                    }
                }
            }

            //중복된 심볼이 하나도 없을 경우
            if (!isDuplicate)
            {
                yield return new WaitForSeconds(0.2f);
                battleMgr.playerAttackEffect.SetActive(true);
                //기본 데미지가 가장 큰 심볼의 기본 효과 발동
                highest.BasicEffect(battleMgr.targetEnemy);
                yield return new WaitForSeconds(0.2f);
                battleMgr.playerAttackEffect.SetActive(false);
            }
        }

        battleMgr.ChangeState(new PlayerTurnEndState());
        yield return null;
    }
}
