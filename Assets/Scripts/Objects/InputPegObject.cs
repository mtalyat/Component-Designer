using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;

public class InputPegObject : BlockDataObject
{
    public override int dataCount => 3;

    private InputPeg dataInput => (InputPeg)data;

    public override object Get(int index)
    {
        if (index == 0)
        {
            return data.position;
        }
        else if (index == 1)
        {
            return dataInput.rotation;
        }
        else if (index == 2)
        {
            return dataInput.length;
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
            return "Length";
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
            float f = (float)d;

            //length corresponds to the scale on the y axis
            dataInput.length = f;

            transform.localScale = new Vector3(1f, f, 1f);
        }
    }

    public override void UpdateData()
    {
        Set(2, transform.localScale.y);

        base.UpdateData();
    }
}
