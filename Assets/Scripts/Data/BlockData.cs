using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;

public abstract class BlockData
{
    public enum Type
    {
        blocks,
        inputs,
        outputs
    }

    [DontSaveThis]
    public Type blockType { get; set; }

    [SaveThis]
    [EditableProperty]
    public Vector3 position { get; set; } = new Vector3(0, 0, 0);

    [SaveThis]
    [EditableProperty]
    public Vector3 rotation { get; set; } = new Vector3(0, 0, 0);

    [DontSaveThis]
    [EditableProperty]
    public int order { get; set; } = 0;

    public BlockData(Type type)
    {
        blockType = type;
    }

    public virtual void Load()
    {

    }

    public virtual void Save()
    {

    }

    public abstract BlockData Duplicate();
}
