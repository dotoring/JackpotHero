using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaceNode : MonoBehaviour
{
    Button b;
    public Place place;

    void Start()
    {
        b = GetComponent<Button>();

        if (b != null)
        {
            b.onClick.AddListener(NextNodeOn);
        }
    }

    void NextNodeOn()
    {
        PlayData.curPlace = place;
        SceneManager.LoadScene("BattleScene");
    }
}
