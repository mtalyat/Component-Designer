using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.EventSystems;
using SUCC;
using System.Linq;

public abstract class BlockDataObject : MonoBehaviour
{
    private static List<BlockDataObject> objects = new List<BlockDataObject>();

    public BlockData data;

    public virtual int dataCount => 1;

    private Outline outline;

    private ComponentObject parent;

    public bool isSelected => outline.enabled;

    protected MeshRenderer renderer;

    public Bounds bounds => renderer.bounds;

    public bool scalable = false;

    protected virtual void Awake()
    {
        objects.Add(this);

        outline = GetComponentInChildren<Outline>();
        renderer = GetComponentInChildren<MeshRenderer>();
    }

    protected virtual void OnDestroy()
    {
        objects.Remove(this);
    }

    public void Initialize(BlockData data, ComponentObject compObj)
    {
        this.data = data;

        parent = compObj;

        //go through and reset each one- this way the prefabs will change to match the data
        UpdateObject();
    }

    protected void ReplaceMesh(int index)
    {
        //save if it is selected or not
        bool selected = outline.isActiveAndEnabled;

        //delete old mesh
        Destroy(transform.GetChild(0).gameObject);

        //instantiate the mesh at the index as the new child
        //also reset the renderer reference
        GameObject go = Instantiate(ObjectBuilder.instance.meshes[index], transform);
        renderer = go.GetComponent<MeshRenderer>();
        outline = go.GetComponent<Outline>();

        //select it if it was selected before
        if(selected)
        {
            Select(this);
        }
    }

    public abstract void Set(int index, object d);

    public abstract object Get(int index);

    public abstract string GetName(int index);

    public void SetSelected(bool sel)
    {
        outline.enabled = sel;
    }

    public static void Select(BlockDataObject obj, bool selectMultiple = false)
    {
        int selectedCount = 0;

        //unselect all others, select this one
        foreach (BlockDataObject bdo in objects)
        {
            //always set if not selecting mutliple (to ensure only one is selected)
            // OR if obj is null (sets all to false)
            // OR if we are selecting multiple, and this bdo matches the obj, so we want to set it to true
            if(!selectMultiple || obj == null || bdo == obj)
            {
                bdo.SetSelected(bdo == obj);
            }

            if(bdo.isSelected)
            {
                selectedCount++;
            }
        }

        //put the selected one in the block property window, since it has been selected
        //if none selected, clear
        if (selectedCount > 0)
        {
            if (selectedCount == 1)
            {
                //only one selected, show that data
                GameManager.instance.blockPropertyWindow.Initialize(obj.data.blockType.ToString(), obj.data);

                SelectionTools.instance.SetMatch(obj);
            }
            else
            {
                //multiple selected, show nothing
                GameManager.instance.blockPropertyWindow.Clear();

                SelectionTools.instance.AddMatch(obj);
            }
        }
        else
        {
            GameManager.instance.blockPropertyWindow.Clear();

            SelectionTools.instance.RemoveAllMatches();
        }
    }

    public static void RefreshSelection()
    {
        SelectionTools.instance.RemoveAllMatches();

        BlockDataObject obj = null;
        int selectedCount = 0;

        foreach(BlockDataObject bdo in objects)
        {
            if(bdo.isSelected)
            {
                selectedCount++;

                obj = bdo;

                SelectionTools.instance.AddMatch(bdo);
            }
        }

        //put the selected one in the block property window, since it has been selected
        //if none selected, clear
        if (selectedCount > 0)
        {
            if (selectedCount == 1)
            {
                //only one selected, show that data
                GameManager.instance.blockPropertyWindow.Initialize(obj.data.blockType.ToString(), obj.data);
            }
            else
            {
                //multiple selected, show nothing
                GameManager.instance.blockPropertyWindow.Clear();
            }
        }
        else
        {
            GameManager.instance.blockPropertyWindow.Clear();
        }
    }

    public void UpdateObject()
    {
        for (int i = 0; i < dataCount; i++)
        {
            Set(i, Get(i));
        }
    }

    public virtual void UpdateData()
    {
        Set(0, transform.position);
        Set(1, transform.eulerAngles);

        GameManager.instance.blockPropertyWindow.Refresh(data);
    }
}