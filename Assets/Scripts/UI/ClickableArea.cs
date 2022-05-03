using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableArea : MonoBehaviour
{
    public static ClickableArea instance;

    public bool isInArea => IsMouseInArea();

    private RectTransform rectTransform;

    private void Awake()
    {
        instance = this;

        rectTransform = GetComponent<RectTransform>();
    }

    private bool IsMouseInArea()
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition);
    }
}
