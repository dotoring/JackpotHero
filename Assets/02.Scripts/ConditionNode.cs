using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConditionNode : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI count;

    public void SetNode(Sprite sprite, int val)
    {
        image.sprite = sprite;
        count.text = val.ToString();
    }
}
