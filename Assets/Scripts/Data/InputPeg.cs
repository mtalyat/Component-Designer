using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;

public class InputPeg : Peg
{
    [SaveThis]
    [EditableProperty]
    public float length { get; set; } = 0.6f;

    public InputPeg() : base(Type.inputs) { }

    public override BlockData Duplicate()
    {
        return new InputPeg
        {
            blockType = Type.inputs,
            order = order,
            position = position,
            rotation = rotation,
            length = length,
        };
    }
}
