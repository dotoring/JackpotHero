using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySecond : Enemy
{
    public override IEnumerator ActionEnemy()
    {
        yield return new WaitForSeconds(0.2f);
        attckEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gameMgr.TakeDmg(enemyPower);
        Heal(2);
        yield return new WaitForSeconds(0.2f);
        attckEffect.gameObject.SetActive(false);

        yield return null;
    }

    void Heal(int val)
    {
        enemyHp += val;
        RefreshEnemyHP();
    }
}
