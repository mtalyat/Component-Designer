using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class BoolPropertyItem : PropertyItem
{
    [SerializeField]
    private Toggle toggle;

    public override Type storedType => typeof(bool);

    private void Awake()
    {
        toggle.onValueChanged.AddListener((bool b) => OnChange(b));
    }

    public override void Refresh(object data)
    {
        toggle.SetIsOnWithoutNotify((bool)data);
    }
}
