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
                SceneManager.LoadScene("BattleScene");
                break;
            case PlaceType.Event:
                SceneManager.LoadScene("EventTempScene");
                break;
            case PlaceType.Elite:
                SceneManager.LoadScene("BattleScene");
                break;
            case PlaceType.Rest:
                SceneManager.LoadScene("RestTempScene");
                break;
            case PlaceType.Shop:
                SceneManager.LoadScene("ShopTempScene");
                break;
            case PlaceType.Treasure:
                SceneManager.LoadScene("TreasureTempScene");
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
}
