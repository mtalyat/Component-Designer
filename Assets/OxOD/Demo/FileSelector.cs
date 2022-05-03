using UnityEngine;
using System.Collections;
using OxOD;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Events;

public class FileSelector : MonoBehaviour
{
    [System.Serializable]
    public class FileSelectEvent : UnityEvent<string>
    {
    }

    [Header("OxOD Reference")]
    public FileDialog dialog;

    [Header("File Dialogue Options")]
    public string defaultPath;
    public FileDialog.FileDialogMode mode;
    public string extensions;
    public int maxSize = -1;
    public bool saveLastPath = true;

    [Header("Events")]
    public FileSelectEvent OnDialogueEnded;

    [Header("Internal References")]
    public InputField selectedFile;


    public string result { get; set; }

    public void SelectFile()
    {
        StartCoroutine(Select(string.IsNullOrEmpty(result) ? defaultPath : result));
    }

    public IEnumerator Select(string path)
    {
        if (mode == FileDialog.FileDialogMode.Open)
        {
            yield return StartCoroutine(dialog.Open(path, extensions, "OPEN FILE", null, maxSize, saveLastPath));
        }
        else if (mode == FileDialog.FileDialogMode.Save)
        {
            yield return StartCoroutine(dialog.Save(path, extensions, "SAVE FILE", null, saveLastPath));
        }
        else
        {
            yield return StartCoroutine(dialog.SelectFolder(path, "SELECT FOLDER", null, saveLastPath));
        }

        if (dialog.result != null)
        {
            result = dialog.result;
            if (selectedFile != null)
                selectedFile.text = new FileInfo(dialog.result).Name;

            OnDialogueEnded.Invoke(result);
        }
    }
}
