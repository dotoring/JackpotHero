using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopMgr : MonoBehaviour
{
    GameMgr gameMgr;
    public SymbolScrollCtrl symbolScrollCtrl;
    public GameObject ownSymbolsPanel;

    public GameObject shopPanel;
    public Button shopOpenButton;

    public GameObject saleSymbolNodePref;
    public Transform saleLayout;

    public Button discardSelectBtn;
    public Button discardSymbolBtn;
    public TextMeshProUGUI discardPriceText;
    public int discardPrice;

    void Start()
    {
        gameMgr = GameMgr.Instance;

        GenSaleSymbols();

        if(shopOpenButton != null)
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
            GameObject saleSymbolNode = Instantiate(saleSymbolNodePref);
            saleSymbolNode.transform.SetParent(saleLayout, false);
            //saleSymbolNode.GetComponent<SaleSymbolNode>().symbol = saleSymbol;
            saleSymbolNode.GetComponentInChildren<SaleSymbolNode>().symbol = saleSymbol;
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
        gameMgr.useGold(discardPrice);
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
