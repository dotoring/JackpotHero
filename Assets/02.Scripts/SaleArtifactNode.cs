using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaleArtifactNode : ArtifactNode
{
    Button button;
    public TextMeshProUGUI priceText;

    void Start()
    {
        button = GetComponent<Button>();
        img = GetComponent<Image>();

        img.sprite = artifact.GetArtifactSprite();

        uiMgr = GameObject.Find("UIMgr").GetComponent<UIMgr>();

        //���� ����
        int price;
        //<<<vipī�� ȿ��
        if (GameMgr.Instance.vipCard)
        {
            price = (int)(artifact.GetArtifactPrice() * 0.8f);
            //vipī�� ȿ��>>>
        }
        else
        {
            price = artifact.GetArtifactPrice();
        }
        priceText.text = price.ToString();

        if (button != null)
        {
            button.onClick.AddListener(BuyArtifact);
        }
    }

    void BuyArtifact()
    {
        if (GameMgr.Instance.GetGoldAmount() >= artifact.GetArtifactPrice())
        {
            GameMgr.Instance.AddArtifact(artifact);
            GameMgr.Instance.useGold(artifact.GetArtifactPrice());

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("�� ����");
        }
    }
}
