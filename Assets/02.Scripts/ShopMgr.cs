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

        //<<<vipī��ȿ��
        if(gameMgr.vipCard)
        {
            discardPrice = (int)(discardPrice * 0.8f);
        }
        //vipī��ȿ��>>>

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
        //�Ǹ� �ɺ� �̱�
        List<Symbol> sales = new List<Symbol>();

        List<Symbol> entireSymbols = new List<Symbol>(gameMgr.GetEntireSymbols());
        for (int i = 0; i < 4;) //������ 4�� ����
        {
            int ran = Random.Range(0, entireSymbols.Count);
            if (sales.Contains(entireSymbols[ran])) //�ߺ��̸� �ٽ� �̱�
            {
                continue;
            }
            else
            {
                sales.Add(entireSymbols[ran]);
                i++;
            }
        }

        //���� �Ǹ� �ɺ��� ����
        foreach (Symbol saleSymbol in sales)
        {
            GameObject saleSymbolNode = Instantiate(symbolSaleNodePref);
            saleSymbolNode.transform.SetParent(symbolSaleLayout, false);
            saleSymbolNode.GetComponentInChildren<SaleSymbolNode>().symbol = saleSymbol;
        }
    }

    void GenSaleArtifacts()
    {
        //�Ǹ� ���� �̱�
        List<Artifact> sales = new List<Artifact>();
        List<Artifact> entireArtifacts = new List<Artifact>(gameMgr.GetAvailableArtifacts());
        for (int i = 0; i < 4;) //������ 4�� ����
        {
            int ran = Random.Range(0, entireArtifacts.Count);
            if (sales.Contains(entireArtifacts[ran])) //�ߺ��̸� �ٽ� �̱�
            {
                continue;
            }
            else
            {
                sales.Add(entireArtifacts[ran]);
                i++;
            }
        }

        //���� �Ǹ� ������ ����
        foreach (Artifact artifact in sales)
        {
            GameObject artifactSaleNode = Instantiate(artifactSaleNodePref);
            artifactSaleNode.transform.SetParent(artifactSaleLayout, false);
            artifactSaleNode.GetComponentInChildren<SaleArtifactNode>().artifact = artifact;
        }
    }

    void ShowDiscardSymbolPanel()
    {
        //���� ��� �ϸ� ���� �г� ����
        if(gameMgr.GetGoldAmount() >= discardPrice)
        {
            ownSymbolsPanel.SetActive(true);
            discardSymbolBtn.gameObject.SetActive(true);
        }
    }

    IEnumerator DiscardSymbol()
    {
        //�� ���� �� �ɺ� ����
        gameMgr.UseGold(discardPrice);
        Symbol symbol = symbolScrollCtrl.GetCurSymbol();
        gameMgr.RemovePlayerSymbol(symbol);

        ownSymbolsPanel.SetActive(false);

        //�ɺ� ���� ��� ���� 2�辿
        discardPrice *= 2;

        //�ɺ� ���� �ִϸ��̼�
        yield return new WaitForSeconds(1.0f);
        yield return null;
    }
}
