using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class PropertyWindow : PropertyItem
{
    private object currentlyLoadedData;

    [SerializeField]
    private Transform content;

    [SerializeField]
    private TMP_InputField nameInput;
    public string nameInputText => nameInput.text;

    public override Type storedType => typeof(object);

    [SerializeField]
    private bool resizable = true;

    [SerializeField]
    private bool renamable = false;

    [SerializeField]
    private bool reverse = false;

    [SerializeField]
    private float minHeight = 0;

    private RectTransform rectTransform;

    [SerializeField]
    private ScrollRect scrollRect;

    private List<PropertyItem> items = new List<PropertyItem>();

    public bool hasLoadedData => currentlyLoadedData != null;

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
        nameInput.SetTextWithoutNotify("--");
        nameInput.readOnly = true;

        items.Clear();

        //clear the list
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        //clear all event listeners since they're gone now
        ClearListeners();

        //do not clear permanent one

        //and then reset the size
        Resize(0);

        currentlyLoadedData = null;
    }

    private void Resize(float additionalHeight)
    {
        if (!resizable) return;

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, minHeight + additionalHeight);
    }

    public override void Refresh(object data)
    {
        //make sure there is data to refresh
        if (currentlyLoadedData == null || currentlyLoadedData != data) return;

        //get all properties
        Type t = data.GetType();

        PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (reverse)
        {
            properties = properties.Reverse().ToArray();
        }

        int index = 0;
        foreach (PropertyInfo propertyInfo in properties)
        {
            //if cannot read and write, move to the next one
            if (!propertyInfo.CanRead || !propertyInfo.CanWrite) continue;

            //and only continue if it has EditablePropertyAttribute
            if (!Attribute.IsDefined(propertyInfo, typeof(EditablePropertyAttribute))) continue;

            object val = propertyInfo.GetValue(data);

            //if null for some reason, skip
            if (val == null) continue;

            //get the item
            PropertyItem item = items[index];

            //refresh it
            item.Refresh(val);

            index++;
        }
    }

    public override void Initialize(string name, object data)
    {
        Clear();

        if (data == null)
        {
            return;
        }

        currentlyLoadedData = data;

        //set name
        nameInput.text = name;
        nameInput.readOnly = !renamable;

        //get all properties
        Type t = data.GetType();

        float additionalHeight = 0f;

        PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if(reverse)
        {
            properties = properties.Reverse().ToArray();
        }

        foreach (PropertyInfo propertyInfo in properties)
        {
            //if cannot read and write, move to the next one
            if (!propertyInfo.CanRead || !propertyInfo.CanWrite) continue;

            //and only continue if it has EditablePropertyAttribute
            if (!Attribute.IsDefined(propertyInfo, typeof(EditablePropertyAttribute))) continue;

            object val = propertyInfo.GetValue(data);

            //if null for some reason, skip
            if (val == null) continue;

            //create a corresponding property item
            PropertyItem prefab = GameManager.instance.GetPropertyItem(val);

            //instantiate it, and then set the data
            PropertyItem item = Instantiate(prefab, content);
            item.Initialize(propertyInfo.Name, val);

            //add to height for resizing
            additionalHeight += item.GetComponent<RectTransform>().rect.height;

            //then tell it what to do when the value changes, since we want to update it
            item.AddListener((object d) =>
            {
                //set the value
                propertyInfo.SetValue(data, d);

                //then notify this window
                OnChange(data);
            });

            items.Add(item);
        }

        //resize, if able
        Resize(additionalHeight);
    }

    public void AddListenerToNameInput(UnityEngine.Events.UnityAction<string> call)
    {
        nameInput.onValueChanged.AddListener(call);
    }
}
