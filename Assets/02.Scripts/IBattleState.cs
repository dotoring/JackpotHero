using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBattleState
{
    public void OnState(BattleMgr battleMgr);
}

public class TurnStartState : IBattleState
{
    public void OnState(BattleMgr battleMgr)
    {
        Debug.Log("턴 시작 시 효과 코루틴");
        battleMgr.StartCoroutine(battleMgr.TurnStartEffect());
    }
}

public class PlayerTurnState : IBattleState
{
    public void OnState(BattleMgr battleMgr)
    {
        Debug.Log("슬롯머신 돌리기 버튼 활성화");
        battleMgr.SlotMachineOn();
    }
}

public class PlayerTurnEndState : IBattleState
{
    public void OnState(BattleMgr battleMgr)
    {
        Debug.Log("전투 상태 체크");
        int result = battleMgr.CheckBattleCondition();
        if (result == 0)
        {
            battleMgr.ChangeState(new MonsterTurnState());
        }
        else if (result == 1)
        {
            battleMgr.ChangeState(new BattleEndState(true));
        }
        else if (result == 2)
        {
            battleMgr.ChangeState(new BattleEndState(false));
        }
        else
        {
            Debug.LogError("wrong battle result");
        }
    }
}

public class MonsterTurnState : IBattleState
{
    public void OnState(BattleMgr battleMgr)
    {
        Debug.Log("몬스터 공격 코루틴");
        battleMgr.StartCoroutine(battleMgr.EnemiesAction());
    }
}

public class MonsterTurnEndState : IBattleState
{
    public void OnState(BattleMgr battleMgr)
    {
        Debug.Log("전투 상태 체크");
        int result = battleMgr.CheckBattleCondition();
        if (result == 0)
        {
            battleMgr.ChangeState(new TurnStartState());
        }
        else if (result == 1)
        {
            battleMgr.ChangeState(new BattleEndState(true));
        }
        else if (result == 2)
        {
            battleMgr.ChangeState(new BattleEndState(false));
        }
        else
        {
            Debug.LogError("wrong battle result");
        }
    }
}

public class BattleEndState : IBattleState
{
    public bool isWin;

    public BattleEndState(bool isWin)
    {
        this.isWin = isWin;
    }

    public void OnState(BattleMgr battleMgr)
    {
        Debug.Log("승부 결과에 따른 결과창");
        if(isWin)
        {
            battleMgr.resultPanelWin.SetActive(true);
        }
        else
        {
            battleMgr.resultPanelLose.SetActive(true);
        }
    }
}