using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventCtrl : MonoBehaviour
{
    public void AddWheel()
    {
        GameMgr.Instance.AddWheel();
    }

    public void RemoveWheel()
    {
        GameMgr.Instance.RemoveWheel();
    }

    public void Heal(int v)
    {
        GameMgr.Instance.HealPlayer(v);
    }

    public void Damage(int v)
    {
        GameMgr.Instance.TakeDmg(v);
    }

    public void AddSymbol(Symbol symbol)
    {
        GameMgr.Instance.AddPlayerSymbol(symbol);
    }

    public void AddRandomSymbol()
    {
        List<Symbol> entireSymbols = new List<Symbol>(GameMgr.Instance.GetEntireSymbols());
        int ran = Random.Range(0, entireSymbols.Count);
        GameMgr.Instance.AddPlayerSymbol(entireSymbols[ran]);
    }

    public void RemoveSymbol()
    {
        EventMgr eventMgr = GameObject.Find("EventMgr").GetComponent<EventMgr>();
        eventMgr.ShowSymbolSelectPanel();
        eventMgr.selectType = SelectType.Discard;
    }

    public void CopySymbol()
    {
        EventMgr eventMgr = GameObject.Find("EventMgr").GetComponent<EventMgr>();
        eventMgr.ShowSymbolSelectPanel();
        eventMgr.selectType = SelectType.Copy;
    }

    public void ChangeAllSymbols()
    {
        int symbolNum = GameMgr.Instance.GetPlayerOwnSymbols().Count;

        GameMgr.Instance.ResetPlayerSymbol();

        for(int i = 0; i < symbolNum; i++)
        {
            AddRandomSymbol();
        }
    }

    public void AddArtifact(Artifact artifact)
    {
        GameMgr.Instance.AddArtifact(artifact);
    }

    public void AddRandomArtifact()
    {
        List<Artifact> entireArtifacts = new List<Artifact>(GameMgr.Instance.GetAvailableArtifacts());
        int ran = Random.Range(0, entireArtifacts.Count);
        GameMgr.Instance.AddArtifact(entireArtifacts[ran]);
    }

    public void UseGold(int v)
    {
        GameMgr.Instance.UseGold(v);
    }

    public void UseGoldAndHeal(int v)
    {
        if(GameMgr.Instance.GetGoldAmount() >= v)
        {
            GameMgr.Instance.UseGold(v);

            GameMgr.Instance.HealPlayerByRate(0.5f);
        }
        else
        {
            Debug.Log("µ∑ ∫Œ¡∑");
        }
    }

    public void UseGoldAndAddSymbol(int v)
    {
        if (GameMgr.Instance.GetGoldAmount() >= v)
        {
            GameMgr.Instance.UseGold(v);

            AddRandomSymbol();
        }
        else
        {
            Debug.Log("µ∑ ∫Œ¡∑");
        }
    }

    public void UseGoldAndAddArtifact(int v)
    {
        if (GameMgr.Instance.GetGoldAmount() >= v)
        {
            GameMgr.Instance.UseGold(v);

            AddRandomArtifact();
        }
        else
        {
            Debug.Log("µ∑ ∫Œ¡∑");
        }
    }

    public void UseGoldAndRemoveSymbol(int v)
    {
        if (GameMgr.Instance.GetGoldAmount() >= v)
        {
            GameMgr.Instance.UseGold(v);

            RemoveSymbol();
        }
        else
        {
            Debug.Log("µ∑ ∫Œ¡∑");
        }
    }

    public void EarnGold(int v)
    {
        GameMgr.Instance.EarnGold(v);
    }
}
