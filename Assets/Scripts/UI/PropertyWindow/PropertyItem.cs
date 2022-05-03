using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public abstract class PropertyItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;

    protected string storedName { get; private set; }

    public abstract System.Type storedType { get; }

    private UnityEvent<object> onChange = new UnityEvent<object>();

    //these won't be cleared
    [SerializeField]
    private UnityEvent<object> onChangePermanent = new UnityEvent<object>();

    public virtual void Initialize(string name, object data)
    {
        SetNameText(name);

        Refresh(data);
    }

    public abstract void Refresh(object data);

    protected void SetNameText(string name)
    {
        storedName = name;
        nameText.text = GetProperName(name);
    }

    public virtual bool ConditionForSelection(object obj, System.Type other)
    {
        return storedType == other;
    }

    protected string GetProperName(string name)
    {
        return name + ":";
    }

    protected void SetNameTextVisibility(bool visible)
    {
        nameText.gameObject.SetActive(visible);
    }

    public void OnChange(object d)
    {
        onChange.Invoke(d);
        onChangePermanent.Invoke(d);
    }

    public void AddListener(UnityAction<object> call)
    {
        onChange.AddListener(call);
    }

    public void ClearListeners()
    {
        onChange.RemoveAllListeners();
    }

    protected float ParseFloat(string text)
    {
        float f;

        if(!float.TryParse(text, out f))
        {
            return 0f;
        }

        return f;
    }
}
