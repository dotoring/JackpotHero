using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    GameMgr gameMgr;

    [Header("Symbols")]
    public GameObject ownSymbolsPanel;
    public Button symbolsInventoryBtn;
    public Button inventoryCloseBtn;
    public Transform ownSymbolLayout;
    public GameObject symbolNodePref;
    public SymbolScrollCtrl symbolScrollCtrl;
    public TextMeshProUGUI symbolCount;

    [Space (10f)]
    public TextMeshProUGUI symbolNameText;
    public TextMeshProUGUI symbolDmgText;
    public TextMeshProUGUI symbolPairEffectText;
    public TextMeshProUGUI symbolPerfectEffectText;

    [Header("Gold")]
    public TextMeshProUGUI goldText;

    [Header("Artifacts")]
    public GameObject artifactNodePref;
    public Transform ownArtifactGridLayout;
    public GameObject artifactTooltipGO;


    void Start()
    {
        gameMgr = GameMgr.Instance;

        RefreshOwnArtifacts();
        GenerateOwnSymbols();

        if (symbolsInventoryBtn != null)
        {
            symbolsInventoryBtn.onClick.AddListener(ShowSymbols);
        }

        if (inventoryCloseBtn != null)
        {
            inventoryCloseBtn.onClick.AddListener(ShowSymbols);
        }
    }

    void Update()
    {
        goldText.text = gameMgr.GetGoldAmount().ToString();
        if(ownSymbolsPanel.activeSelf)
        {
            ShowSymbolInformation();
        }

        CountSymbol();
    }

    //���� �ɺ� ���� �Լ�
    void GenerateOwnSymbols()
    {
        List<Symbol> Symbols = new List<Symbol>(gameMgr.GetPlayerOwnSymbols());
        if (Symbols.Count < 4)
        {
            foreach (Symbol symbol in Symbols)
            {
                GameObject symbolNode = Instantiate(symbolNodePref);
                symbolNode.transform.SetParent(ownSymbolLayout, false);
                symbolNode.GetComponent<SymbolNode>().symbol = symbol;
            }
        }
        if (Symbols.Count < 7)
        {
            foreach (Symbol symbol in Symbols)
            {
                GameObject symbolNode = Instantiate(symbolNodePref);
                symbolNode.transform.SetParent(ownSymbolLayout, false);
                symbolNode.GetComponent<SymbolNode>().symbol = symbol;
            }
        }
        foreach (Symbol symbol in Symbols)
        {
            GameObject symbolNode = Instantiate(symbolNodePref);
            symbolNode.transform.SetParent(ownSymbolLayout, false);
            symbolNode.GetComponent<SymbolNode>().symbol = symbol;
        }
        symbolScrollCtrl.RefreshSymbolScroll();
    }

    //���� �ɺ� Ȯ�� ��ư
    void ShowSymbols()
    {
        if (ownSymbolsPanel.activeSelf)
        {
            ownSymbolsPanel.SetActive(false);
        }
        else
        {
            ownSymbolsPanel.SetActive(true);
        }
    }

    void ShowSymbolInformation()
    {
        Symbol symbol = symbolScrollCtrl.GetCurSymbol();

        if (symbol != null)
        {
            symbolNameText.text = symbol.symbolName;
            symbolDmgText.text = symbol.basicDmg.ToString();
            symbolPairEffectText.text = symbol.symbolPairDescription;
            symbolPerfectEffectText.text = symbol.symbolPerfectDescription;
        }
    }

    void CountSymbol()
    {
        symbolCount.text = symbolScrollCtrl.count + "/" + (gameMgr.GetPlayerOwnSymbols().Count);
    }

    //���� ǥ��
    public void RefreshOwnArtifacts()
    {
        //�ִ� ������Ʈ�� �����
        foreach(Transform child in ownArtifactGridLayout)
        {
            Destroy(child.gameObject);
        }

        //���� ������ �ٽ� �����ϱ�
        foreach (Artifact artifact in gameMgr.GetArtifacts())
        {
            GameObject artifactNode = Instantiate(artifactNodePref);
            artifactNode.GetComponent<ArtifactNode>().artifact = artifact;
            artifactNode.transform.SetParent(ownArtifactGridLayout, false);
        }
    }

    public void ShowAndSetArtifactTooltip(string name, string desc)
    {
        artifactTooltipGO.SetActive(true);
        ArtifactTooltip artifactTooltip = artifactTooltipGO.GetComponent<ArtifactTooltip>();
        artifactTooltip.SetArtifactNameText(name);
        artifactTooltip.SetArtifactDescriptionText(desc);
        artifactTooltip.UpdateLayout();
    }

    public void HideArtifactTooltip()
    {
        artifactTooltipGO.SetActive(false);
    }
}
