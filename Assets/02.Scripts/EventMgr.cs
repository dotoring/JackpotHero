using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum SelectType
{
    Discard,
    Copy,
}

public class EventMgr : MonoBehaviour
{
    GameMgr gameMgr;
    public List<Event> events = new List<Event>();

    public TextMeshProUGUI eventMainText;
    public Button[] choiceBtn;
    Event curEvent;
    int choiceIndex;
    public Button nextBtn;

    [Header("For Select Symbol")]
    public GameObject symbolsSelectPanel;
    public Transform ownSymbolLayout;
    public GameObject symbolNodePref;
    public SymbolScrollCtrl symbolScrollCtrl;
    public TextMeshProUGUI symbolCount;
    public Button symbolDiscardBtn;

    [Space(10f)]
    public TextMeshProUGUI symbolNameText;
    public TextMeshProUGUI symbolDmgText;
    public TextMeshProUGUI symbolPairEffectText;
    public TextMeshProUGUI symbolPerfectEffectText;

    public SelectType selectType;

    void Start()
    {
        gameMgr = GameMgr.Instance;
        GenerateOwnSymbols();

        //이벤트 선택
        curEvent = GetRandomEvent();

        //이벤트 메인 텍스트 출력
        eventMainText.text = curEvent.description;

        //이벤트 선택지들 출력
        for (int i = 0; i < choiceBtn.Length; i++)
        {
            if (i < curEvent.choices.Count)
            {
                //선택지 활성화
                choiceBtn[i].gameObject.SetActive(true);
                choiceBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = curEvent.choices[i].choiceText;
                int choiceIndex = i; // 클로저 문제 해결을 위한 로컬 변수
                //선택지 클릭 시 효과 발동
                choiceBtn[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            }
            else
            {
                choiceBtn[i].gameObject.SetActive(false);
            }
        }

        if(symbolDiscardBtn != null)
        {
            symbolDiscardBtn.onClick.AddListener(() =>
            {
                if (selectType == SelectType.Discard)
                {
                    StartCoroutine(DiscardSymbol());
                }
                else if(selectType == SelectType.Copy)
                {
                    StartCoroutine(CopySymbol());
                }
            });
        }
    }

    void Update()
    {
        if (symbolsSelectPanel.activeSelf)
        {
            ShowSymbolInformation();
        }
        CountSymbol();
    }

    //랜덤 이벤트 선택 함수
    Event GetRandomEvent()
    {
        int randomIndex = Random.Range(0, events.Count);
        return events[randomIndex];
    }

    //선택지에 따른 효과 발동
    void OnChoiceSelected(int index)
    {
        curEvent.choices[index].action.Invoke();
        choiceIndex = index;

        StartCoroutine(EndEvent());
    }

    IEnumerator EndEvent()
    {
        for (int i = 0; i < choiceBtn.Length; i++)
        {
            choiceBtn[i].gameObject.SetActive(false);
        }

        eventMainText.text = curEvent.resultTexts[choiceIndex];
        yield return new WaitForSeconds(0.5f);

        nextBtn.gameObject.SetActive(true);
        yield return null;
    }

    public void ShowSymbolSelectPanel()
    {
        symbolsSelectPanel.SetActive(true);
    }

    public void GenerateOwnSymbols()
    {
        foreach (Transform child in ownSymbolLayout)
        {
            Destroy(child.gameObject);
        }

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

    IEnumerator DiscardSymbol()
    {
        //심볼 제거
        Symbol symbol = symbolScrollCtrl.GetCurSymbol();
        gameMgr.RemovePlayerSymbol(symbol);

        symbolsSelectPanel.SetActive(false);

        //심볼 삭제 애니메이션
        yield return new WaitForSeconds(1.0f);
        yield return null;
    }

    IEnumerator CopySymbol()
    {
        //심볼 복사
        Symbol symbol = symbolScrollCtrl.GetCurSymbol();
        gameMgr.AddPlayerSymbol(symbol);

        symbolsSelectPanel.SetActive(false);

        //심볼 추가 애니메이션
        yield return new WaitForSeconds(1.0f);
        yield return null;
    }
}
