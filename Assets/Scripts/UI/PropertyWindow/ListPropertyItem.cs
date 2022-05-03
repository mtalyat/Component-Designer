using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListPropertyItem : PropertyItem
{
    private IList currentlyLoadedList;

    [SerializeField]
    private Transform content;

    public override Type storedType => typeof(object);

    [SerializeField]
    private bool resizable = true;

    [SerializeField]
    private float minHeight = 0;

    private RectTransform rectTransform;

    [SerializeField]
    private ScrollRect scrollRect;

    [Space]

    [SerializeField]
    private Button addButton;
    [SerializeField]
    private Button removeButton;

    private List<PropertyItem> items = new List<PropertyItem>();

    private int lastEditedIndex = -1;

    private void Awake()
    {
        //event listeners added in initialize due to the nature of the window, as opposed to awake

        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        //if it is resizable, we do not want to scroll on it
        if (resizable)
        {
            scrollRect.enabled = false;
        }
    }

    public void Clear()
    {
        //clear the name
        SetNameText("--");

        //clear the list
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        //clear all event listeners since they're gone now
        ClearListeners();

        //do not clear permanent one

        removeButton.interactable = false;
        addButton.interactable = false;

        //and then reset the size
        Resize(0);
    }

    private void Resize(float additionalHeight)
    {
        if (!resizable) return;

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, minHeight + additionalHeight);
    }

    public override void Refresh(object data)
    {
        //for lists, we need to redo the whole thing in case items have been added/removed
        Initialize(storedName, data);
    }

    public override void Initialize(string name, object data)
    {
        Clear();

        if (data == null)
        {
            return;
        }

        //set name
        SetNameText(name);

        IList list = data as IList;

        currentlyLoadedList = list;

        if(list == null)
        {
            Resize(0);
            return;
        }

        //make sure the buttons are in order now, so even if there is nothing, we can still add to it
        addButton.interactable = true;
        removeButton.interactable = false;

        if (list.Count == 0)
        {
            //if here, list was empty
            Resize(0);
            return;
        }

        //get the type this list contains
        Type t = data.GetType().GetGenericArguments()[0];

        //get the first data object so we can ge the proper property item
        //all objects in the list will be the same, so we can rely on that

        //get the prefab to use
        PropertyItem prefab = GameManager.instance.GetPropertyItem(list[0]);

        float additionalHeight = 0f;

        int index = 0;

        foreach (var e in list)
        {
            //instantiate it, and then set the data
            PropertyItem item = Instantiate(prefab, content);
            item.Initialize(index.ToString(), e);

            //add to height for resizing
            additionalHeight += item.GetComponent<RectTransform>().rect.height;

            int i = index;

            //then tell it what to do when the value changes, since we want to update it
            item.AddListener((object d) =>
            {
                //set the value
                //propertyInfo.SetValue(data, d);
                if(i >= 0 && i < list.Count)
                {
                    list.GetType().GetProperty("Item").SetValue(list, e, new object[] { i });
                }

                lastEditedIndex = i;
                removeButton.interactable = true;

                //then notify this window
                OnChange(data);
            });

            //increment index
            index++;

            items.Add(item);
        }

        //resize, if able
        Resize(additionalHeight);
    }

    public override bool ConditionForSelection(object obj, Type other)
    {
        return other.IsGenericType && obj is IList;
    }

    //adds a new item
    public void Add()
    {
        //get the type this list contains
        Type t = currentlyLoadedList.GetType().GetGenericArguments()[0];

        //create a new instance of it
        object o = Activator.CreateInstance(t);

        //add it to the list
        currentlyLoadedList.Add(o);

        //reload list
        Refresh(currentlyLoadedList);
    }

    //removes the selected item
    public void Remove()
    {
        //only edit if there is a valid last edited index
        if(lastEditedIndex == -1)
        {
            return;
        }

        //remove
        currentlyLoadedList.RemoveAt(lastEditedIndex);

        //reload list
        Refresh(currentlyLoadedList);
    }
}
