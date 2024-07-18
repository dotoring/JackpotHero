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

        //�̺�Ʈ ����
        curEvent = GetRandomEvent();

        //�̺�Ʈ ���� �ؽ�Ʈ ���
        eventMainText.text = curEvent.description;

        //�̺�Ʈ �������� ���
        for (int i = 0; i < choiceBtn.Length; i++)
        {
            if (i < curEvent.choices.Count)
            {
                //������ Ȱ��ȭ
                choiceBtn[i].gameObject.SetActive(true);
                choiceBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = curEvent.choices[i].choiceText;
                int choiceIndex = i; // Ŭ���� ���� �ذ��� ���� ���� ����
                //������ Ŭ�� �� ȿ�� �ߵ�
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

    //���� �̺�Ʈ ���� �Լ�
    Event GetRandomEvent()
    {
        int randomIndex = Random.Range(0, events.Count);
        return events[randomIndex];
    }

    //�������� ���� ȿ�� �ߵ�
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

        //������ ȸ���ϴ� ������ ���� ���� �ɺ� ������ ���� �߰� ����
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
        //�ɺ� ����
        Symbol symbol = symbolScrollCtrl.GetCurSymbol();
        gameMgr.RemovePlayerSymbol(symbol);

        symbolsSelectPanel.SetActive(false);

        //�ɺ� ���� �ִϸ��̼�
        yield return new WaitForSeconds(1.0f);
        yield return null;
    }

    IEnumerator CopySymbol()
    {
        //�ɺ� ����
        Symbol symbol = symbolScrollCtrl.GetCurSymbol();
        gameMgr.AddPlayerSymbol(symbol);

        symbolsSelectPanel.SetActive(false);

        //�ɺ� �߰� �ִϸ��̼�
        yield return new WaitForSeconds(1.0f);
        yield return null;
    }
}
