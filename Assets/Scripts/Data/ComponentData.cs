using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;
using SUCC.ParsingLogic;

public class ComponentData
{
    [DontSaveThis]
    public string name { get; set; } = "ComponentData";

    [SaveThis]
    [EditableProperty]
    public string column { get; set; } = "";
    [SaveThis]
    [EditableProperty]
    public string category { get; set; } = "";

    //not a property so it doesn't show up on the property window
    [SaveThis]
    public PrefabData prefab { get; set; } = new PrefabData();

    [EditableProperty]
    public string logicCode { get; set; } = "";

    [EditableProperty]
    public string logicScript { get; set; } = "";

    [EditableProperty]
    public string clientCode { get; set; } = "";

    [SaveThis]
    [EditableProperty]
    public PlacingRuleset placingRules { get; set; } = new PlacingRuleset();

    public ComponentData()
    {

    }

    public void Load()
    {
        if (column == null)
            column = "";
        if (category == null)
            category = "";
        if (logicCode == null)
            logicCode = "";
        if (logicScript == null)
            logicScript = "";
        if (clientCode == null)
            clientCode = "";

        ReassignOrders();

        prefab.Load();
    }

    public void Save()
    {
        prefab.Save();

        //set to null if empty so it doesn't appear in the SUCC
        if (string.IsNullOrWhiteSpace(column))
        {
            column = null;
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            category = null;
        }

        if (string.IsNullOrWhiteSpace(logicCode))
        {
            logicCode = null;
        }

        if (string.IsNullOrWhiteSpace(logicScript))
        {
            logicScript = null;
        }

        if (string.IsNullOrWhiteSpace(clientCode))
        {
            clientCode = null;
        }
    }

    private void ReassignOrders()
    {
        prefab.ReassignOrders();
    }

    public override string ToString()
    {
        return name;
    }
}















