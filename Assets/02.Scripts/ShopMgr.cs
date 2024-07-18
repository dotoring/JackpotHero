using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopMgr : MonoBehaviour
{
    GameMgr gameMgr;
    public GameObject shopPanel;
    public Button shopOpenButton;

    [Header("Symbol")]
    public SymbolScrollCtrl symbolScrollCtrl;
    public GameObject ownSymbolsPanel;

    public GameObject symbolSaleNodePref;
    public Transform symbolSaleLayout;

    public Button discardSelectBtn;
    public Button discardSymbolBtn;
    public TextMeshProUGUI discardPriceText;
    public int discardPrice;

    [Header("Artifact")]
    public GameObject artifactSaleNodePref;
    public Transform artifactSaleLayout;

    [Header("UI")]
    public TextMeshProUGUI playerHp;
    public TextMeshProUGUI playerBarrier;
    public GameObject playerBarrierUI;

    void Start()
    {
        gameMgr = GameMgr.Instance;

        GenSaleSymbols();
        GenSaleArtifacts();

        //<<<vip카드효과
        if(gameMgr.vipCard)
        {
            discardPrice = (int)(discardPrice * 0.8f);
        }
        //vip카드효과>>>

        if (shopOpenButton != null)
        {
            shopOpenButton.onClick.AddListener(() =>
            {
                shopPanel.SetActive(true);
            });
        }

        if (discardSymbolBtn != null)
        {
            discardSymbolBtn.onClick.AddListener(() =>
            {
                StartCoroutine(DiscardSymbol());
            });
        }

        if (discardSelectBtn != null)
        {
            discardSelectBtn.onClick.AddListener(ShowDiscardSymbolPanel);
        }
    }

    void Update()
    {
        discardPriceText.text = discardPrice.ToString();

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

        if (!ownSymbolsPanel.activeSelf)
        {
            discardSymbolBtn.gameObject.SetActive(false);
        }
    }

    void GenSaleSymbols()
    {
        //판매 심볼 뽑기
        List<Symbol> sales = new List<Symbol>();

        List<Symbol> entireSymbols = new List<Symbol>(gameMgr.GetEntireSymbols());
        for (int i = 0; i < 4;) //상점은 4개 제공
        {
            int ran = Random.Range(0, entireSymbols.Count);
            if (sales.Contains(entireSymbols[ran])) //중복이면 다시 뽑기
            {
                continue;
            }
            else
            {
                sales.Add(entireSymbols[ran]);
                i++;
            }
        }

        //뽑은 판매 심볼들 생성
        foreach (Symbol saleSymbol in sales)
        {
            GameObject saleSymbolNode = Instantiate(symbolSaleNodePref);
            saleSymbolNode.transform.SetParent(symbolSaleLayout, false);
            saleSymbolNode.GetComponentInChildren<SaleSymbolNode>().symbol = saleSymbol;
        }
    }

    void GenSaleArtifacts()
    {
        //판매 유물 뽑기
        List<Artifact> sales = new List<Artifact>();
        List<Artifact> entireArtifacts = new List<Artifact>(gameMgr.GetAvailableArtifacts());
        for (int i = 0; i < 4;) //상점은 4개 제공
        {
            int ran = Random.Range(0, entireArtifacts.Count);
            if (sales.Contains(entireArtifacts[ran])) //중복이면 다시 뽑기
            {
                continue;
            }
            else
            {
                sales.Add(entireArtifacts[ran]);
                i++;
            }
        }

        //뽑은 판매 유물들 생성
        foreach (Artifact artifact in sales)
        {
            GameObject artifactSaleNode = Instantiate(artifactSaleNodePref);
            artifactSaleNode.transform.SetParent(artifactSaleLayout, false);
            artifactSaleNode.GetComponentInChildren<SaleArtifactNode>().artifact = artifact;
        }
    }

    void ShowDiscardSymbolPanel()
    {
        //돈이 충분 하면 제거 패널 열기
        if(gameMgr.GetGoldAmount() >= discardPrice)
        {
            ownSymbolsPanel.SetActive(true);
            discardSymbolBtn.gameObject.SetActive(true);
        }
    }

    IEnumerator DiscardSymbol()
    {
        //돈 차감 후 심볼 제거
        gameMgr.UseGold(discardPrice);
        Symbol symbol = symbolScrollCtrl.GetCurSymbol();
        gameMgr.RemovePlayerSymbol(symbol);

        ownSymbolsPanel.SetActive(false);

        //심볼 제거 비용 증가 2배씩
        discardPrice *= 2;

        //심볼 삭제 애니메이션
        yield return new WaitForSeconds(1.0f);
        yield return null;
    }
}
