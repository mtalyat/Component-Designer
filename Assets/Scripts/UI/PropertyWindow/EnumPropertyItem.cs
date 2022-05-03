using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class EnumPropertyItem : PropertyItem
{
    [SerializeField]
    private TMP_Dropdown enumInput;

    private Type t;

    public override Type storedType => typeof(Enum);

    private void Awake()
    {
        enumInput.onValueChanged.AddListener((int i) => OnChange(i));
    }

    public override void Refresh(object data)
    {
        //get the list to populate the dropdown
        Type newT = data.GetType();
        if (t == null || newT != t)
        {
            //new type, populate accordingly
            t = newT;

            t = data.GetType();
            List<string> names = Enum.GetNames(t).ToList();
            enumInput.ClearOptions();
            enumInput.AddOptions(names);

            //select the one that we were given
            enumInput.value = names.IndexOf(data.ToString());
        } else
        {
            List<string> names = Enum.GetNames(t).ToList();

            //select the one that we were given
            enumInput.value = names.IndexOf(data.ToString());
        }

        
    }

    public override bool ConditionForSelection(object obj, Type other)
    {
        return other.IsEnum;
    }
}
