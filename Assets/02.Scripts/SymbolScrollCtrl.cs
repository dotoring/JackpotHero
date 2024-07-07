using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolScrollCtrl : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;

    private List<RectTransform> items;
    private float itemHeight;

    private SymbolNode cursymbol;
    public int count = 1;

    void Start()
    {
        items = new List<RectTransform>();
        foreach (Transform child in content)
        {
            items.Add(child.GetComponent<RectTransform>());
        }

        if (items.Count > 0)
        {
            itemHeight = items[0].rect.height;
        }

        // Ensure content height is correct
        content.sizeDelta = new Vector2(content.sizeDelta.x, items.Count * itemHeight);

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y + itemHeight);
    }

    void Update()
    {
        if(content.anchoredPosition.y == 0)
        {
            cursymbol = items[2].GetComponent<SymbolNode>();
        }
        else if(content.anchoredPosition.y == itemHeight)
        {
            cursymbol = items[3].GetComponent<SymbolNode>();
        }
        else if(content.anchoredPosition.y ==  itemHeight * 2)
        {
            cursymbol = items[4].GetComponent<SymbolNode>();
        }

        // Handle user input for scrolling
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ScrollUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ScrollDown();
        }

        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput > 0)
        {
            ScrollUp();
        }
        else if(wheelInput < 0)
        {
            ScrollDown();
        }

        // Check for circular scrolling
        float contentPosition = content.anchoredPosition.y;
        if (contentPosition < 0)
        {
            MoveToStart();
        }
        else if (contentPosition > itemHeight*2)
        {
            MoveToEnd();
        }
    }

    public void ScrollUp()
    {
        Vector2 newPosition = content.anchoredPosition;
        newPosition.y = newPosition.y - itemHeight;
        content.anchoredPosition = newPosition;

        count--;
        if(count == 0)
        {
            count = GameMgr.Instance.GetPlayerOwnSymbols().Count;
        }
    }

    public void ScrollDown()
    {
        Vector2 newPosition = content.anchoredPosition;
        newPosition.y = newPosition.y + itemHeight;
        content.anchoredPosition = newPosition;

        count++;
        if (count == GameMgr.Instance.GetPlayerOwnSymbols().Count + 1)
        {
            count = 1;
        }
    }

    private void MoveToEnd()
    {
        RectTransform firstItem = items[0];
        items.RemoveAt(0);
        items.Add(firstItem);

        firstItem.SetAsLastSibling();

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y - itemHeight);
    }

    private void MoveToStart()
    {
        RectTransform lastItem = items[items.Count - 1];
        items.RemoveAt(items.Count - 1);
        items.Insert(0, lastItem);

        lastItem.SetAsFirstSibling();

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y + itemHeight);
    }

    public void RefreshSymbolScroll()
    {
        items = new List<RectTransform>();
        foreach (Transform child in content)
        {
            items.Add(child.GetComponent<RectTransform>());
        }

        // Ensure content height is correct
        content.sizeDelta = new Vector2(content.sizeDelta.x, items.Count * itemHeight);

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, content.anchoredPosition.y + itemHeight);
    }

    public Symbol GetCurSymbol()
    {
        if(cursymbol != null)
        {
            return cursymbol.symbol;
        }
        return null;
    }
}
