using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactTooltip : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public LayoutGroup layoutGroup;
    RectTransform canvasRectTransform;
    RectTransform tooltipRectTransform;
    Canvas canvas;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        tooltipRectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // 툴팁이 활성화되어 있으면 마우스 위치로 업데이트합니다.
        if (gameObject.activeSelf)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, canvas.worldCamera, out localPoint);

            if (tooltipRectTransform.pivot.y == 1)
            {
                localPoint.y -= 25.0f;
                tooltipRectTransform.localPosition = localPoint;
            }
            else
            {
                tooltipRectTransform.localPosition = localPoint;
            }
        }
    }

    public void SetArtifactNameText(string t)
    {
        nameText.text = t;
    }

    public void SetArtifactDescriptionText(string t)
    {
        descriptionText.text = t;
    }

    public void UpdateLayout()
    {
        //레이아웃 크기 즉시 업데이트
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
    }
}
