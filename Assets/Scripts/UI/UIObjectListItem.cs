using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIObjectListItem : MonoBehaviour, IPointerDownHandler
{
    public TMP_Text nameText;

    [HideInInspector]
    public int index;
    [HideInInspector]
    public UIObjectList list;

    public void Initialize(string objectName, int i, UIObjectList l)
    {
        nameText.text = objectName;
        index = i;
        list = l;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        list.Select(index);
    }

    public void Rename(string newName)
    {
        nameText.text = newName;
    }
}
