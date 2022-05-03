using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Vector2PropertyItem : PropertyItem
{
    [SerializeField]
    private TMP_InputField xInput;
    [SerializeField]
    private TMP_InputField yInput;

    public override System.Type storedType => typeof(Vector2);

    private void Awake()
    {
        UnityAction<string> onNumberChange = (string s) => OnChange(GetVector2());

        xInput.onValueChanged.AddListener(onNumberChange);
        yInput.onValueChanged.AddListener(onNumberChange);
    }

    private Vector2 GetVector2()
    {
        float x = ParseFloat(xInput.text);
        float y = ParseFloat(yInput.text);

        return new Vector2(x, y);
    }

    public override void Refresh(object data)
    {
        Vector2 v = (Vector2)data;

        xInput.text = v.x.ToString();
        yInput.text = v.y.ToString();
    }
}
