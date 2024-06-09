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
    //나온 심볼종류와 갯수를 세는 딕셔너리
    Dictionary<Symbol, int> symbolCounts = new Dictionary<Symbol, int>();
    public List<Image> wheels;


    void Start()
    {
        gameMgr = GameObject.Find("GameMgr").GetComponent<GameMgr>();

        //보유 심볼들 가져오기
        symbols = gameMgr.GetSymbols();
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

        StartCoroutine(ExecuteResult());
    }

    public IEnumerator ExecuteResult()
    {
        //결과 확인 및 효과 발동
        bool isDuplicate = false;
        Symbol highest = null;
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
                    yield return new WaitForSeconds(0.2f);
                    battleMgr.playerAttackEffect.SetActive(true);
                    //퍼펙트 효과 발동
                    pair.Key.PerfectEffect();
                    yield return new WaitForSeconds(0.2f);
                    battleMgr.playerAttackEffect.SetActive(false);
                }
                else
                {
                    yield return new WaitForSeconds(0.2f);
                    battleMgr.playerAttackEffect.SetActive(true);
                    //중복 효과 발동
                    pair.Key.DuplicationEffect(pair.Value);
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

        battleMgr.ChangeState(new PlayerTurnEndState());
        yield return null;
    }
}
