using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapMgrTemp : MonoBehaviour
{
    public Button button;

    void Start()
    {
        if(button != null)
        {
            button.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("BattleScene");
            });
        }
    }

    void Update()
    {
        
    }
}
