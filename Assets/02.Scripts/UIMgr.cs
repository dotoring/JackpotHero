using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    GameMgr gameMgr;

    [Header("Symbols")]
    public GameObject ownSymbolsPanel;
    public Button symbolsInventoryBtn;
    public GameObject ownSymbolGridLayout;
    public GameObject symbolNodePref;
    public SymbolScrollCtrl symbolScrollCtrl;

    [Space (10f)]
    public TextMeshProUGUI symbolNameText;
    public TextMeshProUGUI symbolDmgText;
    public TextMeshProUGUI symbolPairEffectText;
    public TextMeshProUGUI symbolPerfectEffectText;

    [Header("Gold")]
    public TextMeshProUGUI goldText;

    [Header("Artifacts")]
    public GameObject artifactNodePref;
    public GameObject ownArtifactGridLayout;

    void Start()
    {
        gameMgr = GameMgr.Instance;

        ShowArtifacts();
        GenerateOwnSymbols();

        if (symbolsInventoryBtn != null)
        {
            symbolsInventoryBtn.onClick.AddListener(ShowSymbols);
        }
    }

    void Update()
    {
        goldText.text = gameMgr.GetGoldAmount().ToString();
        if(ownSymbolsPanel.activeSelf)
        {
            ShowSymbolInformation();
        }
    }

    //보유 심볼 생성 함수
    void GenerateOwnSymbols()
    {
        List<Symbol> Symbols = new List<Symbol>(gameMgr.GetPlayerOwnSymbols());
        foreach (Symbol symbol in Symbols)
        {
            GameObject symbolNode = Instantiate(symbolNodePref);
            symbolNode.transform.SetParent(ownSymbolGridLayout.transform, false);
            symbolNode.GetComponent<SymbolNode>().symbol = symbol;
        }
        symbolScrollCtrl.RefreshSymbolScroll();
    }

    //보유 심볼 확인 버튼
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

    //유물 표시
    void ShowArtifacts()
    {
        foreach (Artifact artifact in gameMgr.GetArtifacts())
        {
            GameObject artifactNode = Instantiate(artifactNodePref);
            artifactNode.GetComponent<ArtifactNode>().artifact = artifact;
            artifactNode.transform.SetParent(ownArtifactGridLayout.transform, false);
        }
    }
}
