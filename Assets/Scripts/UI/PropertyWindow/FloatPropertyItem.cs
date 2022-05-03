using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FloatPropertyItem : PropertyItem
{
    [SerializeField]
    private TMP_InputField numberInput;

    public override Type storedType => typeof(float);

    private void Awake()
    {
        numberInput.onValueChanged.AddListener((string s) => OnChange(string.IsNullOrEmpty(s) ? 0 : float.Parse(s)));
    }

    public override void Refresh(object data)
    {
        numberInput.text = data.ToString();
    }
}
