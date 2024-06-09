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
        Debug.Log("�� ���� �� ȿ�� �ڷ�ƾ");
        battleMgr.StartCoroutine(battleMgr.TurnStartEffect());
    }
}

public class PlayerTurnState : IBattleState
{
    public void OnState(BattleMgr battleMgr)
    {
        Debug.Log("���Ըӽ� ������ ��ư Ȱ��ȭ");
        battleMgr.SlotMachineOn();
    }
}

public class PlayerTurnEndState : IBattleState
{
    public void OnState(BattleMgr battleMgr)
    {
        Debug.Log("���� ���� üũ");
        int result = battleMgr.CheckBattleCondition();
        if (result == 0)
        {
            battleMgr.ChangeState(new MonsterTurnState());
        }
        else
        {
            battleMgr.ChangeState(new BattleEndState());
        }
    }
}

public class MonsterTurnState : IBattleState
{
    public void OnState(BattleMgr battleMgr)
    {
        Debug.Log("���� ���� �ڷ�ƾ");
        battleMgr.StartCoroutine(battleMgr.EnemiesAction());
    }
}

public class MonsterTurnEndState : IBattleState
{
    public void OnState(BattleMgr battleMgr)
    {
        Debug.Log("���� ���� üũ");
        int result = battleMgr.CheckBattleCondition();
        if (result == 0)
        {
            battleMgr.ChangeState(new TurnStartState());
        }
        else
        {
            battleMgr.ChangeState(new BattleEndState());
        }
    }
}

public class BattleEndState : IBattleState
{
    public void OnState(BattleMgr battleMgr)
    {
        Debug.Log("�º� ����� ���� ���â");
    }
}