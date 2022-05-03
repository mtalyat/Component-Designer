using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBuilder : MonoBehaviour
{
    public static ObjectBuilder instance;

    private ComponentObject currentlyLoadedObject;

    [SerializeField]
    private ComponentObject componentPrefab;

    [SerializeField]
    public GameObject[] meshes;
    /*
        Cube,
        BufferBody,
        BufferBody_Long,
        FlatQuad,
        OriginCube,

        BetterCube_OpenBottom,
     */

    public bool isBuildingObject => currentlyLoadedObject != null;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Build(ComponentData cd)
    {
        //kill the old object
        if(currentlyLoadedObject != null)
        {
            Destroy(currentlyLoadedObject.gameObject);
            currentlyLoadedObject = null;
        }

        if (cd == null)
        {
            //if no data, we are done
            return;
        }

        currentlyLoadedObject = Instantiate(componentPrefab, transform);
        currentlyLoadedObject.Initialize(cd);

        //load the property window
        GameManager.instance.ReloadComponentPropertyWindow(cd);
    }

    public void UpdateObject()
    {
        if(currentlyLoadedObject)
        {
            currentlyLoadedObject.UpdateObject();
        }
    }

    public void UpdateData()
    {
        if (currentlyLoadedObject)
        {
            currentlyLoadedObject.UpdateData();
        }
    }

    public void CreateNewBlock()
    {
        if(currentlyLoadedObject != null)
        {
            currentlyLoadedObject.CreateNewBlock();
        }
    }

    public void CreateNewInput()
    {
        if (currentlyLoadedObject != null)
        {
            currentlyLoadedObject.CreateNewInput();
        }
    }

    public void CreateNewOutput()
    {
        if (currentlyLoadedObject != null)
        {
            currentlyLoadedObject.CreateNewOutput();
        }
    }

    public Bounds GetComponentBounds()
    {
        return currentlyLoadedObject.GetXZBounds();
    }

    public void DeleteSelected()
    {
        if (currentlyLoadedObject)
        {
            currentlyLoadedObject.DeleteSelected();
        }
    }

    public void DuplicateSelected()
    {
        if (currentlyLoadedObject)
        {
            currentlyLoadedObject.DuplicateSelected();
        }
    }

    public void SelectAll()
    {
        if(currentlyLoadedObject)
        {
            currentlyLoadedObject.SelectAll();
            GameManager.instance.blockPropertyWindow.Clear();
        }
    }
}
