using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;
using System.Linq;

public class PrefabData
{
    [SaveThis]
    public List<Block> blocks { get; set; } = new List<Block>();
    [SaveThis]
    public List<InputPeg> inputs { get; set; } = new List<InputPeg>();
    [SaveThis]
    public List<OutputPeg> outputs { get; set; } = new List<OutputPeg>();

    public PrefabData()
    {

    }

    public void Load()
    {
        foreach (BlockData b in blocks)
            b.Load();
        foreach (BlockData b in inputs)
            b.Load();
        foreach (BlockData b in outputs)
            b.Load();
    }

    private T CreateNew<T>(List<T> list) where T : BlockData, new()
    {
        T t = new T();
        t.order = list.Count;
        list.Add(t);
        return t;
    }

    public Block CreateNewBlock()
    {
        return CreateNew(blocks);
    }

    public InputPeg CreateNewInput()
    {
        return CreateNew(inputs);
    }

    public OutputPeg CreateNewOutput()
    {
        return CreateNew(outputs);
    }

    public void ReassignOrders()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].order = i;
        }

        for (int i = 0; i < inputs.Count; i++)
        {
            inputs[i].order = i;
        }

        for (int i = 0; i < outputs.Count; i++)
        {
            outputs[i].order = i;
        }
    }

    public void Save()
    {
        SortByOrders();

        foreach (BlockData b in blocks)
            b.Save();
        foreach (BlockData b in inputs)
            b.Save();
        foreach (BlockData b in outputs)
            b.Save();
    }

    private void SortByOrders()
    {
        blocks.Sort((x, y) => x.order.CompareTo(y.order));
        inputs.Sort((x, y) => x.order.CompareTo(y.order));
        outputs.Sort((x, y) => x.order.CompareTo(y.order));
    }
}