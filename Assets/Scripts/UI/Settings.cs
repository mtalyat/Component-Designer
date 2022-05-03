using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    [SerializeField] private TMP_InputField defaultDirectoryInput;

    public static string defaultDirectory
    {
        get
        {
            return PlayerPrefs.GetString("dd", "");
        }
        set
        {
            PlayerPrefs.SetString("dd", value);
        }
    }

    public void SetDefaultDirectory(string path)
    {
        defaultDirectory = path;
    }

    public string GetDefaultDirectory()
    {
        return defaultDirectory;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //ThemeColorSetter.SetColors();

        defaultDirectoryInput.SetTextWithoutNotify(defaultDirectory);
    }
}
