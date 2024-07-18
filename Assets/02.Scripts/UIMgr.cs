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
        RefreshOwnSymbols();

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

    public void RefreshOwnSymbols()
    {
        StartCoroutine(GenerateOwnSymbols());
    }

    //보유 심볼 생성 함수
    public IEnumerator GenerateOwnSymbols()
    {
        foreach(Transform child in ownSymbolLayout)
        {
            Destroy(child.gameObject);
        }

        yield return new WaitForEndOfFrame();

        //무한히 회전하는 연출을 위해 보유 심볼 갯수에 따른 추가 생성
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

        yield return null;
    }

    //보유 심볼 확인 버튼
    public void ShowSymbols()
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

    //유물 표시
    public void RefreshOwnArtifacts()
    {
        //있던 오브젝트들 지우기
        foreach(Transform child in ownArtifactGridLayout)
        {
            Destroy(child.gameObject);
        }

        //보유 유물들 다시 생성하기
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
