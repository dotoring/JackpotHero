using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    public override IEnumerator ActionEnemy()
    {
        yield return new WaitForSeconds(0.2f);
        attckEffect.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        AttackPlayer();
        yield return new WaitForSeconds(0.2f);
        attckEffect.gameObject.SetActive(false);

        yield return null;
    }
}
