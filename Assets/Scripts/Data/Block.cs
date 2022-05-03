using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;

public class Block : BlockData
{
    public enum MeshType : int
    {
        Cube,
        BufferBody,
        BufferBody_Long,
        FlatQuad,
        OriginCube,

        //BetterCube_OpenBottom,
    }

    [SaveThis]
    [EditableProperty]
    public ByteColor color { get; set; } = new ByteColor();

    [SaveThis]
    [EditableProperty]
    public Vector3 scale { get; set; } = Vector3.one;

    [SaveThis]
    public string mesh { get; set; } = "";

    [EditableProperty]
    public MeshType meshType { get; set; } = MeshType.Cube;

    public Block() : base(Type.blocks) { 

    }

    public override void Load()
    {
        base.Load();

        if(string.IsNullOrEmpty(mesh))
        {
            meshType = MeshType.Cube;
        } else
        {
            meshType = (MeshType)System.Enum.Parse(typeof(MeshType), mesh);
        }
    }

    public override void Save()
    {
        base.Save();

        if (meshType == MeshType.Cube)
        {
            mesh = null;
        }
        else
        {
            mesh = meshType.ToString();
        }
    }

    public override BlockData Duplicate()
    {
        return new Block
        {
            blockType = Type.blocks,
            order = order,
            position = position,
            rotation = rotation,
            scale = scale,
            color = new ByteColor(color.r, color.g, color.b),
            meshType = meshType,
            mesh = "",
        };
    }
}
