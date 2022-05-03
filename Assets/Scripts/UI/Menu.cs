using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SUCC;
using TMPro;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField modFolderPathInput;
    [SerializeField]
    private FileSelector fileSelector;
    [SerializeField]
    private Button loadButton;
    [SerializeField]
    private TMP_Text loadButtonText;
    [SerializeField]
    private ConfirmDialog confirmDialog;

    private void Start()
    {
        if (fileSelector != null && fileSelector.OnDialogueEnded != null)
        {
            fileSelector.OnDialogueEnded.AddListener((string s) =>
            {
                if (modFolderPathInput != null && s != null)
                {
                    //set with notify so the load button text changes
                    modFolderPathInput.text = s;
                }
            });
        }

        modFolderPathInput.onValueChanged.AddListener((string s) => SetLoadButtonText(s));

        fileSelector.defaultPath = Settings.defaultDirectory;
        modFolderPathInput.SetTextWithoutNotify(Settings.defaultDirectory);

        SetLoadButtonText(Settings.defaultDirectory);
    }

    private void SetLoadButtonText(string s)
    {
        if (!string.IsNullOrWhiteSpace(s))
        {
            if (Directory.Exists(s))
            {
                loadButtonText.text = "Load Mod";
            }
            else
            {
                loadButtonText.text = "Create Mod";
            }

            loadButton.interactable = true;
        }
        else
        {
            loadButtonText.text = "Invalid Path";

            loadButton.interactable = false;
        }
    }

    public void OnLoadClick()
    {
        string path = modFolderPathInput.text;

        GameManager.instance.SetLoadedData(new ModData(path));
        Hide();
    }

    public void Show()
    {
        transform.parent.gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void Quit()
    {
        AskToSaveThenQuit();
    }

    public void AskToSaveThenQuit()
    {
        //do not prompt to save if there is nothing to save
        if (GameManager.instance.currentlyLoadedData == null)
        {
            GameManager.instance.ForceQuit();

            return;
        }

        confirmDialog.Show(() =>
        {
            //accept
            GameManager.instance.SaveLoadedData();

            GameManager.instance.ForceQuit();
        },
        () =>
        {
            //cancel
            GameManager.instance.ForceQuit();
        }, "Would you like to save before you quit?", "Yes", "No");
    }
}
