using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PlaceNode : MonoBehaviour
{
    Button b;
    Image img;
    public Place place;

    //0: battle, 1: event, 2: elite, 3: rest, 4: shop, 5: treasure
    [SerializeField] Sprite[] sprites;

    void Start()
    {
        b = GetComponent<Button>();
        img = GetComponent<Image>();

        ChangePlaceImg();

        if (b != null)
        {
            b.onClick.AddListener(ChangeScene);
        }
    }

    void ChangeScene()
    {
        PlayData.curPlace = place;
        switch (place.placeType)
        {
            case PlaceType.Battle:
                GameMgr.Instance.isElite = false;
                SceneManager.LoadScene("BattleScene");
                break;
            case PlaceType.Event:
                switch(GetRandomResult())
                {
                    case 0:
                        SceneManager.LoadScene("EventScene");
                        break;
                    case 1:
                        SceneManager.LoadScene("BattleScene");
                        break;
                    case 2:
                        SceneManager.LoadScene("ShopScene");
                        break;
                    case 3:
                        SceneManager.LoadScene("TreasureScene");
                        break;
                }
                break;
            case PlaceType.Elite:
                GameMgr.Instance.isElite = true;
                SceneManager.LoadScene("BattleScene");
                break;
            case PlaceType.Rest:
                SceneManager.LoadScene("RestScene");
                break;
            case PlaceType.Shop:
                SceneManager.LoadScene("ShopScene");
                break;
            case PlaceType.Treasure:
                SceneManager.LoadScene("TreasureScene");
                break;
        }
    }

    void ChangePlaceImg()
    {
        switch (place.placeType)
        {
            case PlaceType.Battle:
                img.sprite = sprites[0];
                break;
            case PlaceType.Event:
                img.sprite = sprites[1];
                break;
            case PlaceType.Elite:
                img.sprite = sprites[2];
                break;
            case PlaceType.Rest:
                img.sprite = sprites[3];
                break;
            case PlaceType.Shop:
                img.sprite = sprites[4];
                break;
            case PlaceType.Treasure:
                img.sprite = sprites[5];
                break;
        }
    }

    public int GetRandomResult()
    {
        //가중치 설정
        int Event = 5;
        int Enemy = 1;
        int Shop = 1;
        int Treasure = 1;
        int[] weights = { Event, Enemy, Shop, Treasure};  // 각각의 결과에 대한 가중치

        int totalWeight = 0;
        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return i;
            }
        }

        return 0;
    }
}
