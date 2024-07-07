using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetSceneMgr : MonoBehaviour
{
    GameMgr gameMgr;
    public SymbolScrollCtrl symbolScrollCtrl;
    public GameObject ownSymbolsPanel;

    public Button recoverBtn;
    public Button discardSelectBtn;
    public Button discardSymbolBtn;

    [Header("UI")]
    public TextMeshProUGUI playerHp;
    public TextMeshProUGUI playerBarrier;
    public GameObject playerBarrierUI;


    void Start()
    {
        gameMgr = GameMgr.Instance;

        if(recoverBtn != null)
        {
            recoverBtn.onClick.AddListener(() =>
            {
                StartCoroutine(Recover());
            });
        }

        if(discardSymbolBtn != null)
        {
            discardSymbolBtn.onClick.AddListener(DiscardSymbol);
        }

        if(discardSelectBtn != null)
        {
            discardSelectBtn.onClick.AddListener(ShowDiscardSymbolPanel);
        }
    }

    void ShowDiscardSymbolPanel()
    {
        discardSymbolBtn.gameObject.SetActive(true);
    }

    void Update()
    {
        playerHp.text = gameMgr.GetPlayerCurHP().ToString();
        playerBarrier.text = gameMgr.GetBarrier().ToString();
        if (gameMgr.GetBarrier() > 0)
        {
            playerBarrierUI.SetActive(true);
        }
        else
        {
            playerBarrierUI.SetActive(false);
        }

        if(!ownSymbolsPanel.activeSelf)
        {
            discardSymbolBtn.gameObject.SetActive(false);
        }
    }

    IEnumerator Recover()
    {
        gameMgr.HealPlayer((int)(gameMgr.GetPlayerMaxHP() / 2));

        //»∏∫π æ÷¥œ∏ﬁ¿Ãº«
        Debug.Log("ªœ∑Œ∑’");
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("MapScene");

        yield return null;
    }

    void DiscardSymbol()
    {
        Symbol symbol = symbolScrollCtrl.GetCurSymbol();
        gameMgr.RemovePlayerSymbol(symbol);
    }
}
