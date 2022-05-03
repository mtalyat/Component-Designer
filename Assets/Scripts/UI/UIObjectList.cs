using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIObjectList : MonoBehaviour
{
    public TMP_Text titleText;

    public RectTransform selector;
    public UIObjectListItem listItemPrefab;
    public Transform content;

    public UnityEvent<int> onSelectionChange;
    public int selectedIndex { get; private set; } = -1;
    public int length => content.childCount;

    private object[] displayedObjects;

    public void SetTitle(string title)
    {
        titleText.text = title;
    }

    public void Clear()
    {
        Set(new object[0]);
        Select(-1);
    }

    public void Set(object[] os)
    {
        //for each object, instantiate a prefab and display it. Then when the object is selected, we call the event

        //set parent of selector to null so it isn't accidentally deleted
        selector.SetParent(transform);

        if(content.childCount > 0)
        {
            //first destroy old ones
            for (int i = content.childCount - 1; i >= 0; i--)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < os.Length; i++)
        {
            UIObjectListItem item = Instantiate(listItemPrefab, content);
            item.Initialize(os[i].ToString(), i, this);
        }

        displayedObjects = os;

        RemoveSelection();
    }

    public void Select(int index)
    {
        if (index < 0)
        {
            RemoveSelection();
        } else
        {
            //set index
            selectedIndex = index;
            
            //move selector
            selector.SetParent(content.GetChild(index));
            selector.localPosition = Vector3.zero;
            selector.offsetMax = Vector2.zero;
            selector.offsetMin = Vector2.zero;
            if (!selector.gameObject.activeSelf)
            {
                selector.gameObject.SetActive(true);
            }

            //notify the other methods
            onSelectionChange.Invoke(index);
        }
    }

    private void RemoveSelection()
    {
        //set index
        selectedIndex = -1;

        //hide selector
        selector.SetParent(transform);
        selector.gameObject.SetActive(false);

        //notify
        onSelectionChange.Invoke(-1);
    }

    public void UpdateSelected()
    {
        if(selectedIndex >= 0)
        {
            content.GetChild(selectedIndex).GetComponent<UIObjectListItem>().Rename(displayedObjects[selectedIndex].ToString());
        }
    }

    public void Reload()
    {
        StartCoroutine(ReloadEnumerator());
    }

    private IEnumerator ReloadEnumerator()
    {
        int index = selectedIndex;

        Set(displayedObjects);

        yield return null;

        Select(index);
    }
}
