using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetSceneMgr : MonoBehaviour
{
    GameMgr gameMgr;

    public Button recoverBtn;
    public Button discardBtn;

    public GameObject SymbolListPanel;

    [Header("UI")]
    public TextMeshProUGUI playerHp;
    public TextMeshProUGUI playerBarrier;
    public GameObject playerBarrierUI;
    public Text playerGoldText;


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

        if(discardBtn != null)
        {
            discardBtn.onClick.AddListener(ClickDiscardBtn);
        }
    }

    void Update()
    {
        playerHp.text = gameMgr.GetPlayerCurHP().ToString();
        playerGoldText.text = gameMgr.GetGoldAmount().ToString();
        playerBarrier.text = gameMgr.GetBarrier().ToString();
        if (gameMgr.GetBarrier() > 0)
        {
            playerBarrierUI.SetActive(true);
        }
        else
        {
            playerBarrierUI.SetActive(false);
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

    void ClickDiscardBtn()
    {
        SymbolListPanel.SetActive(true);
    }
}
