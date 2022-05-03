using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ColorPropertyItem : PropertyItem
{
    [SerializeField]
    private FlexibleColorPicker colorPicker;

    public override Type storedType => typeof(ByteColor);

    private Color selectedColor;

    private void Update()
    {
        if(colorPicker.color != selectedColor)
        {
            selectedColor = colorPicker.color;
            OnChange(new ByteColor(selectedColor));
        }
    }

    public override void Refresh(object data)
    {
        selectedColor = ((ByteColor)data).ToColor();

        colorPicker.startingColor = selectedColor;
        colorPicker.color = selectedColor;
    }
}
