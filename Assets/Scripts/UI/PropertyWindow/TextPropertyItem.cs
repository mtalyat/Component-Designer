using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TextPropertyItem : PropertyItem
{
    [SerializeField]
    private TMP_InputField textInput;

    public override Type storedType => typeof(string);

    private void Awake()
    {
        textInput.onValueChanged.AddListener((string s) => OnChange(s));
    }

    public override void Refresh(object data)
    {
        textInput.text = data.ToString();
    }
}
