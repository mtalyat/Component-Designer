using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;
using System.Linq;

public class ComponentObject : MonoBehaviour
{
    [SerializeField]
    private BlockObject blockPrefab;
    [SerializeField]
    private InputPegObject inputPegPrefab;
    [SerializeField]
    private OutputPegObject outputPegPrefab;

    private ComponentData currentlyLoadedData;

    private List<BlockObject> blocks = new List<BlockObject>();
    private List<InputPegObject> inputs = new List<InputPegObject>();
    private List<OutputPegObject> outputs = new List<OutputPegObject>();

    public void Initialize(ComponentData cd)
    {
        currentlyLoadedData = cd;

        //data is not null, so lets build the object

        blocks.Clear();
        for (int i = 0; i < cd.prefab.blocks.Count; i++)
        {
            AddBlock(cd.prefab.blocks[i]);
        }

        inputs.Clear();
        for (int i = 0; i < cd.prefab.inputs.Count; i++)
        {
            AddInput(cd.prefab.inputs[i]);
        }

        outputs.Clear();
        for (int i = 0; i < cd.prefab.outputs.Count; i++)
        {
            AddOutput(cd.prefab.outputs[i]);
        }
    }

    private BlockDataObject[] GetAllBlockDataObjectChildren()
    {
        return blocks.Cast<BlockDataObject>().Concat(inputs).Concat(outputs).ToArray();
    }

    public Bounds GetXZBounds()
    {
        BlockDataObject[] children = GetAllBlockDataObjectChildren();

        if(children.Length == 0)
        {
            return new Bounds();
        } else if (children.Length == 1)
        {
            return children[0].bounds;
        }

        float minX = children[0].bounds.min.x;
        float minZ = children[0].bounds.min.z;
        float maxX = children[0].bounds.min.x;
        float maxZ = children[0].bounds.min.z;

        Bounds b;
        Vector3 min;
        Vector3 max;

        //find min and max of all children for bounds
        for (int i = 1; i < children.Length; i++)
        {
            b = children[i].bounds;
            min = b.min;
            max = b.max;

            if(min.x < minX)
            {
                minX = min.x;
            }
            if(min.z < minZ)
            {
                minZ = min.z;
            }

            if(max.x > maxX)
            {
                maxX = max.x;
            }
            if(max.z > maxZ)
            {
                maxZ = max.z;
            }
        }

        //round outwards of 0, so it is not part of a grid space
        min = Helper.RoundOut(new Vector3(minX, 0f, minZ));
        max = Helper.RoundOut(new Vector3(maxX, 1f, maxZ));

        Vector3 size = max - min;
        Vector3 center = size / 2f + min;

        return new Bounds(center, size);
    }

    public void UpdateObject()
    {
        //update each visual to reflect changes
        foreach(BlockDataObject b in blocks)
        {
            b.UpdateObject();
        }
        foreach (BlockDataObject b in inputs)
        {
            b.UpdateObject();
        }
        foreach (BlockDataObject b in outputs)
        {
            b.UpdateObject();
        }
    }

    public void UpdateData()
    {
        foreach (BlockDataObject b in blocks)
        {
            b.UpdateData();
        }
        foreach (BlockDataObject b in inputs)
        {
            b.UpdateData();
        }
        foreach (BlockDataObject b in outputs)
        {
            b.UpdateData();
        }
    }

    public void DeleteSelected()
    {
        //rebuild lists to reflect additions/removals
        currentlyLoadedData.prefab.blocks.Clear();
        for (int i = blocks.Count - 1; i >= 0; i--)
        {
            if(blocks[i].isSelected)
            {
                GameObject go = blocks[i].gameObject;
                blocks.RemoveAt(i);
                Destroy(go);
                continue;
            }

            currentlyLoadedData.prefab.blocks.Add((Block)blocks[i].data);
        }

        currentlyLoadedData.prefab.inputs.Clear();
        for (int i = inputs.Count - 1; i >= 0; i--)
        {
            if (inputs[i].isSelected)
            {
                GameObject go = inputs[i].gameObject;
                inputs.RemoveAt(i);
                Destroy(go);
                continue;
            }

            currentlyLoadedData.prefab.inputs.Add((InputPeg)inputs[i].data);
        }

        currentlyLoadedData.prefab.outputs.Clear();
        for (int i = outputs.Count - 1; i >= 0; i--)
        {
            if (outputs[i].isSelected)
            {
                GameObject go = outputs[i].gameObject;
                outputs.RemoveAt(i);
                Destroy(go);
                continue;
            }

            currentlyLoadedData.prefab.outputs.Add((OutputPeg)outputs[i].data);
        }

        SelectionTools.instance.Hide();
        GameManager.instance.blockPropertyWindow.Clear();
    }

    private void Duplicate<T, S>(List<T> goList, List<S> dataList) where T : BlockDataObject, new() where S: BlockData, new()
    {
        for(int i = goList.Count - 1; i >= 0; i--)
        {
            if(goList[i].isSelected)
            {
                T original = goList[i];

                //unselect the old gameobject
                original.SetSelected(false);

                //duplicate the gameobject
                T t = Instantiate(original, original.transform.parent);

                //duplicate the data
                S s = (S)original.data.Duplicate();

                t.Initialize(s, this);

                t.transform.position += new Vector3(0f, original.bounds.size.y, 0f);

                //add the duplicated data
                dataList.Add(s);
                goList.Add(t);

                //reselect the new one
                t.SetSelected(true);
            }
        }
    }

    public void DuplicateSelected()
    {
        Duplicate(blocks, currentlyLoadedData.prefab.blocks);
        Duplicate(inputs, currentlyLoadedData.prefab.inputs);
        Duplicate(outputs, currentlyLoadedData.prefab.outputs);

        BlockDataObject.RefreshSelection();
    }

    //generic add method
    private void Add(BlockDataObject prefab, BlockData bd, IList list, bool select = false)
    {
        BlockDataObject bo = Instantiate(prefab, transform);
        bo.Initialize(bd, this);
        list.Add(bo);

        if(select)
        {
            BlockDataObject.Select(bo);
        }
    }

    private void AddBlock(Block bd, bool select = false)
    {
        BlockObject bo = Instantiate(blockPrefab, transform);
        bo.Initialize(bd, this);
        blocks.Add(bo);

        if(select)
        {
            BlockDataObject.Select(bo);
        }
    }

    private void AddInput(InputPeg bd, bool select = false)
    {
        InputPegObject bo = Instantiate(inputPegPrefab, transform);
        bo.Initialize(bd, this);
        inputs.Add(bo);

        if (select)
        {
            BlockDataObject.Select(bo);
        }
    }

    private void AddOutput(OutputPeg bd, bool select = false)
    {
        OutputPegObject bo = Instantiate(outputPegPrefab, transform);
        bo.Initialize(bd, this);
        outputs.Add(bo);

        if (select)
        {
            BlockDataObject.Select(bo);
        }
    }

    public void CreateNewBlock()
    {
        //add to data object, then instantiate something
        AddBlock(currentlyLoadedData.prefab.CreateNewBlock(), true);
    }

    public void CreateNewInput()
    {
        //add to data object, then instantiate something
        AddInput(currentlyLoadedData.prefab.CreateNewInput(), true);
    }

    public void CreateNewOutput()
    {
        //add to data object, then instantiate something
        AddOutput(currentlyLoadedData.prefab.CreateNewOutput(), true);
    }

    public void SelectAll()
    {
        foreach(var o in blocks)
        {
            BlockDataObject.Select(o, true);
        }
        foreach (var o in inputs)
        {
            BlockDataObject.Select(o, true);
        }
        foreach (var o in outputs)
        {
            BlockDataObject.Select(o, true);
        }
    }
}
