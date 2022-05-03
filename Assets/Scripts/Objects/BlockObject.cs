using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;

public class BlockObject : BlockDataObject
{
    public override int dataCount => 5;

    private Block dataBlock => (Block)data;

    private int meshIndex = -1;

    protected override void Awake()
    {
        base.Awake();
    }

    public override object Get(int index)
    {
        if (index == 0)
        {
            return data.position;
        }
        else if (index == 1)
        {
            return data.rotation;
        }
        else if (index == 2)
        {
            return dataBlock.scale;
        }
        else if (index == 3)
        {
            return dataBlock.color;
        } else if (index == 4)
        {
            return dataBlock.meshType;
        }

        throw new System.ArgumentOutOfRangeException($"index was out of range (0 <= {index} < {dataCount}).");
    }

    public override void Set(int index, object d)
    {
        Vector3 v;

        if (index == 0)
        {
            v = (Vector3)d;

            data.position = v;
            transform.localPosition = v;
        }
        else if (index == 1)
        {
            v = (Vector3)d;

            data.rotation = v;
            transform.eulerAngles = v;
        }
        else if (index == 2)
        {
            v = (Vector3)d;

            dataBlock.scale = v;
            transform.localScale = v;
        }
        else if (index == 3)
        {
            dataBlock.color = (ByteColor)d;
            renderer.material.color = dataBlock.color.ToColor();
        } else if (index == 4)
        {
            int i = (int)d;

            //don't change if alread the same
            if (meshIndex == i) return;

            meshIndex = i;

            //otherwise we need to replace the mesh, it has changed
            ReplaceMesh(i);

            //set the color back
            renderer.material.color = dataBlock.color.ToColor();
        }
    }

    public override string GetName(int index)
    {
        if (index == 0)
        {
            return "Position";
        }
        else if (index == 1)
        {
            return "Rotation";
        }
        else if (index == 2)
        {
            return "Scale";
        }
        else if (index == 3)
        {
            return "Color";
        }
        else if (index == 4)
        {
            return "Mesh";
        }

        throw new System.ArgumentOutOfRangeException($"index was out of range (0 <= {index} < {dataCount}).");
    }

    public override void UpdateData()
    {
        Set(2, transform.localScale);

        base.UpdateData();
    }
}

