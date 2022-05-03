using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUCC;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public ModData currentlyLoadedData;

    public static GameManager instance;

    [SerializeField]
    private UIObjectList filesList;

    [SerializeField]
    private UIObjectList componentsList;

    public PropertyWindow componentPropertyWindow;
    public PropertyWindow blockPropertyWindow;

    private PropertyItem[] propertyItems;
    public PropertyItem defaultPropertyItem;

    [Header("UI References")]
    [SerializeField] private Button newComponentButton;
    [SerializeField] private Button deleteComponentButton;
    [SerializeField] private Button newFileButton;
    [SerializeField] private Button deleteFileButton;
    [SerializeField] private Button newBlockButton;
    [SerializeField] private Button newInputButton;
    [SerializeField] private Button newOutputButton;
    [SerializeField] private Button saveButton;

    [Space]

    [SerializeField] private TextDialog textDialog;
    [SerializeField] private ConfirmDialog confirmDialog;

    [Space]

    [SerializeField] private Menu menu;

    private bool quitting = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        propertyItems = Resources.LoadAll<PropertyItem>("Properties");
    }

    private void Start()
    {
        //quitting
        Application.wantsToQuit += () =>
        {
            if(!quitting)
            {
                menu.AskToSaveThenQuit();
            }

            //we want to close on our own accord, not when the button is pressed
            return quitting;
        };

        //when file list selects something, modify components list to match
        filesList.onSelectionChange.AddListener((int i) =>
        {
            bool validIndex = i >= 0;

            if (validIndex)
            {
                componentsList.Set(currentlyLoadedData.files[i].components.Values.ToArray());
                componentPropertyWindow.Clear();
            }
            else
            {
                componentsList.Clear();
            }

            newFileButton.interactable = true;
            newComponentButton.interactable = validIndex;
            deleteFileButton.interactable = validIndex;
        });

        //when a component has changed, update the display
        componentsList.onSelectionChange.AddListener((int i) =>
        {
            bool validIndex = i >= 0;

            if (validIndex)
            {
                //valid index, load that
                ObjectBuilder.instance.Build(currentlyLoadedData.files[filesList.selectedIndex].components.Values.ElementAt(i));
            }
            else
            {
                //invalid index
                ObjectBuilder.instance.Build(null);
            }

            deleteComponentButton.interactable = validIndex;
            newBlockButton.interactable = validIndex;
            newInputButton.interactable = validIndex;
            newOutputButton.interactable = validIndex;

            //regardless of object selection, hide tools
            SelectionTools.instance.Hide();
            //and also clear blocks property window
            blockPropertyWindow.Clear();
        });

        componentPropertyWindow.AddListenerToNameInput((string s) =>
        {
            if (HasSelectedComponentData() && s.CompareTo("--") != 0 && s.CompareTo(GetCurrentlySelectedComponentData().ToString()) != 0)
            {
                RenameSelectedComponent(componentPropertyWindow.nameInputText);

                //since we renamed something, we need to reload the list
                componentsList.UpdateSelected();
            }
        });
    }

    public PropertyItem GetPropertyItem(object obj)
    {
        System.Type type = obj.GetType();

        //find based on type
        foreach(PropertyItem item in propertyItems)
        {
            if(item.ConditionForSelection(obj, type))
            {
                return item;
            }
        }

        //no type, return window as default
        return defaultPropertyItem;
    }

    public void SetLoadedData(ModData data)
    {
        currentlyLoadedData = data;

        if(data != null)
        {
            filesList.Set(currentlyLoadedData.files.ToArray());
            saveButton.interactable = true;
        } else
        {
            filesList.Clear();
            saveButton.interactable = false;
        }
    }

    public void SaveLoadedData()
    {
        if(currentlyLoadedData != null)
        {
            currentlyLoadedData.Save();
        }
    }

    #region Files List

    public void CreateNewFile()
    {
        //do it with a dialog
        textDialog.Show((string s) =>
        {
            //create a new file
            currentlyLoadedData.CreateNewFile(s);

            //update list
            SetLoadedData(currentlyLoadedData);
        });
    }

    public void DeleteSelectedFile()
    {
        //do it with a dialog
        confirmDialog.Show(() =>
        {
            //remove the file
            currentlyLoadedData.DeleteFile(filesList.selectedIndex);

            //update list
            SetLoadedData(currentlyLoadedData);
        }, $"Are you sure you want to delete \"{currentlyLoadedData.files[filesList.selectedIndex].name}\"?");
    }

    #endregion

    #region Components List

    public void CreateNewComponent()
    {
        textDialog.Show((string s) =>
        {
            currentlyLoadedData.files[filesList.selectedIndex].CreateNewComponent(s);

            componentsList.Set(currentlyLoadedData.files[filesList.selectedIndex].components.Values.ToArray());
            StartCoroutine(SelectComponentNextFrame(currentlyLoadedData.files[filesList.selectedIndex].components.Values.Count - 1));
        });
    }

    private IEnumerator SelectComponentNextFrame(int index)
    {
        yield return null;

        componentsList.Select(index);
    }

    public void DeleteSelectedComponent()
    {
        //do it with a dialog
        confirmDialog.Show(() =>
        {
            string key = currentlyLoadedData.files[filesList.selectedIndex].components.Keys.ElementAt(componentsList.selectedIndex);

            currentlyLoadedData.files[filesList.selectedIndex].components.Remove(key);

            componentsList.Set(currentlyLoadedData.files[filesList.selectedIndex].components.Values.ToArray());
        }, $"Are you sure you want to delete \"{currentlyLoadedData.files[filesList.selectedIndex].components.Values.ElementAt(componentsList.selectedIndex).name}\"?");
    }

    private bool HasSelectedComponentData()
    {
        return filesList.selectedIndex >= 0 && componentsList.selectedIndex >= 0;
    }

    private ComponentData GetCurrentlySelectedComponentData()
    {
        return currentlyLoadedData.files[filesList.selectedIndex].components.ElementAt(componentsList.selectedIndex).Value;
    }

    public void RenameSelectedComponent(string newName)
    {
        if (!HasSelectedComponentData()) return;

        if (string.IsNullOrWhiteSpace(newName))
        {
            newName = "Unnamed Component";
        }

        //get the object at the key, and set the name
        ComponentData cd = GetCurrentlySelectedComponentData();

        //make sure this name does not exist
        if(currentlyLoadedData.files[filesList.selectedIndex].components.Any(c => c.Value.name.CompareTo(newName) == 0))
        {
            //hmmm... another item already has this name. Do not rename
            return;
        }

        cd.name = newName;
    }

    #endregion

    public void ReloadComponentPropertyWindow(ComponentData cd)
    {
        //set new data to property window
        componentPropertyWindow.Initialize(cd.name, cd);
    }

    public void ForceQuit()
    {
        quitting = true;
        Application.Quit();
    }

    public void ShowMenu()
    {
        menu.Show();
    }
}
