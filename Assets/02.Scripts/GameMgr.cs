using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    [SerializeField] int playerHP;
    [SerializeField] int gold;
    [SerializeField] List<Symbol> symbols;
    [SerializeField] int wheelNumber;
    //List<Artifact> artifacts;
    //List<Item> items;

    public Text playerHpText;

    // Start is called before the first frame update
    void Start()
    {
        RefreshHp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetPlayerHP()
    {
        return playerHP;
    }

    public void TakeDmg(int dmg)
    {
        playerHP -= dmg;
        RefreshHp();
    }

    public void RefreshHp()
    {
        playerHpText.text = playerHP.ToString();
    }

    public List<Symbol> GetSymbols()
    {
        return symbols;
    }
}
