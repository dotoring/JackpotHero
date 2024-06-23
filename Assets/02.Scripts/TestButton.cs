using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        if(button != null )
        {
            button.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("MapScene");
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
