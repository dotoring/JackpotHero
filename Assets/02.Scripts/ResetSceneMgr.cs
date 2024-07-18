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
            discardSymbolBtn.onClick.AddListener(() =>
            {
                StartCoroutine(DiscardSymbol());
            });
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
        if(!ownSymbolsPanel.activeSelf)
        {
            discardSymbolBtn.gameObject.SetActive(false);
        }
    }

    IEnumerator Recover()
    {
        gameMgr.HealPlayer((int)(gameMgr.GetPlayerMaxHP() / 2));

        discardSelectBtn.interactable = false;
        recoverBtn.interactable = false;
        //회복 애니메이션
        Debug.Log("뾰로롱");
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("MapScene");

        yield return null;
    }

    IEnumerator DiscardSymbol()
    {
        Symbol symbol = symbolScrollCtrl.GetCurSymbol();
        gameMgr.RemovePlayerSymbol(symbol);

        ownSymbolsPanel.SetActive(false);
        discardSelectBtn.interactable = false;
        recoverBtn.interactable = false;

        //심볼 삭제 애니메이션
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("MapScene");

        yield return null;
    }
}
