using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Vector3PropertyItem : PropertyItem
{
    [SerializeField]
    private TMP_InputField xInput;
    [SerializeField]
    private TMP_InputField yInput;
    [SerializeField]
    private TMP_InputField zInput;

    public override System.Type storedType => typeof(Vector3);

    private void Awake()
    {
        UnityAction<string> onNumberChange = (string s) => OnChange(GetVector3());

        xInput.onValueChanged.AddListener(onNumberChange);
        yInput.onValueChanged.AddListener(onNumberChange);
        zInput.onValueChanged.AddListener(onNumberChange);
    }

    private Vector3 GetVector3()
    {
        float x = ParseFloat(xInput.text);
        float y = ParseFloat(yInput.text);
        float z = ParseFloat(zInput.text);

        return new Vector3(x, y, z);
    }

    public override void Refresh(object data)
    {
        Vector3 v = (Vector3)data;

        xInput.text = v.x.ToString();
        yInput.text = v.y.ToString();
        zInput.text = v.z.ToString();
    }
}
