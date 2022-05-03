using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;

public class OutputPeg : Peg
{
    [SaveThis]
    [EditableProperty]
    public bool startOn { get; set; } = false;

    public OutputPeg() : base(Type.outputs) { }

    public override BlockData Duplicate()
    {
        return new OutputPeg
        {
            blockType = Type.outputs,
            order = order,
            position = position,
            rotation = rotation,
            startOn = startOn,
        };
    }
}
