using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;

public class OutputPegObject : BlockDataObject
{
    public override int dataCount => 3;

    private OutputPeg dataInput => (OutputPeg)data;

    [SerializeField]
    private Color offColor = Color.black;

    [SerializeField]
    private Color onColor = Color.red;

    public override object Get(int index)
    {
        if (index == 0)
        {
            return data.position;
        }
        else if (index == 1)
        {
            return dataInput.rotation;
        } else if (index == 2)
        {
            return dataInput.startOn;
        }

        throw new System.ArgumentOutOfRangeException($"index was out of range (0 <= {index} < {dataCount}).");
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
            return "StartOn";
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

            dataInput.rotation = v;
            transform.eulerAngles = v;
        }
        else if (index == 2)
        {
            bool b = (bool)d;

            dataInput.startOn = b;
            renderer.material.color = b ? onColor : offColor;
        }
    }

    public override void UpdateData()
    {
        base.UpdateData();
    }
}
